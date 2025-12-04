using UnityEngine;

public class TinDollController : MonoBehaviour, ICharcters
{

    public enum TinDollState
    {
        MOVE,   // 移動
        TURN,   // Uターン
        END,    // 終了
    }

    [SerializeField] EnemyData tinDoll;
    [SerializeField] TinDollState state;
    [SerializeField] Vector3[] patrolPos = new Vector3[2];      // 巡回ポイント
    [SerializeField] int direction = 1;

    private void Start()
    {
        tinDoll.myData.charctersInterface = this;
        transform.position = tinDoll.myData.pos;
        direction = direction < 0 ? -1 : 1;
    }

    void Update()
    {
        if (tinDoll == null) { return; }
        tinDoll.myData.charctersInterface.State();
    }

    void ICharcters.State()
    {
        switch(state)
        {
            case TinDollState.MOVE:
                tinDoll.myData.charctersInterface.Move();
                break;

            case TinDollState.TURN:
                Turn();
                break;

            case TinDollState.END:
                tinDoll.myData.charctersInterface.End();
                break;
        }
    }

    void ICharcters.Idle() { }

    // 前方に移動
    void ICharcters.Move()
    {
        Vector3 currentPos = this.gameObject.transform.position;
        Vector3 destinationPos;
        float destinationX = currentPos.x + tinDoll.myData.speed;

        
    }

    void ICharcters.End()
    {

    }

    // 状態遷移
    void ICharcters.SetMyState(int newState) { state = (TinDollState)newState; }

    // 移動方向を変える
    void Turn()
    {
        // 方向転換
        direction *= -1;

        tinDoll.myData.charctersInterface.SetMyState((int)TinDollState.MOVE);
    }
}
