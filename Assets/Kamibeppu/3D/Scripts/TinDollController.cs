using RaruLib;
using UnityEngine;
using UnityEngine.Playables;

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
    [SerializeField, Header("巡回する座標")] GameObject[] patrolPos = new GameObject[2];
    // Uターンする障害物のレイヤー
    [SerializeField] LayerMask obstacleLayer;
    // 前判定に使う視野の長さ
    [SerializeField] float viewLength = 2f;
    // アニメーター
    [SerializeField] Animator anim;

    private bool foundChild = false;    // 子どもを見つけたかどうか
    private int patrolPosIndex = 0;     // 現在の巡回地点インデックス
    private float threshold = 0.1f;     // 巡回地点に到達と判定するしきい値

    private ICharcters myCharacter;
    private IEnemy myEnemy;
    private Sound _sound => Sound.instance;
    private void Start()
    {
        myCharacter = this;
        myEnemy = this;
    }

    void Update()
    {
        // キャラクターデータがあれば、状態ごとの処理をする
        if (tinDoll != null) { myCharacter.State(); }
    }

    /// <summary>
    /// 状態を管理
    /// </summary>
    void ICharcters.State()
    {
        if (patrolPos[0] == null) return;

        switch(state)
        {
            case State.IDLE:
                myCharacter.Idle();
                break;
            case State.FOUND:
                myEnemy.FoundPlayer();
                break;
            case State.MOVE:
                myCharacter.Move();
                break;
            case State.END:
                myCharacter.End();
                break;
        }
    }

    /*  以下、ICharcters の実装です。キャラクターの基本動作を実装しています。
        Idle()      停止状態の時の処理をする関数
        Move()      決められたルートを巡回する関数
        End()       終了処理をする関数
        SetMyState()    状態を変える関数
     */

    /// <summary>
    /// 停止状態
    /// </summary>
    void ICharcters.Idle() {  }
    /// <summary>
    /// 巡回中
    /// </summary>
    void ICharcters.Move()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        bool isInside =
            viewPos.z > 0 &&
            viewPos.x > 0 && viewPos.x < 1 &&
            viewPos.y > 0 && viewPos.y < 1;
        // もし、カメラ外なら、動かない。
        if (!isInside)
        {
            state = State.IDLE;
        }
            if (anim != null && anim.GetBool("Walking") != true) anim.SetBool("Walking", true);
        // 視界内に子どもがいたら FOUND 状態に遷移する
        if (foundArea.IsWithinChildInVisibility()) { myCharacter.SetMyState((int)State.FOUND); }
        // 前方に壁がある もしくは 巡回地点に到達したら Uターンする
        if(CheckWallForward() || CheckPatrolPos()) { Turn(); }
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
     */
    /// <summary>
    /// Playerを見つけた
    /// </summary>
    void IEnemy.FoundPlayer() 
    {
        if (anim != null)
        {
            anim.SetBool("Walking", false);
        }
        if (!foundChild)
        {
            _sound.Play("SE", "Marionette_caveat");
            _sound.Play("SE", "Warning");
            _sound.Play("SE", "Whitenoise");
        }
        foundChild = true;
        GameObject child = GameObject.FindGameObjectWithTag("Child");
        if (child != null) child.GetComponentInParent<ChildController>().SetIsFound();

        // タイムラインの再生
        GameObject.FindAnyObjectByType<GameOverManager>().PlayEnemyGameOver(2, anim);
    }
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
    /// 巡回地点に到達したか調べる
    /// </summary>
    /// <returns></returns>
    bool CheckPatrolPos()
    {
        // 巡回地点と自身の距離を求める
        float distance = Mathf.Abs(patrolPos[patrolPosIndex].transform.position.x - this.transform.position.x);
        // 距離がしきい値以下なら true を返す
        return distance <= threshold;
    }

    /// <summary>
    /// 前に壁があるか調べる
    /// </summary>
    /// <returns></returns>
    bool CheckWallForward()
    {
        Vector3 origin = this.gameObject.transform.position;
        Vector3 direction = this.gameObject.transform.right;
        RaycastHit hit;
        
        // 当たったオブジェクトが障害物なら true を返す
        if(Physics.Raycast(origin, direction, out hit, viewLength, obstacleLayer) == true)
        {
            if(hit.collider != null) { return true; }
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