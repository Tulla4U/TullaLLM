using System;
using System.Threading.Tasks;
using Dalamud.Game.Text;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using TullaLLM.Tools;
using TullaLLM.Util;

namespace TullaLLM;

public class AnswerBot : IDisposable
{
    private readonly LlmSettings settings;
    private readonly AIAgent agent;
    private readonly WebSearchTool search;
    private readonly IChatInteractor chatInteractor;

    public AnswerBot(Configuration config) : this(config, new ChatInteractor())
    { }

    public AnswerBot(Configuration config, IChatInteractor chatInteractor)
    {
        this.chatInteractor = chatInteractor;
        search = new(config.LlmSettings.LangSearchApiToken);
        settings = config.LlmSettings;
        agent = new OllamaApiClient(new Uri(settings.BaseUrl), settings.Model)
            .AsAIAgent(
                instructions: $"Today is: {DateTime.Now} and you are a straight to the point helper that provides short concise answers to the user. You do not add any fluff or praise. Do not use any markdown or other forms of formatting. Plain text only. Use the DoSearch tool. Use the GetWebPage tool to load the content of a website.",
                tools: [AIFunctionFactory.Create(search.DoSearch), new HostedWebSearchTool(), AIFunctionFactory.Create(WebPageTool.GetWebPage)]);
    }

    private AgentSession? session;

    public async Task Query(string query)
    {
        try
        {
            if (query.StartsWith("reset"))
            {
                query = query["reset".Length..];
                if (session is not null)
                {
                    session = await agent.CreateSessionAsync().ConfigureAwait(false);
                }
            }
            chatInteractor.SendMessage("User", query, XivChatType.Party);
            session ??= await agent.CreateSessionAsync().ConfigureAwait(false);
            var response = await agent.RunAsync(query, session).ConfigureAwait(false);
            chatInteractor.SendMessage("LLM", response.Text, XivChatType.Party);
        }
        catch (Exception ex)
        {
            // Plugin.Log.Error(ex.Message);
        }
    }

    public void Dispose()
    {
        search.Dispose();
    }
}
