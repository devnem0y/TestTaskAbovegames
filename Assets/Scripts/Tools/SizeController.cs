using UnityEngine;
using UnityEngine.UI;

public class SizeController : MonoBehaviour
{
    [SerializeField] private RectTransform _bannerPanel;
    [SerializeField] private RectTransform _tabBar;
    [SerializeField] private RectTransform _gallery;
    [SerializeField] private ScrollViewWidthController _scrollViewWidthController;

    public void InitLayout()
    {
        var rect = GetComponent<RectTransform>().rect;
        
        var deltaHeight = _bannerPanel.GetComponent<LayoutElement>().preferredHeight + 
                          _tabBar.GetComponent<LayoutElement>().preferredHeight ;
        var galleryLayoutElement = _gallery.GetComponent<LayoutElement>();
        galleryLayoutElement.preferredHeight = rect.height - deltaHeight;
        
        _bannerPanel.sizeDelta = new Vector2(rect.width, _bannerPanel.rect.height);
        _tabBar.sizeDelta = new Vector2(rect.width, _tabBar.rect.height);
        _gallery.sizeDelta = new Vector2(rect.width, galleryLayoutElement.preferredHeight);
        
        _scrollViewWidthController.Rebuild();
    }
}
