using System;
using System.Threading.Tasks;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using TullaLLMLib.Util;

namespace Translate;

public class TranslationHook
{
    private TranslationConfig config;
    private readonly IChatInteractor chatInteractor;
    private MessageReceivedEventArgs? lastMessage ;
    private string playerName;
    private AIAgent? client;
    
    public TranslationHook(TranslationConfig config, IChatInteractor chatInteractor, string playerName)
    {
        this.config = config;
        this.chatInteractor = chatInteractor;
        this.playerName = playerName;
        chatInteractor.OnMessageReceived += CacheLastMessage;
    }

    private void CacheLastMessage(object? sender, MessageReceivedEventArgs e)
    {
        if (e.Sender != playerName)
        {
            lastMessage = e;
        }
    }

    public async Task OnMessageSent(string message)
    {
        CreateClient();
        if (client is null)
        {
            return;
        }
 
        var session = await client.CreateSessionAsync();
        var translation = 
            await client.RunAsync($"Translate this message into {config.OutgoingLanguage.ToString()}: {message}", session);
        // await chatInteractor.PrintLocalMessage("Tester", translation.Text.TrimEnd(), ChatType.Party);
        chatInteractor.SendGameMessage(translation.Text.TrimEnd(), ChatType.Say);
    }

    public async Task OnMessageReceived()
    {
        if (lastMessage is null)
        {
            return;
        }
        CreateClient();
        if (client is null)
        {
            return;
        }

 
        var session = await client.CreateSessionAsync();
        var translation =
            await client.RunAsync($"Translate this message into {config.IncomingLanguage.ToString()}: {lastMessage.Message}", session);
        await chatInteractor.PrintLocalMessage(lastMessage.Sender, translation.Text.TrimEnd(), ChatType.Party);
        
    }

    private void CreateClient()
    {
        if (client is not null)
        {
            return;
        }

        var options = new ChatOptions
            {
                ToolMode = ChatToolMode.None,
                MaxOutputTokens = 10000,
                Instructions =
                    "You are a translator. Respond only with the translated message in the language requested. Be casual where possible, these aren't overly formal conversations. Messages must be shorter than 255 characters long.",
                ResponseFormat = new ChatResponseFormatText()
            };
        var chatOptions = new ChatClientAgentOptions
        {
            ChatOptions = options,
            Name = "Translator",
        };
        client = new OllamaApiClient(new OllamaApiClient.Configuration
            {
                Model = config.ModelName,
                Uri = new Uri(config.OllamaUri)
            })
            .AsAIAgent(chatOptions);
    }
}
