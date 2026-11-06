namespace TullaLLMLib.Util;

public interface IChatInteractor
{
    event EventHandler<MessageReceivedEventArgs>? OnMessageReceived;
    Task PrintLocalMessage(string name, string message, ChatType chatType);
    void SendGameMessage(string message, ChatType chatType);
}

public enum ChatType
{
    Party,
    Say
}


public record MessageReceivedEventArgs(string Message, string Sender);
