using Cysharp.Threading.Tasks;
using RaruLib;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderEvent : MonoBehaviour
{
    public enum SLIDER_KIND
    {
        SoundBGM,
        SoundSE,
        Brightness,
        MAX
    }

    [SerializeField] SLIDER_KIND slider_kind = SLIDER_KIND.SoundBGM;
    
    private Sound _sound;
    [SerializeField] Material _material_brightness;

    private Slider _slider;

    private async UniTaskVoid Start()
    {
        _sound = (Sound.instance!=null)?Sound.instance : gameObject.AddComponent<Sound>();
        _slider = GetComponent<Slider>();

        // スライダーの種類ごとに別のトリガー待機
        switch (slider_kind)
        {
            case SLIDER_KIND.SoundBGM:
            case SLIDER_KIND.SoundSE:
                var token = this.GetCancellationTokenOnDestroy();
                await UniTask.WaitUntil(() => _sound.finishedInit, cancellationToken: token);
                break;
        }
        ChangeValue();
    }

    // 
    private void ChangeValue()
    {
        switch(slider_kind)
        {
            case SLIDER_KIND.SoundBGM:
                _slider.value = _sound.GetVolume("BGM");
                break;
            case SLIDER_KIND.SoundSE:
                _slider.value = _sound.GetVolume("SE");
                break;
        }
    }

    public void ChangeBGMVol(float value)
    {
        _sound.ChangeVolume("BGM", value);
    }

    public void ChangeSEVol(float value)
    {
        _sound.ChangeVolume("SE", value);
    }

    public void ChangeBrightness(float value)
    {
        _material_brightness.SetFloat("_Brightness", value);
    }
}
