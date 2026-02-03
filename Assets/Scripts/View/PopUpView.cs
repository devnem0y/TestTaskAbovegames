using UnityEngine;
using UnityEngine.UI;

public class PopUpView : MonoBehaviour
{
    [SerializeField] private Button _btnBack;
    [SerializeField] private Image _img;

    private void Awake()
    {
        _btnBack.onClick.AddListener(() => Destroy(gameObject));
    }

    public void SetImage(Sprite sprite)
    {
        _img.sprite = sprite;
    }
}