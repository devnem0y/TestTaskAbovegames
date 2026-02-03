using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalResourceProvider : ResourceProvider
{
    private const string localPath = "Assets/Resources/TestPics";

    public LocalResourceProvider() {}

    public override async UniTask<Sprite> LoadSpriteAsync(string key)
    {
        var fullPath = Path.Combine(localPath, key);

        if (!File.Exists(fullPath)) return null;

        var bytes = await File.ReadAllBytesAsync(fullPath);
        var texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
