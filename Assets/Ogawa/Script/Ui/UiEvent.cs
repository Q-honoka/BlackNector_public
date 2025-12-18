using RaruLib;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sound _sound => Sound.instance;

    private void Start()
    {
        //_sound = (Sound.instance != null) ? Sound.instance : gameObject.AddComponent<Sound>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sound.Play("SE", "SE1");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // マウスが離れた
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックされた
    }
}
