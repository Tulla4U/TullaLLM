# Tulla  ̶A̶i̶ LLM

A collection of my tinkering with local LLMs.

## Current Features
- Chat Bot
  - Web search
  - AniList lookup
  - Nyaa.si torrent search
- Translations
  - Send translated messages to say chat
  - Translate incoming messages

The code is jank as fuck, it's straight up just me dicking around with stuff.

## Requirements
- Ollama
- langSearch token (optional: for web search)

## Setup

Configuration is very much in a broken state, so you can't rely on the UI actually doing what you'd expect, thus I recommend making changes in the config json directly for now.

### Ollama
1. Go to Ollama Settings and enable "Expose Ollama to the network"
2. ChatBot: Open terminal and pull a model e.g. `ollama pull gemma4:latest`
3. Translation: Open terminal and pull a translation model e.g. `ollama pull translategemma:4b`

### Plugin
1. Add the repo `dasdasdasd`
2. Install Tulla LLM
3. Chat Bot
   1. Open `/llm-cfg` and enter your ollama endpoint default: `http://127.0.0.1:11434`
   2. Enter the name of your model e.g. `gemma4:latest`
   3. (Optional) Enter your langSearch token
4. Translation
   1. Open `/tl-cfg` and enter your ollama endpoint default: `http://127.0.0.1:11434`
   2. Enter the name of your model e.g. `translategemma:4b`
   3. Select the desired languages for incoming (sent to you) and outgoing (messages you send)
5. Disable and re-enable the plugin (This part is jank af, but I haven't implemented hot reloading yet)

This is the example I'm running on currently.

### Example Config
```json
{
  "$type": "TullaLLM.Configuration, TullaLLM",
  "Version": 0,
  "LlmSettings": {
    "$type": "TullaLLMLib.LlmSettings, TullaLLMLib",
    "BaseUrl": "http://127.0.0.1:11434",
    "Model": "gemma4:latest",
    "LangSearchApiToken": "LangSearchToken"
  },
  "TranslationConfig": {
    "$type": "Translate.TranslationConfig, Translate",
    "ModelName": "translategemma:4b",
    "OllamaUri": "http://127.0.0.1:11434",
    "IncomingLanguage": 0,
    "OutgoingLanguage": 1,
    "AlwaysTranslateIncomingMessages": false
  }
}
```
Language options are (maybe I'll just change that to free text, it's a LLM in the background anyway 🤷‍♀️)
```English = 0,
    Finnish = 1,
    German = 2,
    Norwegian = 3,
    Danish = 4,
    French = 5,
    Dutch = 6,
    Polish = 7,
    Spanish = 8,
 ```
