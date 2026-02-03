using UnityEngine;
using UnityEngine.UI;

public class AdaptiveGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private int _phoneColumns;
    [SerializeField] private int _tabletColumns;
    [SerializeField] private float tabletMinDiagonal = 6.5f; // Минимальная диагональ для планшета в дюймах

    private bool _isTablet;

    private void Start()
    {
        CheckDeviceType();
        
        _grid.constraintCount = _isTablet ? _tabletColumns : _phoneColumns;
    }

    private void CheckDeviceType()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        var screenDPI = Screen.dpi;
        
        if (screenDPI <= 0) screenDPI = 160;
        
        var diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth / screenDPI, 2) 
                                        + Mathf.Pow(screenHeight / screenDPI, 2));

        _isTablet = diagonalInches >= tabletMinDiagonal;
    }
}
