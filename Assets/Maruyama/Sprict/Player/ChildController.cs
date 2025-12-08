using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEngine.RuleTile.TilingRuleOutput;
namespace Maruyama
{
    public class ChildController : MonoBehaviour, ICharcters
{

    public enum CharctersState
    {
        IDLE,
        MOVE,
        END
    };

    public enum Layers
    {
        OBSTACLES = 3,
        ENEMY = 6,
    }



    const float MAX_SPEED = 2.5f;
    [SerializeField] PlayersData child;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] CharctersState state;
    [SerializeField]
    EnemyData[] enemies;
     GameObject player;

    float childToWallDistance;
    void Start()
    {
        child.myData.charctersInterface = this;
        transform.position = child.myData.pos;
        player = GameObject.FindWithTag("Player");
    }




    void Update()
    {
        if (child == null) { return; }

        child.myData.charctersInterface.State();

        foreach(var enemy in enemies)
        {
            if (enemy.enemyInterface.GetFoundPlayer()) { child.myData.charctersInterface.SetMyState((int)CharctersState.END); }
        }
    }



    void ICharcters.State()
    {
        switch (state)
        {
            case CharctersState.IDLE:
                child.myData.charctersInterface.Idle();
                break;

            case CharctersState.MOVE:
                child.myData.charctersInterface.Move();
                break;

            case CharctersState.END:
                child.myData.charctersInterface.End();
                break;
        }

    }




    void ICharcters.Idle()
    {
        if (0 < rigid.linearVelocityX) { rigid.linearVelocityX -= Time.deltaTime; }
    }



    void ICharcters.Move()
    {

        Collider2D col = GetComponent<Collider2D>();
       // Debug.Log(rigid.linearVelocityX);
        //インスタンスがからの場合return
        if (player == null) { return; }

        //前方に壁が存在している場合
        else if (CheckFront()) { child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE); return; }


        Debug.Log(rigid.linearVelocityX);
        if (CharctersCommonDetas.MAX_SPEED <= rigid.linearVelocityX) { rigid.linearVelocityX = MAX_SPEED; }
        rigid.AddForce(transform.right * child.myData.speed);
    }



    void ICharcters.End()
    {
        //アニメーションなど

        //次のシーンへ移動
        SceneManager.LoadScene("GameOver");
    }








    bool CheckFront()
    {
        //変数がvector2で宣言しているのは坂があった時に対応可能にするため

        //右を確認
        Ray2D ray = new Ray2D(new Vector2((transform.position.x + transform.localScale.x / 2) + 0.02f, transform.position.y), transform.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1.5f);
        Debug.DrawRay(ray.origin, ray.direction, new Color(1.0f, 0, 0));


        //障害物に当たったかを確認
        if (hit.collider == null || hit.collider.gameObject.layer != 3) { Debug.Log("当たらなかった"); return false; }

        Debug.Log("当たった");

        Vector2 hitPos = hit.point;

        Collider2D col = GetComponent<Collider2D>();

        //プレイヤーの当たり判定右部分を代入
        Vector2 myPos = col.bounds.max;


        //距離を算出
        childToWallDistance = transform.position.x + transform.localScale.x - hitPos.x;

        //障害物との距離が一定以下である場合「true」,出ない場合「false」
        if (MAX_SPEED < childToWallDistance) { return false; }

        return true;
    }




    /// <summary>
    /// 状態を遷移させる
    /// </summary>
    /// <param name="newState"></param>
    void ICharcters.SetMyState(int newState) { state = (CharctersState)newState; }




    public void SetCanMove() 
    {
        Debug.Log(state);
        if (state == CharctersState.MOVE) { child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE); return; }
        else if (state == CharctersState.IDLE) { child.myData.charctersInterface.SetMyState((int)CharctersState.MOVE); return; }
            
    }

    public void ResetPos()
    {
        child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE);
        transform.position = child.myData.pos;
        rigid.linearVelocityX = 0;
    }
}}