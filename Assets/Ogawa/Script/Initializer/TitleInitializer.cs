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
            Sound.instance.Play("BGM", "Title_BGM");
            Sound.instance.Stop("BGM","Game_BGM");
            Sound.instance.Stop("SE","Wind");
        }
    }
}
