namespace Translate;

public class TranslationConfig
{
    public string ModelName { get; set; } = "translategemma:4b";
    public string OllamaUri { get; set; } = "http://127.0.0.1:11434";
    public Language IncomingLanguage { get; set; } = Language.German;
    public Language OutgoingLanguage { get; set; } = Language.English;
    public bool AlwaysTranslateIncomingMessages { get; set; }
}

public enum Language
{
    English = 0,
    Finnish = 1,
    German = 2,
    Norwegian = 3,
    Danish = 4,
    French = 5,
    Dutch = 6,
    Polish = 7,
    Spanish = 8,
}
