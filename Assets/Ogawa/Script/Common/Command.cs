using Cysharp.Threading.Tasks;
using RaruLib;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
            Debug.Log("ƒŠƒgƒ‰ƒC¸”s", gameObject);
            return;
        }
        Retry.instance.CallRetry();
    }
    public void CallSceneChange(int value)
    {
        if (SceneController.instance == null)
        {
            Debug.Log("ƒV[ƒ“‘JˆÚ¸”s", gameObject);
            return;
        }
        SceneController.instance.SceneChange(value);
    }
    public void CallPanelChange_SendMain()
    {
        if (ScenePanel.instance == null)
        {
            Debug.Log("ƒpƒlƒ‹‘JˆÚ¸”s", gameObject);
            return;
        }
        ScenePanel.instance.DataCallOpenPanel(PanelKind.Main);
    }
    public void CallPanelChange_SendMenu()
    {
        if (ScenePanel.instance == null)
        {
            Debug.Log("ƒpƒlƒ‹‘JˆÚ¸”s", gameObject);
            return;
        }
        ScenePanel.instance.DataCallOpenPanel(PanelKind.Menu);
    }
    public void CallNovelMessagePlay()
    {
        if (NovelSubject.instance == null)
        {
            Debug.Log("ƒmƒxƒ‹Ä¶¸”s", gameObject);
            return;
        }
        NovelSubject.instance.Play(NOVEL_KIND.Event_1_1).Forget();
    }
    public void CallNovelMessageNext()
    {
        if (NovelSubject.instance == null)
        {
            Debug.Log("ƒmƒxƒ‹is¸”s", gameObject);
            return;
        }
        NovelSubject.instance.Next();
    }
}
