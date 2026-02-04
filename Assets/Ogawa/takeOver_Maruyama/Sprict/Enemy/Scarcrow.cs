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

    private ICharcters myCharacter;
    private IEnemy myEnemy;
    void Start()
    {
        myCharacter = this;
        myEnemy = this;
        foundArea = this.transform.GetComponentInChildren<EnemyVisibility>();
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

        // É^ÉCÉÄÉâÉCÉìÇÃçƒê∂
        GameObject.FindAnyObjectByType<GameOverManager>().PlayEnemyGameOver(0, anim);
    }

    bool IEnemy.GetFoundPlayer() { return found; }

    public void SetMyState(int newState)
    {
        state = (State)newState;
    }
}
