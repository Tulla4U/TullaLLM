namespace TullaLLMLib.Util;

public interface IChatInteractor
{
    Task SendMessage(string name, string message, ChatType chatType);
}

public enum ChatType
{
    Party,
    Say
}
