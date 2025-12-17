using RaruLib;
using UnityEngine;

public class SoundSpawn : MonoBehaviour
{
    [SerializeField,Header("サウンドコントローラー")] GameObject gameobj;

    private void Start()
    {
        if(Sound.instance == null)
        {
            Instantiate(gameobj);
        }
    }
}
