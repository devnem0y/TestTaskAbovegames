using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemFactory
{
    private readonly GameObject _prefab;
    private readonly RectTransform _parent;
    private readonly Dictionary<int, Item> _items;


    public ItemFactory(GameObject prefab, RectTransform parent)
    {
        _prefab = prefab;
        _parent = parent;
        
        _items = new Dictionary<int, Item>();
    }

    public Dictionary<int, Item> CreateItems(List<int> itemIds, Action<int> action)
    {
        foreach (var id in itemIds) CreateItem(id, action);
        return _items;
    }

    public Item GetItem(int itemId) => _items.TryGetValue(itemId, out var item) ? item : null;

    public void DestroyAllItems()
    {
        foreach (var item in _items.Values) Object.Destroy(item.gameObject);
        _items.Clear();
    }

    private void CreateItem(int itemId, Action<int> action)
    {
        var obj = Object.Instantiate(_prefab, _parent);
        var controller = obj.GetComponent<Item>();
        var isPremium = itemId % 4 == 0;
        controller.Init(itemId, null, isPremium);
        controller.onFirstVisible += action;
        _items[itemId] = controller;
    }
}