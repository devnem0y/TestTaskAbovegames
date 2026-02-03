using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private Color _activeColor;
    [SerializeField] private GameObject _line;

    public UnityEvent onClick;

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            _label.color = _isSelected ? _activeColor : _inactiveColor;
            _line.SetActive(_isSelected);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isSelected) onClick.Invoke();
    }
}
