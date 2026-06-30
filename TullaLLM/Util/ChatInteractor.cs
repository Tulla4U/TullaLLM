using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game.Text;
using TullaLLMLib.Tools;
using TullaLLMLib.Util;

namespace TullaLLM.Util;

public class ChatInteractor: IChatInteractor
{
    public async Task SendMessage(string name, string message, ChatType chatType)
    {
        if (message.Length > 450)
        {
            var subMessages = TextChunking.ChunkText(message, 450);
            WriteMessageToClient(name, subMessages[0]?.AsSpan().ToString()!, GetXivChatType(chatType));
            
            foreach (var subMessage in subMessages.Skip(1).Select(x => x?.AsSpan().ToString()))
            {
                await Task.Delay(100);
                if (string.IsNullOrEmpty(subMessage)) continue;
                WriteMessageToClient(string.Empty, subMessage, GetXivChatType(chatType));
            }
        }
        else
        {
            WriteMessageToClient(name, message, GetXivChatType(chatType));
        }
    }
    

    private static void WriteMessageToClient(string name, string message, XivChatType chatType)
    {
        Plugin.ChatGui.Print(new XivChatEntry
        {
            Message = message,
            Type = chatType,
            Name = name
        });
    }

    private XivChatType GetXivChatType(ChatType chatType) => chatType switch
    {
        ChatType.Party => XivChatType.Party,
        ChatType.Say => XivChatType.Say,
        _ => XivChatType.Party
    };
}
