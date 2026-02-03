using UnityEngine;
using UnityEngine.UI;

public class PopUpPremium : MonoBehaviour
{
    [SerializeField] private Button _btnBack;

    private void Awake()
    {
        _btnBack.onClick.AddListener(() => Destroy(gameObject));
    }
}