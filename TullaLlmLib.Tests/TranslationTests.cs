using Translate;

namespace TullaLlmLib.Tests;

public class TranslationTests
{
    [Fact]
    public async Task TranslateEnToDe()
    {
        var config = new TranslationConfig();
        var chat = new MockChat();

        var translateService = new TranslationHook(config, chat, "void");
        
        chat.TriggerMessage("Hello, how can I help you?", "Tulla");

        await translateService.OnMessageReceived();
        
        Assert.NotEmpty(chat.Messages);
    }

    [Fact]
    public async Task TranslateSendCommand()
    {
        var config = new TranslationConfig();
        var chat = new MockChat();

        var translateService = new TranslationHook(config, chat, "void");

        await translateService.OnMessageSent("Na wie geht's dir heut?");
        
        Assert.NotEmpty(chat.Messages);
    }
}
