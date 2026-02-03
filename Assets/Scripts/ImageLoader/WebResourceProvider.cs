using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebResourceProvider : ResourceProvider
{
    private readonly string _baseUrl;
    private readonly Dictionary<string, Sprite> _cache = new();

    public WebResourceProvider(string url)
    {
        _baseUrl = url;
    }

    public override async UniTask<Sprite> LoadSpriteAsync(string fileName)
    {
        var url = _baseUrl + fileName;
        var key = Path.GetFileName(url);

        if (_cache.TryGetValue(key, out var sprite)) return sprite;

        using var request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();

        while (!operation.isDone) await UniTask.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Ошибка загрузки: {url}");
            return null;
        }

        var texture = DownloadHandlerTexture.GetContent(request);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        _cache[key] = sprite;

        return sprite;
    }
}
