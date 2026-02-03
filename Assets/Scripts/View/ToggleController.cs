using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _toggleImage;
    [SerializeField] private Image _checkImage;
    [SerializeField] private TMP_Text _lblPrice;
    [SerializeField] private TMP_Text _lblPer;
    [SerializeField] private List<Color> _colors;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(SetColor);
        SetColor(_toggle.isOn);
    }

    private void SetColor(bool isOn)
    {
        if (isOn)
        {
            _checkImage.color = _colors[0];
            _toggleImage.color = _colors[0];
            _lblPrice.color = _colors[0];
            _lblPer.color = _colors[1];
        }
        else
        {
            _checkImage.color = _colors[2];
            _toggleImage.color = _colors[2];
            _lblPrice.color = _colors[2];
            _lblPer.color = _colors[3];
        }
    }
}