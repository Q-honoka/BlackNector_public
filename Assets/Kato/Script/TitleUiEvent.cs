using RaruLib;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleUiEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sound _sound => Sound.instance;

    private void Start()
    {
        //_sound = (Sound.instance != null) ? Sound.instance : gameObject.AddComponent<Sound>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sound.Play("SE", "Cursor_touch");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // マウスが離れた
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックされた
        _sound.Play("SE", "Cursor_click");
        _sound.Stop("BGM","Title_BGM");
        _sound.Play("SE", "Wind");
        _sound.Play("BGM", "Game_BGM");
    }
}
