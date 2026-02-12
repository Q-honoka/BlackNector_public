using System;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

public class Marionette : MonoBehaviour, ICharcters, IEnemy
{
    /// <summary>
    /// 状態
    /// </summary>
    enum State
    {
        //待機・停止
        IDLE,
        //見つけた
        FOUND,
        //落ちる
        FALL,
        //壊れた
        BROKEN,
        //ネスターに捕まった
        CATCHBYNECTOR,
    }

    //物理挙動
    [SerializeField] Rigidbody rigid;
    //当たり判定
    [SerializeField] new Collider collider;
    //キャラクターデータ
    [SerializeField] EnemyData marionette;
    //視界
    [SerializeField] EnemyVisibility foundArea;
    //現在の状態
    [SerializeField] State state;
    // アニメーター
    [SerializeField] Animator anim;

    private ICharcters myCharacter;
    private IEnemy myEnemy;

    // 落下するイベント
    private Subject<Unit> fallSubject = new Subject<Unit>();
    public IObservable<Unit> OnFallSubject
    {
        get
        {
            return fallSubject;
        }
    }

    //playerを見つけたか否か
    bool foundChild = false;
    //糸のインスタンス
    GimmickThread thread;
    void Start()
    {
        //
        myCharacter = this;
        myEnemy = this;
        thread = gameObject.transform.GetComponentInChildren<GimmickThread>();    // 子オブジェクトのコライダーを使用する
    }

    void Update()
    {
        myCharacter.State();
    }

    /// <summary>
    /// 状態を管理
    /// </summary>
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

            case State.FALL:
                Debug.Log(state);
                Fall();
                break;

            case State.BROKEN:
                //アニメーション
                Broken();
                break;

            case State.CATCHBYNECTOR:
                //共通関数
                break;

        }

    }
    /// <summary>
    /// 待機・停止状態
    /// </summary>
    void ICharcters.Idle()
    {
        if (foundArea.IsWithinChildInVisibility()) { myCharacter.SetMyState((int)State.FOUND); }
        if (!thread.GetConnctMarionnet()) { myCharacter.SetMyState((int)State.FALL); }
    }
    /// <summary>
    /// 動き(動かないので無記入)
    /// </summary>
    void ICharcters.Move() { }
    /// <summary>
    /// 終了処理
    /// </summary>
    void ICharcters.End() { Destroy(gameObject); }
    /// <summary>
    /// 遷移先の状態を指定
    /// </summary>
    /// <param name="newState"></param>
    void ICharcters.SetMyState(int newState) { state = (State)newState; }

    /// <summary>
    /// Playerを見つけた
    /// </summary>
    void IEnemy.FoundPlayer() 
    {
        if (anim != null) anim.SetTrigger("found");
        foundChild = true;
        GameObject child = GameObject.FindGameObjectWithTag("Child");
        if (child != null) child.GetComponentInParent<ChildController>().SetIsFound();

        // タイムラインの再生
        GameObject.FindAnyObjectByType<GameOverManager>().PlayEnemyGameOver(1, anim);
    }
    /// <summary>
    /// ネスターに捕まった
    /// </summary>
    void IEnemy.CatchByNector() { }
    /// <summary>
    /// playerの捜索状態を共有
    /// </summary>
    /// <returns></returns>
    bool IEnemy.GetFoundPlayer() { return foundChild; }

    /// <summary>    
    /// /// 落ちている
    /// </summary>
    void Fall() 
    {
        if (anim != null) anim.SetTrigger("StartFalling");
        rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints.FreezePositionX;       // 真下に落ちるようにする
        rigid.useGravity = true;
    }
    /// <summary>
    /// 壊れた
    /// </summary>
    void Broken() 
    {
        rigid.isKinematic = true;
        collider.isTrigger = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        transform.rotation = Quaternion.identity;
        foundArea.gameObject.SetActive(false);
        if (anim != null) anim.SetTrigger("EndFalling");

        gameObject.layer = 5;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground") { return; }
        myCharacter.SetMyState((int)State.BROKEN);
    }
}
