using Dalamud.Game.Text;

namespace TullaLLM.Util;

public interface IChatInteractor
{
    void SendMessage(string name, string message, XivChatType chatType);
}
