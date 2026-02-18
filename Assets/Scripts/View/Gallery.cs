using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    [SerializeField] private string _url;
    [SerializeField] private int _imageMaxCount;
    [SerializeField] private TabBar _tabBar;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private ScrollRect _scroll;

    private ImageLoader _imageLoader;
    private ItemFactory _itemFactory;
    private List<int> _visibleIds;
    private Dictionary<int, Item> _items;
    
    private void Awake()
    {
#if UNITY_EDITOR
        _imageMaxCount = 15;
#endif
        
        _tabBar.onTabSelected += SetFilter;
        _scroll.onValueChanged.AddListener(_ => CheckVisibilityItems());
        
        _imageLoader = new ImageLoader(_url);
        _itemFactory = new ItemFactory(_itemPrefab, _content);
    }

    private void OnDestroy()
    {
        _tabBar.onTabSelected -= SetFilter;
        _scroll.onValueChanged.RemoveAllListeners();
    }
    
    public void Init()
    {
        _visibleIds = new List<int>();
        _items = new Dictionary<int, Item>();
        
        SetFilter(FilterType.ALL);
    }

    private void SetFilter(FilterType filter)
    {
        ClearAll();

        for (var i = 1; i <= _imageMaxCount; i++)
        {
            if (IsMatchFilter(i, filter)) _visibleIds.Add(i);
        }

        RebuildUI();
    }

    private static bool IsMatchFilter(int id, FilterType filter) =>
        filter == FilterType.ALL ||
        (filter == FilterType.ODD && id % 2 == 1) ||
        (filter == FilterType.EVEN && id % 2 == 0);

    private void ClearAll()
    {
        _itemFactory.DestroyAllItems();
        _items.Clear();
        _visibleIds.Clear();
    }

    private void RebuildUI()
    {
        _items = _itemFactory.CreateItems(_visibleIds, OnFirstVisible);
        Invoke(nameof(CheckVisibilityItems), 0.1f);
    }

    private void OnSpriteLoaded(int itemId, Sprite sprite)
    {
        var item = _itemFactory.GetItem(itemId);
        if (item != null) item.UpdateSprite(sprite);
    }

    private void OnFirstVisible(int itemId)
    {
        _imageLoader.LoadSprite(itemId, OnSpriteLoaded);
    }

    private void CheckVisibilityItems()
    {
        foreach (var item in _items.Values) item.CheckVisibility();
    }
}