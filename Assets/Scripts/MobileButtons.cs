using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    PlayerInputs playerInputs;
    [SerializeField] private Button myButton;
    [SerializeField] Vector2 v;

    void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
    }

    // Ação executada quando o botão é pressionado
    public void OnPointerDown(PointerEventData eventData)
    {
        playerInputs.SetPlayerInput(v);
    }

    // Ação executada quando o botão é solto
    public void OnPointerUp(PointerEventData eventData)
    {
        playerInputs.SetPlayerInput(Vector2.zero);
    }
}
