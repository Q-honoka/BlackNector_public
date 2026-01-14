using UnityEngine;

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
    bool found = false;
    void Start()
    {
        scacrow.myData.charctersInterface = this;
        scacrow.enemyInterface = this;
        foundArea = this.transform.GetComponentInChildren<EnemyVisibility>();
        Debug.Log($"{gameObject.name}‚ÌŽ‹ŠE: {foundArea.name}");
    }

    void Update()  { scacrow.myData.charctersInterface.State(); }


    void ICharcters.State()
    {
        switch (state)
        {
            case State.IDLE:
                scacrow.myData.charctersInterface.Idle();
                break;

            case State.FOUND:
                scacrow.enemyInterface.FoundPlayer();
                break;
        }

    }

    void ICharcters.Idle()
    {
        if (foundArea.IsWithinChildInVisibility()) { scacrow.myData.charctersInterface.SetMyState((int)State.FOUND); }
    }

    void ICharcters.Move() { }

    void ICharcters.End() { }
    void IEnemy.CatchByNector() { return; }

    void IEnemy.FoundPlayer() { found = true; }

    bool IEnemy.GetFoundPlayer() { return found; }

    public void SetMyState(int newState)
    {
        state = (State)newState;
    }
}
