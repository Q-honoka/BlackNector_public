using UnityEngine;
using UnityEngine.Events;

public class CommandTrigger : Command
{
    [SerializeField] private UnityEvent m_event;

    [SerializeField] private bool isOnce = false;
    private bool _OnOnced = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if(isOnce && _OnOnced)
        {
            return;
        }
        m_event?.Invoke();
        _OnOnced = true;
    }
    public override void LogOutForGame()
        { base.LogOutForGame(); }
    public override void CallRetry()
        {base.CallRetry();}
    public override void CallSceneChange(int value)
        {base.CallSceneChange(value);}
    public override void CallPanelChange_SendMain()
        {base.CallPanelChange_SendMain();}
    public override void CallPanelChange_SendMenu()
        {base.CallPanelChange_SendMenu();}
    public override void CallNovelMessagePlay(NOVEL_KIND novelKind)
        {base.CallNovelMessagePlay(novelKind);}
    public override void CallNovelMessageNext()
    { base.CallNovelMessageNext();}

}