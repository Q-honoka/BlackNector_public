using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, ICharcters
{
    //キャラクターのすべての状態
    public enum CharctersState
    {
        IDLE,   //待機
        MOVE,   //移動
        END     //終了
    };

    const float KEY_INPUT_SPAN = 1f;      //次にキー操作を行う際のスパン
    const float MAX_SPEED_X = 2.5f;
    const float MAX_SPEED_Y = MAX_SPEED_X / 2;
    const float GRAVITY_SPEED_Y = -5;
    const float SATY_GRAVITY = 1;
    const bool  SATY_GRAVITY_BOOL = true;
    const float STAY_FORCE = 10f;
    const float STAY_SPAN = 0.25f;
    [SerializeField] PlayersData player;                //playerの情報
    [SerializeField] Rigidbody rigid;                 //物理演算
    [SerializeField] CharctersState state;              //キャラクタの状態

    public bool peckInput = false;          //つつくをしているかどうか
    //インスタンス
    ChildController child;


    bool onGround = false;                      //接地フラグ
    bool beginGround = false;                  //地面が存在しているか
    bool jumpFlag = false;                     //ジャンプしているか
    float groundToPlayerDistance = 0;          //地面とplayerとの距離
    float callChildSpan = KEY_INPUT_SPAN;      //次にキー操作を行う際のスパン
    float peckSpan = KEY_INPUT_SPAN;           //次にキー操作を行う際のスパン
    float staySpan = STAY_SPAN;
    float jumpLimit = 0;
    float stayPosY = 0;
    void Start()
    {
        player.myData.charctersInterface = this;
        child = GameObject.FindAnyObjectByType<ChildController>();
        transform.position = player.myData.pos;
        jumpLimit = transform.localScale.y;
    }
    void Update()
    {
        //player処理開始
        player.myData.charctersInterface.State();
    }


    void ICharcters.State()
    {
        ////*キー入力のスパンの減少処理*////
        if (0 < callChildSpan) { callChildSpan -= Time.deltaTime; }
        if (0 < peckSpan) { peckSpan -= Time.deltaTime; }

        //ジャンプをしたなら
        if (jumpFlag)
        {
            //jump制限が 0 以下なら
            if (jumpLimit <= 0) { rigid.useGravity = false; jumpFlag = false; }   
            else
            {
                rigid.linearVelocity += new Vector3(0, (0 < rigid.linearVelocity.y) ? -Time.deltaTime : Time.deltaTime, 0);
                jumpLimit -= rigid.linearVelocity.y;
            }
        }
        else
        {
            rigid.linearVelocity += new Vector3(0, (0 < rigid.linearVelocity.y) ? -Time.deltaTime : Time.deltaTime, 0);
        }

        rigid.linearVelocity += new Vector3((0 < rigid.linearVelocity.y) ? -Time.deltaTime : Time.deltaTime, 0, 0);


        //Debug.Log(rigid.linearVelocityY);

        //状態に応じて関数を呼ぶ
        switch (state)
        {
            //待機
            case CharctersState.IDLE:
                player.myData.charctersInterface.Idle();
                break;

            //動く・動いている
            case CharctersState.MOVE:
                player.myData.charctersInterface.Move();
                break;

            //終了
            case CharctersState.END:
                player.myData.charctersInterface.End();
                break;  
        }

    }

    void ICharcters.Idle()
    {

        //いずれかのキーを操作したとき、状態遷移させる
        if (Input.anyKey) { stayPosY = 0; player.myData.charctersInterface.SetMyState((int)CharctersState.MOVE); return; }

        staySpan -= Time.deltaTime;
        if (0 < staySpan) { return; }
        //静止した場所を取得
        if (stayPosY == 0) { stayPosY = transform.position.y; }

        if (onGround || jumpFlag) { return; }
        //重力を止まっている間のみ適応
        rigid.useGravity = false;

        //現在の位置が静止した場所よりも低い位置にあるなら
        if (transform.position.y < stayPosY)
        {
            //現在かかっている力を0にする
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
            //再度上方向に力を加える
            //rigid.AddForce(transform.up * STAY_FORCE);
        }
    }

    void ICharcters.Move()
    {
        staySpan = STAY_SPAN;

        //空中にいる場合関数を発動させる
        if (!onGround) { beginGround = CheckBeingUnderGround(); }
        if ((!onGround && !jumpFlag) && rigid.useGravity != false) { rigid.useGravity = false; }

        switch (InputControl.Instance.CheckPlayerMoveKey())
        {


            //上方向
            case (int)InputControl.PlayerActions.MOVE_UP:
                {
                    if (onGround)
                    {
                        //接地フラグをfalse
                        onGround = false;

                        //ジャンプフラグをtrue
                        jumpFlag = true;

                        jumpLimit = transform.localScale.y;

                        //初速として重力を反転
                        rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, GRAVITY_SPEED_Y, rigid.linearVelocity.z);
                    }
                    else
                    {
                        if (jumpFlag) { break; }
                        //速度制御
                        if (MAX_SPEED_Y <= rigid.linearVelocity.y)
                        {
                            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, MAX_SPEED_Y, rigid.linearVelocity.z);
                        }
                        rigid.AddForce(transform.up * player.myData.speed);

                    }
                    break;
                }



            //左方向
            case (int)InputControl.PlayerActions.MOVE_LEFT:
                {
                    if (rigid.linearVelocity.x <= -MAX_SPEED_X) { rigid.linearVelocity = new Vector3(-MAX_SPEED_X, 0, 0); }

                    rigid.AddForce(transform.right * -player.myData.speed);
                    break;
                }



            //下方向
            case (int)InputControl.PlayerActions.MOVE_DOWN:
                {
                    //速度を制御
                    if (rigid.linearVelocity.y <= -MAX_SPEED_Y) { rigid.linearVelocity = new Vector3(0, -MAX_SPEED_Y, 0); }

                    //下方向に移動
                    rigid.AddForce(transform.up * -player.myData.speed);

                    //自身の足元に地面がある場合、重力を戻す
                    if (beginGround) { rigid.useGravity = false; }
                    break;
                }



            //右方向
            case (int)InputControl.PlayerActions.MOVE_RIGHT:
                {
                    if (MAX_SPEED_X <= rigid.linearVelocity.x) { rigid.linearVelocity = new Vector3(MAX_SPEED_X, 0, 0); }
                    rigid.AddForce(transform.right * player.myData.speed);
                    break;
                }



            //つつく
            case (int)InputControl.PlayerActions.ACTION_PECK:
                {
                    if (0 < peckSpan) { peckInput = false; return; }
                    peckSpan = KEY_INPUT_SPAN;
                    peckInput = true;
                    break;
                }


            //呼ぶ
            case (int)InputControl.PlayerActions.ACTION_CALL:
                {
                    if (0 < callChildSpan) { return; }
                    callChildSpan = KEY_INPUT_SPAN;
                    //子供を呼ぶ
                    child.SetCanMove();
                    break;
                }



            default:
                {
                    //何も押されていない場合Idle状態にする
                    player.myData.charctersInterface.SetMyState((int)CharctersState.IDLE);
                    break;
                }
        }

    }
    void ICharcters.End() { }
    /// <summary>
    /// 状態を切り替える
    /// </summary>
    /// <param name="newState"></param>
    void ICharcters.SetMyState(int newState) { state = (CharctersState)newState; }

    /// <summary>
    /// 自身の真下に地面が存在しているかを確認
    /// </summary>
    /// <returns></returns>
    bool CheckBeingUnderGround()
    {

        //変数がvector2で宣言しているのは坂があった時に対応可能にするため

        //真下を確認
        float maxDistance = Mathf.Infinity;
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, (transform.position.y - transform.localScale.y / 2) - 0.02f, transform.position.z);
        Vector3 rayDirection = -transform.up;
        bool isHit = Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance);
        Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.white);

        //地面に当たったかを確認
        if (hit.collider == null || hit.collider.gameObject.tag != "Ground") { return false; }

        Vector3 hitPos = hit.point;

        Collider col = GetComponent<Collider>();

        //プレイヤーの当たり判定下部分を代入
        Vector3 myPos = col.bounds.min;


        //距離を算出
        groundToPlayerDistance = myPos.y - hitPos.y;

        //地面との距離が一定以下である場合「true」,出ない場合「false」
        if (0.5f < groundToPlayerDistance) { return false; }

        return true;
    }

    public Vector3 GetPlayerPosition() { return transform.position; }

    private void OnCollisionEnter(Collision collision)
    {
        //onGround = true;
    }
}
