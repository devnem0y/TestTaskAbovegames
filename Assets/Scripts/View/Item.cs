using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _premiumBadge;
    
    //TODO: Так как в рамках ТЗ нет менеджера UI, то делаю максимально простое решение.
    [SerializeField] private GameObject _popUpView;
    [SerializeField] private GameObject _popUpPremium;
    
    private int _itemId;
    private bool _isPremium;
    private bool _hasBeenVisible;
    
    public event Action<int> onFirstVisible;
    
    public void Init(int itemId, Sprite sprite, bool isPremium)
    {
        _itemId = itemId;
        _isPremium = isPremium;

        if (_image != null && sprite != null) _image.sprite = sprite;
        if (_premiumBadge != null) _premiumBadge.SetActive(isPremium);
        
        CheckVisibility();
    }
    
    public void UpdateSprite(Sprite sprite)
    {
        if (_image != null && sprite != null) _image.sprite = sprite;
    }
    
    public void CheckVisibility()
    {
        if (_hasBeenVisible) return;

        if (!IsAnyCornerVisible()) return;
        
        _hasBeenVisible = true;
        onFirstVisible?.Invoke(_itemId);
    }
    
    private bool IsAnyCornerVisible()
    {
        var rt = (RectTransform)transform;
        var corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        var screenRect = new Rect(0, 0, Screen.width, Screen.height);

        return corners.Any(corner => screenRect.Contains(corner));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isPremium) _popUpPremium.SetActive(true);
        else _popUpView.SetActive(true);
    }
}
