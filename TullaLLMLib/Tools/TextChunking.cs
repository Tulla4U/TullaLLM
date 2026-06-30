namespace TullaLLMLib.Tools;

public static class TextChunking
{
    private static readonly string[] EndCharacters = [". ", "? ", "! ", "\" ", "\n"];

    public static List<string> ChunkText(string text, int maxLenght)
    {
        var chunks = new List<string>();
        do
        {
            var lastIndexOfEndChar = text[..Math.Min(maxLenght, text.Length)].LastIndexOfAny(EndCharacters);
            lastIndexOfEndChar = lastIndexOfEndChar == -1 ? Math.Min(maxLenght, text.Length - 1) : lastIndexOfEndChar;
            var subMessage = text[..(lastIndexOfEndChar + 1)];
            text = text[subMessage.Length..];
            chunks.Add(subMessage.Trim());
        }
        while (text.Length > 0);

        return chunks;
    }

    public static int LastIndexOfAny(this string text, string[] values)
    {
        var index = -1;
        foreach (var value in values)
        {
            index = Math.Max(index, text.LastIndexOf(value, StringComparison.Ordinal));
        }

        return index;
    }
}
