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
    const float STAY_FORCE = 7.5f;
    const float STAY_SPAN = 0.25f;
    [SerializeField] PlayersData player;                //playerの情報
    [SerializeField] Rigidbody2D rigid;                 //物理演算
    [SerializeField] CharctersState state;              //キャラクタの状態

    public bool peckInput = false;          //つつくをしているかどうか
    //インスタンス
    ChildController child;


    bool onGround = true;                      //接地フラグ
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
        child = GameObject.FindWithTag("Child").GetComponent<ChildController>();
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
            if (jumpLimit <= 0) { rigid.gravityScale = 0; jumpFlag = false; }   
            else
            {
                rigid.linearVelocityY += (0 < rigid.linearVelocityY) ? -Time.deltaTime : Time.deltaTime;
                jumpLimit -= rigid.linearVelocityY;
            }
        }
        else
        {
            rigid.linearVelocityY += (0 < rigid.linearVelocityY) ? -Time.deltaTime : Time.deltaTime;
        }

        rigid.linearVelocityX += (0 < rigid.linearVelocityX) ? -Time.deltaTime : Time.deltaTime;


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
        rigid.gravityScale = SATY_GRAVITY;

        //現在の位置が静止した場所よりも低い位置にあるなら
        if (transform.position.y < stayPosY)
        {
            //現在かかっている力を0にする
            rigid.linearVelocityY = 0;
            //再度上方向に力を加える
            rigid.AddForce(transform.up * STAY_FORCE);
        }
    }

    void ICharcters.Move()
    {
        staySpan = STAY_SPAN;

        //空中にいる場合関数を発動させる
        if (!onGround) { beginGround = CheckBeingUnderGround(); }
        if ((!onGround && !jumpFlag) && rigid.gravityScale != 0) { rigid.gravityScale = 0; }

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
                        rigid.gravityScale = GRAVITY_SPEED_Y;
                    }
                    else
                    {
                        if (jumpFlag) { break; }
                        //速度制御
                        if (MAX_SPEED_Y <= rigid.linearVelocityY)
                        {
                            rigid.linearVelocityY = MAX_SPEED_Y;
                        }
                        rigid.AddForce(transform.up * player.myData.speed);

                    }
                    break;
                }



            //左方向
            case (int)InputControl.PlayerActions.MOVE_LEFT:
                {
                    if (rigid.linearVelocityX <= -MAX_SPEED_X) { rigid.linearVelocityX = -MAX_SPEED_X; }
                    rigid.AddForce(transform.right * -player.myData.speed);
                    break;
                }



            //下方向
            case (int)InputControl.PlayerActions.MOVE_DOWN:
                {
                    //速度を制御
                    if (rigid.linearVelocityY <= -MAX_SPEED_Y) { rigid.linearVelocityY = -MAX_SPEED_Y; }

                    //下方向に移動
                    rigid.AddForce(transform.up * -player.myData.speed);

                    //自身の足元に地面がある場合、重力を戻す
                    if (beginGround) { rigid.gravityScale = 1; }
                    break;
                }



            //右方向
            case (int)InputControl.PlayerActions.MOVE_RIGHT:
                {
                    if (MAX_SPEED_X <= rigid.linearVelocityX) { rigid.linearVelocityX = MAX_SPEED_X; }
                    rigid.AddForce(transform.right * player.myData.speed);
                    break;
                }



            //つつく
            case (int)InputControl.PlayerActions.ACTION_PECK:
                {
                    if (0 < peckSpan) { return; }
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
        Ray2D ray = new Ray2D(new Vector2(transform.position.x, (transform.position.y - transform.localScale.y / 2) - 0.02f), -transform.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        Debug.DrawRay(ray.origin, ray.direction);

        //地面に当たったかを確認
        if (hit.collider == null || hit.collider.gameObject.tag != "Ground") { return false; }

        Vector2 hitPos = hit.point;

        Collider2D col = GetComponent<Collider2D>();

        //プレイヤーの当たり判定下部分を代入
        Vector2 myPos = col.bounds.min;


        //距離を算出
        groundToPlayerDistance = myPos.y - hitPos.y;

        //地面との距離が一定以下である場合「true」,出ない場合「false」
        if (0.5f < groundToPlayerDistance) { return false; }

        return true;
    }

    public Vector2 GetPlayerPosition() { return transform.position; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //onGround = true;
    }
}
