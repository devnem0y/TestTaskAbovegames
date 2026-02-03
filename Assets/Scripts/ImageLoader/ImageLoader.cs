using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImageLoader
{
    private readonly ResourceProvider _provider;
    private readonly Dictionary<int, Sprite> _cache;

    public ImageLoader(string baseUrl)
    {
#if UNITY_EDITOR
        _provider = new LocalResourceProvider();
#else
        _provider = new WebResourceProvider(baseUrl);
#endif
        
        _cache = new Dictionary<int, Sprite>();
    }

    public async void LoadSprites(List<int> itemIds, Dictionary<int, Item> items, Action<int, Sprite> onLoaded)
    {
        foreach (var id in itemIds)
        {
            var sprite = await LoadSprite(id);
            onLoaded?.Invoke(id, sprite);
        }
    }

    public async void LoadSprite(int itemId, Action<int, Sprite> onLoaded)
    {
        var sprite = await LoadSprite(itemId);
        onLoaded?.Invoke(itemId, sprite);
    }
    
    private async UniTask<Sprite> LoadSprite(int itemId)
    {
        if (_cache.TryGetValue(itemId, out var cached)) return cached;

        var sprite = await TryLoadWithRetry(itemId, 2);
        if (sprite != null) _cache[itemId] = sprite;
        
        return sprite;
    }

    private async UniTask<Sprite> TryLoadWithRetry(int itemId, int maxRetries)
    {
        for (var attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await _provider.LoadSpriteAsync($"{itemId}.jpg");
            }
            catch (Exception e)
            {
                if (attempt == maxRetries) Debug.LogError($"Failed to load {itemId}.jpg: {e.Message}");
                else await UniTask.Delay(300);
            }
        }
        
        return null;
    }
}
