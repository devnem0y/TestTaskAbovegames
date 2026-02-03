using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorPanel : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Vector2 _dotSize;

    private readonly List<Image> _dots = new();
    
    public void CreateIndicators(int count)
    {
        foreach (var dot in _dots) Destroy(dot.gameObject);
        _dots.Clear();

        for (var i = 0; i < count; i++)
        {
            var dotObj = new GameObject($"Dot_{i}");
            var image = dotObj.AddComponent<Image>();
            image.sprite = _inactiveSprite;
            image.rectTransform.sizeDelta = _dotSize;
            dotObj.transform.SetParent(transform, false);
            _dots.Add(image);
        }
    }
    
    public void SetActiveIndex(int index)
    {
        for (var i = 0; i < _dots.Count; i++)
        {
            _dots[i].sprite = (i == index) ? _activeSprite : _inactiveSprite;
        }
    }
}
