using System.Text.Json;
using TullaLLMLib;
using TullaLLMLib.Tools;

namespace TullaLlmLib.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var searchTool = new WebSearchTool("");
        // var result = await searchTool.DoSearch("How to get to Phaera in FFXIV?");
        var search = await searchTool.DoSearch("Final FantasyXIV");
        var parsed = JsonSerializer.Deserialize<SearchResult>(search);
    }

    [Fact]
    public async Task AgentTest()
    {
        var config = new LlmSettings
        {
            Model = "gemma4:latest",
            LangSearchApiToken = "sk-4860fa0146f34d069a21000cb6cd638b"
        };
        var chat = new MockChat();
        var answerBot = new AnswerBot(config, chat);

        await answerBot.Query("Look up how to get to Phaera in FFXIV");
    }

    [Fact]
    public async Task NyaaTest()
    {
        var config = new LlmSettings
        {
            Model = "gemma4:latest",
            LangSearchApiToken = "sk-4860fa0146f34d069a21000cb6cd638b"
        };
        var chat = new MockChat();
        var answerBot = new AnswerBot(config, chat);

        await answerBot.Query("Get me the torrent for Zombieland Saga");
    }

    [Fact]
    public void Chunk()
    {
        var input = "Here are the available torrents for Zombieland Saga\n\n[ASW] Zombieland Saga Movie - Yumeginga Paradise [1080p HEVC x265 10Bit][AAC]\nMagnet Link: magnet:?xt=urn:btih:d1cb4a93b50caf112c8df04aed8a9ec7300258f2&dn=[ASW] Zombieland Saga Movie - Yumeginga Paradise [1080p HEVC x265 10Bit][AAC]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SubsPlease] Zombieland Saga Movie - Yumeginga Paradise (1080p) [FC2D55BF].mkv\nMagnet Link: magnet:?xt=urn:btih:ff6aea4bd5cbb27fb837b7eb18b4f2fc4fedfe99&dn=[SubsPlease] Zombieland Saga Movie - Yumeginga Paradise (1080p) [FC2D55BF].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SubsPlease] Zombieland Saga Movie - Yumeginga Paradise (720p) [FC2D55BF].mkv\nMagnet Link: magnet:?xt=urn:btih:5839ba26eb1cb61248d30a2d546a6f8ddae915da&dn=[SubsPlease] Zombieland Saga Movie - Yumeginga Paradise (720p) [FC2D55BF].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[Doyansu] Zombieland Saga: Yumeginga Paradise (BD 1080p HEVC Opus DTS-HD MA)\nMagnet Link: magnet:?xt=urn:btih:675888e08b74008c2ef392525ffaed8d574724da&dn=[Doyansu] Zombieland Saga: Yumeginga Paradise (BD 1080p HEVC Opus DTS-HD MA)&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[WBDP] Zombieland Saga (Complete) (S1, S2) [BD][1080p-FLAC][HEVC]\nMagnet Link: magnet:?xt=urn:btih:7cd5957cdacc3ba7ba2737418b4147379dec5a21&dn=[WBDP] Zombieland Saga (Complete) (S1, S2) [BD][1080p-FLAC][HEVC]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga Season 2 (Revenge) (1-12) [BD][1080p HEVC Opus DTS-HD MA]\nMagnet Link: magnet:?xt=urn:btih:659c4b6c06a516cb81de1b5fec38b74699819f7f&dn=[Sav1or] Zombieland Saga + Zombieland Saga Revenge (Zombie Land Saga S1 + S2) (EN|ES|RU|PT|IT|DE|FR|AR) [BD][1080p][AV1][OPUS][Dual Audio]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[EMBER] Zombieland Saga (2018-2021) (Season 1 2) [BDRip] [1080p Dual Audio HEVC 10 bits] (Batch)\nMagnet Link: magnet:?xt=urn:btih:a868a68d34d9013fb74203bff8b24d1af033483d&dn=Zombie.Land.Saga.S01.v2.1080p.BluRay.10-Bit.Dual-Audio.FLAC5.1.x265-YURASUKA (Zombieland Saga)&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[EMBER] Zombieland Saga (2021) (Season 2) [BDRip] [1080p Dual Audio HEVC 10 bits] (Zombieland Saga: Revenge | Zombie Land Saga Revenge) (Batch)\nMagnet Link: magnet:?xt=urn:btih:ad536cee64342000d9147c4078226c1389caea9b&dn=[EMBER] Zombieland Saga (2021) (Season 2) [BDRip] [1080p Dual Audio HEVC 10 bits] (Zombieland Saga: Revenge | Zombie Land Saga Revenge) (Batch)&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga Season 2 (Revenge) (1-12) [480p][Batch]\nMagnet Link: magnet:?xt=urn:btih:88fdd34367be1981ed297103b75c71c639bde11d&dn=[SSA] Zombieland Saga Season 2 (Revenge) (1-12) [480p][Batch]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[feat] Zombieland Saga S01 BluRay AAC 2.0 x265-TPN[Dual Audio][Multi-subs]\nMagnet Link: magnet:?xt=urn:btih:fbb2519d5cc01e53f4c8820fa27d3b48041f9670&dn=Zombieland Saga 2018 S01 BluRay AAC 2.0 x265-TPN[Dual Audio][Multi-subs]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[Anime Time] Zombieland Saga (Season 2) [BD][1080p][HEVC 10bit x265][AAC][Eng Sub]\nMagnet Link: magnet:?xt=urn:btih:4b15cc756ed77ed8c7ac8627eed82674b8acafb4&dn=[Anime Time] Zombieland Saga (Season 2) [BD][1080p][HEVC 10bit x265][AAC][Eng Sub]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[THE] Zombieland Saga (2021) (Season 2) [BDRip] [1080p Dual Audio HEVC 10 bits] (Zombieland Saga: Revenge | Zombie Land Saga Revenge) (Batch)\nMagnet Link: magnet:?xt=urn:btih:b71f89254276ac2935e32c8a3b1eec1ce652feb4&dn=[SubsPlease] Zombieland Saga Movie - Yumeginga Paradise (720p) [FC2D55BF].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[Zombieland Saga S02 - 11 [480p].mkv]\nMagnet Link: magnet:?xt=urn:btih:7ed674a3530da4ab34cec269448b61afb75249ec&dn=[SSA] Zombieland Saga S2 - 11 [480p].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga Season 2 (Revenge) (1-12) [1080p][Batch]\nMagnet Link: magnet:?xt=urn:btih:e779d6e73d048b826b4a42c9394aca0143d62eea&dn=[SSA] Zombieland Saga Season 2 (Revenge) (1-12) [1080p][Batch]&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[Zombieland Saga S2 - 09 [720p].mkv]\nMagnet Link: magnet:?xt=urn:btih:e6a128ba46cfa7889112bf1450a49a8acb5e5df0&dn=[SSA] Zombieland Saga S2 - 09 [720p].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga S2 - 07 [1080p].mkv\nMagnet Link: magnet:?xt=urn:btih:6a38490a535a74c2f8d33cce24bad115818d183a&dn=[SSA] Zombieland Saga S2 - 07 [1080p].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga S2 - 07 [720p].mkv\nMagnet Link: magnet:?xt=urn:btih:3894025951bcd1edd866b1bae975840479f87bde&dn=[SSA] Zombieland Saga S2 - 07 [720p].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga S2 - 07 [1080p].mkv\nMagnet Link: magnet:?xt=urn:btih:8a0a664fbb2a79d7aa8533580b20ea3b5566137a&dn=[SSA] Zombieland Saga S2 - 07 [1080p].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce\n\n[SSA] Zombieland Saga S2 - 08 [720p].mkv\nMagnet Link: magnet:?xt=urn:btih:3b5ce63ad705aada3f7184b42201b3cfbd481320&dn=[SubsPlease] Zombieland Saga S2 - 09 (720p) [74F0E363].mkv&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://open.stealth.si:80/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://exodus.desync.com:6969/announce&tr=udp://tracker.torrent.eu.org:451/announce";
        var chunks = TextChunking.ChunkText(input, 450);
    }
}
