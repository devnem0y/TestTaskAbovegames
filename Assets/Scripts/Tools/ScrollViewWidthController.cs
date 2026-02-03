using UnityEngine;
using UnityEngine.UI;

public class ScrollViewWidthController : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;

    public void Rebuild()
    {
        for (var i = 0; i < _scrollRect.content.childCount; i++)
        {
            var child = _scrollRect.content.GetChild(i);
            child.GetComponent<LayoutElement>().preferredWidth = _scrollRect.viewport.rect.width;
        }
    }
}
