using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using TullaLLMLib.Tools;
using TullaLLMLib.Util;

namespace TullaLLMLib;

public class AnswerBot : IDisposable
{
    private readonly LlmSettings settings;
    private AIAgent agent;
    private readonly WebSearchTool? search;
    private readonly IChatInteractor chatInteractor;
    private readonly AniListTools aniListTools = new();
    private bool ready;
    
    public AnswerBot(LlmSettings config, IChatInteractor chatInteractor)
    {
        this.chatInteractor = chatInteractor;
        if (!string.IsNullOrEmpty(config.LangSearchApiToken))
        {
            
            search = new WebSearchTool(config.LangSearchApiToken);
        }
        settings = config;
        if (!string.IsNullOrEmpty(config.Model) && !string.IsNullOrEmpty(config.BaseUrl))
        {
#pragma warning disable MAAI001
            agent = CreateAgent(config.Model);
#pragma warning restore MAAI001
        }
    }

    private const string Instruction = """

                                           you are a straight to the point helper that provides short concise answers to the user.
                                           You are running as a plugin in FinalFantasy XIV, assume questions about places and activites are relating to the game.
                                           Answer questions using the provided context.
                                           You do not add any fluff or praise. 
                                           Do not use any markdown or other forms of formatting. Plain text only. 
                                           Use the GetWebPage tool to load the content of a website.
                                           When searching for anime information use the GetAnimeOrManga tool.
                                           You can find trending anime and manga with the GetTrendingAnimeOrManga tool.
                                           You can find torrents for anime using the GetNyaaTorrents tool, when doing so always provide the magnetlink and pick the torrent with 1080p resolution and the most seeds possible unless instructed otherwise, Get the closest match to the query, if no results turn up look up info on AniList and try the romaji or english name.

                                       """;

    private AgentSession? session;

    public async Task Query(string query)
    {
        if (!ready) return;
        try
        {
            if (query.StartsWith("reset"))
            {
                query = query["reset".Length..];
                if (session is not null)
                {
                    session = await CreateSession();
                }
            }
            await chatInteractor.PrintLocalMessage("User", query, ChatType.Party);
            session ??= await CreateSession();
            var response = await agent.RunAsync(query, session);
            await chatInteractor.PrintLocalMessage("LLM", response.Text, ChatType.Party);
        }
        catch (Exception ex)
        {
            // Plugin.Log.Error(ex.Message);
        }
    }

    private async Task<AgentSession> CreateSession()
    {
        session = await agent.CreateSessionAsync();
        // await agent.RunAsync("Get Ready for user questions. Do not include this message in your history.", session);
        return session;
    }

    [Experimental("MAAI001")]
    private AIAgent CreateAgent(string modelName)
    {
        IEnumerable<AIContextProvider> contextProviders = search is null
                                                              ? []
                                                              :
                                                              [
                                                                  new TextSearchProvider(
                                                                      search.SearchAdapter,
                                                                      new TextSearchProviderOptions
                                                                      {
                                                                          SearchTime = TextSearchProviderOptions
                                                                              .TextSearchBehavior
                                                                              .OnDemandFunctionCalling
                                                                      })
                                                              ];
        var chatOptions = new HarnessAgentOptions
        {
            ChatOptions = new ChatOptions
            {
                AllowMultipleToolCalls = true,
                MaxOutputTokens = 10000,
                Reasoning = new ReasoningOptions
                {
                    Effort = ReasoningEffort.High,
                    Output = ReasoningOutput.None
                },
                Instructions = $"Today is: {DateTime.Now} and {Instruction}",
                ResponseFormat = new ChatResponseFormatText(),
                Tools =
                [
                    AIFunctionFactory.Create(WebPageTool.GetWebPage),
                    AIFunctionFactory.Create(aniListTools.GetAnimeOrManga),
                    AIFunctionFactory.Create(aniListTools.GetTrendingAnimeOrManga),
                    AIFunctionFactory.Create(NyaaTool.GetNyaaTorrents)
                ]
            },
            Name = "TullaLLM",
            AIContextProviders = contextProviders,
            DisableWebSearch = search is null
        };
        ready = true;
        var harnessAgent = new OllamaApiClient(new OllamaApiClient.Configuration
            {
                Model = modelName,
                Uri = new Uri(settings.BaseUrl)
            })
            .AsHarnessAgent(chatOptions);
        return new LoopAgent(
            harnessAgent,
            new DelegateLoopEvaluator(async (context, cancellationToken) =>
            {
                var todoProvider = context.Agent.GetService<TodoProvider>()
                                   ?? throw new InvalidOperationException(
                                       "The agent did not expose a TodoProvider.");
                var remaining =
                    await todoProvider
                          .GetRemainingTodosAsync(context.Session, cancellationToken)
                          .ConfigureAwait(false);
                return remaining.Count > 0
                           ? LoopEvaluation.Continue(
                               $"Not all todos are complete yet ({remaining.Count} remaining). Please complete the remaining todo items.")
                           : LoopEvaluation.Stop();
            }),
            new LoopAgentOptions
            {
                MaxIterations = 5
            });
        // .AsAIAgent(chatOptions);
    }

    public void SelectModel(string modelName)
    {
        if (!string.IsNullOrEmpty(settings.BaseUrl))
        {
            return;
        }
#pragma warning disable MAAI001
        agent = CreateAgent(modelName);
#pragma warning restore MAAI001
    }

    public void Dispose()
    {
        search.Dispose();
    }
}
