using UnityEngine;

public class TinDollController : MonoBehaviour, ICharcters, IEnemy
{
    /// <summary>
    /// 状態
    /// </summary>
    public enum State
    {
        // 停止
        IDLE,
        // 見つけた
        FOUND,
        // 巡回
        MOVE,
        // 終了
        END,
    }

    // キャラクターデータ
    [SerializeField] EnemyData tinDoll;
    // 視界
    [SerializeField] EnemyVisibility foundArea;
    // 現在の状態
    [SerializeField] State state = State.MOVE;
    // 巡回ポイント
    [SerializeField, Header("巡回する座標")] Vector3[] patrolPos = new Vector3[2];
    // Uターンする障害物のレイヤー
    [SerializeField] LayerMask obstacleLayer;
    // 前判定に使う視野の長さ
    [SerializeField] float viewLength = 2f;

    private bool foundChild = false;
    public int patrolPosIndex = 0;

    private void Start()
    {
        tinDoll.myData.charctersInterface = this;
        tinDoll.enemyInterface = this;
    }

    void Update()
    {
        // キャラクターデータがあれば、状態ごとの処理をする
        if (tinDoll != null) { tinDoll.myData.charctersInterface.State(); }
    }

    /// <summary>
    /// 状態を管理
    /// </summary>
    void ICharcters.State()
    {
        switch(state)
        {
            case State.IDLE:
                tinDoll.myData.charctersInterface.Idle();
                break;
            case State.FOUND:
                tinDoll.enemyInterface.FoundPlayer();
                break;
            case State.MOVE:
                tinDoll.myData.charctersInterface.Move();
                break;
            case State.END:
                tinDoll.myData.charctersInterface.End();
                break;
        }
    }

    /*  以下、ICharcters の実装です。キャラクターの基本動作を実装しています。
        Idle()      停止状態の時の処理をする関数   （未実装）
        Move()      決められたルートを巡回する関数  （未実装）
        End()       終了処理をする関数            （未実装）
        SetMyState()    状態を変える関数
     */

    /// <summary>
    /// 停止状態
    /// </summary>
    void ICharcters.Idle() { }
    /// <summary>
    /// 巡回中
    /// </summary>
    void ICharcters.Move()
    {
        // 視界内に子どもがいたら FOUND 状態に遷移する
        if (foundArea.IsWithinChildInVisibility()) { tinDoll.myData.charctersInterface.SetMyState((int)State.FOUND); }
        // 前方に壁があったらUターンする
        if(CheckWallForward() == true) { Turn(); }
        // 前方に移動
        transform.Translate(transform.right * tinDoll.myData.speed * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// 終了状態
    /// </summary>
    void ICharcters.End()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="newState"></param>
    void ICharcters.SetMyState(int newState) { state = (State)newState; }

    /*  以下、IEnemy の実装です。敵の基本動作を実装しています。
        FoundPlayer()       子どもを見つけたかどうかを true / false で設定する関数
        GetFoundPlayer()    子どもを見つけたかどうかを true / false で返す関数
        CatchByNector()     暗闇でネスターに見つかった時の処理をする関数（未実装）
     */
    /// <summary>
    /// Playerを見つけた
    /// </summary>
    void IEnemy.FoundPlayer() { foundChild = true; }
    /// <summary>
    /// playerの捜索状態を共有
    /// </summary>
    /// <returns></returns>
    bool IEnemy.GetFoundPlayer() { return foundChild; }
    /// <summary>
    /// ネスターに捕まった
    /// </summary>
    void IEnemy.CatchByNector() { }

    /// <summary>
    /// 前に壁があるか調べる
    /// </summary>
    /// <returns></returns>
    bool CheckWallForward()
    {
        Vector3 origin = this.gameObject.transform.position;
        Vector3 direction = this.gameObject.transform.right;
        RaycastHit hit;
        
        // 当たったオブジェクトが障害物なら進行方向を反転する
        Debug.DrawLine(origin, origin + direction * viewLength);
        if(Physics.Raycast(origin, direction, out hit, viewLength) == true)
        {
            if(hit.collider != null && (1 << hit.collider.gameObject.layer) == obstacleLayer.value)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Uターン処理
    /// </summary>
    void Turn()
    {
        // ターゲット地点を移動
        patrolPosIndex = (patrolPosIndex + 1) % patrolPos.Length;
        
        // 角度を反転させる(後々コルーチン使ってなめらかにしたい)
        Vector3 currentAngle = this.gameObject.transform.rotation.eulerAngles;
        currentAngle.y = (currentAngle.y + 180f) % 360f;
        transform.eulerAngles = currentAngle;
    }
}