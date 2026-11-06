using TullaLLMLib.Util;

namespace TullaLlmLib.Tests;

class MockChat : IChatInteractor
{
    public List<string> Messages { get; set; } = [];

    public event EventHandler<MessageReceivedEventArgs>? OnMessageReceived;

    public void TriggerMessage(string text, string user)
    {
        OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(text, user));
    }

    public Task PrintLocalMessage(string name, string message, ChatType chatType)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }

    public void SendGameMessage(string message, ChatType chatType)
    {
        throw new NotImplementedException();
    }
}
