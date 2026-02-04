using UnityEngine;
using UnityEngine.Playables;

public class Scarcrow : MonoBehaviour, IEnemy, ICharcters
{
    enum State
    {
        IDLE,
        FOUND,
        CHACHNECTOR,
        END
    }

    [SerializeField]
    State state;

    [SerializeField]
    EnemyData scacrow;

    [SerializeField]
    EnemyVisibility foundArea;
    
    [SerializeField]
    Animator anim;
    bool found = false;

    // 子どものアニメーター
    [SerializeField] Animator childAnim;
    // タイムライン
    [SerializeField] PlayableDirector FoundDirector;
    // 点滅UI
    [SerializeField] GameObject redFlashUI;

    private ICharcters myCharacter;
    private IEnemy myEnemy;
    void Start()
    {
        myCharacter = this;
        myEnemy = this;
        foundArea = this.transform.GetComponentInChildren<EnemyVisibility>();

        // Timelineにセットする
        foreach (var output in FoundDirector.playableAsset.outputs)
        {
            if (output.streamName == "ChildTrack")
            {
                FoundDirector.SetGenericBinding(output.sourceObject, childAnim);
            }
            else if (output.streamName == "RedFlash")
            {
                FoundDirector.SetGenericBinding(output.sourceObject, redFlashUI.GetComponent<Animator>());
            }
        }
    }

    void Update()  { myCharacter.State(); }


    void ICharcters.State()
    {
        switch (state)
        {
            case State.IDLE:
                myCharacter.Idle();
                break;

            case State.FOUND:
                myEnemy.FoundPlayer();
                break;
        }

    }

    void ICharcters.Idle()
    {
        if (foundArea.IsWithinChildInVisibility()) { myCharacter.SetMyState((int)State.FOUND); }
    }

    void ICharcters.Move() { }

    void ICharcters.End() { }
    void IEnemy.CatchByNector() { return; }

    void IEnemy.FoundPlayer() 
    {
        if(anim != null) anim.SetTrigger("found");
        found = true; 
        GameObject child = GameObject.FindGameObjectWithTag("Child");
        if (child != null) child.GetComponentInParent<ChildController>().SetIsFound();

        // タイムラインの再生
        FoundDirector.Play();
    }

    bool IEnemy.GetFoundPlayer() { return found; }

    public void SetMyState(int newState)
    {
        state = (State)newState;
    }
}
