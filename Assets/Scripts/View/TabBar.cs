using System;
using System.Collections.Generic;
using UnityEngine;

public class TabBar : MonoBehaviour
{
    [SerializeField] private List<Tab> _tabs;
    [SerializeField] private List<FilterType> _tabTypes;

    public event Action<FilterType> onTabSelected;

    private void Awake()
    {
        if (_tabs.Count == 0 || _tabTypes.Count == 0) return;
        
        SelectTab(0);
        
        for (var i = 0; i < _tabs.Count; i++)
        {
            var index = i;
            _tabs[i].onClick.AddListener(() => OnTabClicked(index));
        }
    }

    private void OnTabClicked(int index)
    {
        SelectTab(index);
        onTabSelected?.Invoke(_tabTypes[index]);
    }

    private void SelectTab(int selectedIndex)
    {
        for (var i = 0; i < _tabs.Count; i++) _tabs[i].IsSelected = i == selectedIndex;
    }
}