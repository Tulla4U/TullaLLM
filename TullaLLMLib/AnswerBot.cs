using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using TullaLLMLib.Tools;
using TullaLLMLib.Util;

namespace TullaLLMLib;

public class AnswerBot : IDisposable
{
    private readonly LlmSettings settings;
    private readonly AIAgent agent;
    private readonly WebSearchTool search;
    private readonly IChatInteractor chatInteractor;
    private readonly AniListTools aniListTools = new();
    
    public AnswerBot(LlmSettings config, IChatInteractor chatInteractor)
    {
        this.chatInteractor = chatInteractor;
        search = new WebSearchTool(config.LangSearchApiToken);
        settings = config;
        var chatOptions = new ChatClientAgentOptions
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
            AIContextProviders = [new TextSearchProvider(search.SearchAdapter, new TextSearchProviderOptions
            {
                SearchTime = TextSearchProviderOptions.TextSearchBehavior.OnDemandFunctionCalling
            })]
        };
        agent = new OllamaApiClient(new OllamaApiClient.Configuration
            {
                Model = settings.Model,
                Uri = new Uri(settings.BaseUrl)
            })
            .AsAIAgent(chatOptions);
    }

    private const string Instruction = """

                                           you are a straight to the point helper that provides short concise answers to the user. 
                                           Answer questions using the provided context.
                                           You do not add any fluff or praise. 
                                           Do not use any markdown or other forms of formatting. Plain text only. 
                                           Use the GetWebPage tool to load the content of a website.
                                           When searching for anime information use the GetAnimeOrManga tool.
                                           You can find trending anime and manga with the GetTrendingAnimeOrManga tool.
                                           You can find torrents for anime using the GetNyaaTorrents tool, when doing so always provide the magnetlink and pick the torrent with 1080p resolution and the most seeds possible unless instructed otherwise.

                                       """;

    private AgentSession? session;

    public async Task Query(string query)
    {
        try
        {
            // if (query.StartsWith("reset"))
            // {
            //     query = query["reset".Length..];
            //     if (session is not null)
            //     {
            //         session = await CreateSession();
            //     }
            // }
            await chatInteractor.SendMessage("User", query, ChatType.Party);
            session ??= await CreateSession();
            var response = await agent.RunAsync(query, session);
            await chatInteractor.SendMessage("LLM", response.Text, ChatType.Party);
        }
        catch (Exception ex)
        {
            // Plugin.Log.Error(ex.Message);
        }
    }

    private async Task<AgentSession> CreateSession()
    {
        session = await agent.CreateSessionAsync();
        await agent.RunAsync("Get Ready for user questions. Do not include this message in your history.", session);
        return session;
    }

    public void Dispose()
    {
        search.Dispose();
    }
}
