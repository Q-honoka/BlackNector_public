using RaruLib;
using UnityEngine;

public class TitleInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Sound _sound => Sound.instance;
    void Start()
    {
        if (Sound.instance != null)
        {
            GameData.instance._saveSpot = SaveSpotKind.Stage1_1;
            TitleUiEvent.IsFirstLaunch = true;
            Sound.instance.ChangeVolume("BGM",0.6f);
            Sound.instance.ChangeVolume("SE", 0.8f);
            Sound.instance.Play("BGM", "Title_BGM");
            Sound.instance.Stop("BGM", "Game_BGM");
            Sound.instance.Stop("BGM", "Wind");
            Sound.instance.Stop("SE", "Badfeeling");
            Sound.instance.Stop("SE", "Walk_child");
            Sound.instance.Stop("SE", "Whitenoise");
            Sound.instance.Stop("SE", "Warning");
            Sound.instance.Stop("SE", "Marionette_caveat");
            Sound.instance.Stop("SE", "Rumble");
            Sound.instance.Stop("SE", "Enemy_Badfeeling");
        }
    }
}
