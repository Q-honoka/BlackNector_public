using RaruLib;
using UnityEngine;

public class Command : MonoBehaviour
{
    private void Start()
    {
        if(Sound.instance!=null)
        {
            Sound.instance.Play("BGM", "BGM1");
        }
    }
    public void LogOutForGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void CallRetry()
    {
        if(Retry.instance==null)
        {
            Debug.Log("ƒŠƒgƒ‰ƒCŽ¸”s", gameObject);
            return;
        }
        Retry.instance.CallRetry();
    }
}
