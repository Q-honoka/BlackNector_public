using Cysharp.Threading.Tasks;
using RaruLib;
using System;
using UnityEngine;

public class Command : MonoBehaviour
{
    protected void Start()
    {
        if(Sound.instance!=null)
        {
            Sound.instance.Play("BGM", "BGM1");
        }
    }
    public virtual void LogOutForGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public virtual void CallRetry()
    {
        if(Retry.instance==null)
        {
            Debug.Log("ƒŠƒgƒ‰ƒC¸”s", gameObject);
            return;
        }
        Retry.instance.CallRetry();
    }
    public virtual void CallSceneChange(int value)
    {
        if (SceneController.instance == null)
        {
            Debug.Log("ƒV[ƒ“‘JˆÚ¸”s", gameObject);
            return;
        }
        SceneController.instance.SceneChange(value);
    }
    public virtual void CallPanelChange_SendMain()
    {
        if (ScenePanel.instance == null)
        {
            Debug.Log("ƒpƒlƒ‹‘JˆÚ¸”s", gameObject);
            return;
        }
        ScenePanel.instance.DataCallOpenPanel(PanelKind.Main);
    }
    public virtual void CallPanelChange_SendMenu()
    {
        if (ScenePanel.instance == null)
        {
            Debug.Log("ƒpƒlƒ‹‘JˆÚ¸”s", gameObject);
            return;
        }
        ScenePanel.instance.DataCallOpenPanel(PanelKind.Menu);
    }
    public virtual void CallNovelMessagePlay(NOVEL_KIND novelKind)
    {
        if (NovelSubject.instance == null)
        {
            Debug.Log("ƒmƒxƒ‹Ä¶¸”s", gameObject);
            return;
        }
        NovelSubject.instance.Play(novelKind).Forget();
    }
    public virtual void CallNovelMessagePlay(int novelKind)
    {
        if (NovelSubject.instance == null)
        {
            Debug.Log("ƒmƒxƒ‹Ä¶¸”s", gameObject);
            return;
        }
        if (!Enum.IsDefined(typeof(NOVEL_KIND), novelKind))
        {
            Debug.Log("•s³‚Èenum‚Å¸”s", gameObject);
            return;
        }
        NOVEL_KIND kind = (NOVEL_KIND)novelKind;
        NovelSubject.instance.Play(kind).Forget();
    }
    public virtual void CallNovelMessageNext()
    {
        if (NovelSubject.instance == null)
        {
            Debug.Log("ƒmƒxƒ‹is¸”s", gameObject);
            return;
        }
        NovelSubject.instance.Next();
    }
}
