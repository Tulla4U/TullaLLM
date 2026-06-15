using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;

namespace TullaLLM.Util;

public class ChatInteractor: IChatInteractor
{
    public void SendMessage(string name, string message, XivChatType chatType)
    {
        if (message.Length > 450)
        {
            var subMessages = ChunkMessages(message);
            WriteMessageToClient(name, subMessages[0]?.AsSpan().ToString()!, chatType);
            
            foreach (var subMessage in subMessages.Skip(1).Select(x => x?.AsSpan().ToString()))
            {
                if (string.IsNullOrEmpty(subMessage)) continue;
                WriteMessageToClient(string.Empty, subMessage, chatType);
            }
        }
        else
        {
            WriteMessageToClient(name, message, chatType);
        }
    }

    private readonly char[] endCharacters = ['.', '?', '!', '"'];
    private List<string> ChunkMessages(string message)
    {
        var chunks = new List<string>();
        do
        {
            var lastIndexOfEndChar = message[..(Math.Min(450, message.Length))].LastIndexOfAny(endCharacters);
            var subMessage = message[..(lastIndexOfEndChar+1)];
            chunks.Add(subMessage);
            message = message[subMessage.Length..];
        }
        while (message.Length > 0);

        return chunks;
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
}
