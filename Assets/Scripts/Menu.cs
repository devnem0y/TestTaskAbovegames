using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private SizeController _sizeController;
    [SerializeField] private BannerManager _bannerManager;
    [SerializeField] private Gallery _gallery;

    private void Start()
    {
        _sizeController.InitLayout();
        _bannerManager.Init();
        _gallery.Init();
    }
}