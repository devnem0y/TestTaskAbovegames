using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ResourceProvider
{
    public abstract UniTask<Sprite> LoadSpriteAsync(string key);
}