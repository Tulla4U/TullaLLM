using OllamaSharp;

namespace TullaLLMLib.Util;

public static class LlmTools
{
    public static async Task<List<string>> GetAvailableModels(Uri ollamaEndpoint)
    {
        using var ollamaClient = new OllamaApiClient(ollamaEndpoint);
        return (await ollamaClient.ListLocalModelsAsync()).Select(x => x.ModelName).OfType<string>().ToList();
    }
}
