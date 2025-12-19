using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEngine.RuleTile.TilingRuleOutput;   // なんこれ

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
    [SerializeField] Rigidbody rigid;
    [SerializeField] CharctersState state;
    [SerializeField] LayerMask Obstacle;        // 障害物のレイヤー
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
        if (0 < rigid.linearVelocity.x) { rigid.linearVelocity -= new Vector3(Time.deltaTime, 0, 0); }
    }



    void ICharcters.Move()
    {

        Collider col = GetComponent<Collider>();
        // Debug.Log(rigid.linearVelocityX);
        //インスタンスがからの場合return
        if (player == null) { return; }

        //前方に壁が存在している場合
        else if (CheckFront()) { child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE); return; }


        Debug.Log(rigid.linearVelocity);
        if (CharctersCommonDetas.MAX_SPEED <= rigid.linearVelocity.x) { rigid.linearVelocity = new Vector3(MAX_SPEED, 0, 0); }
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
        //変数がvector2で宣言しているのは坂があった時に対応可能にするため...っ！( > · <⸝⸝ᐢ

        //右を確認
        Vector3 childPos = transform.GetChild(0).gameObject.transform.position;    // キャラモデルの位置を取得
        Vector3 rayOrigin = new Vector3((childPos.x/* + transform.localScale.x / 2*/) + 0.02f, childPos.y, childPos.z);
        RaycastHit hit;
        float maxDistance = 1.5f;

        bool isHit = Physics.Raycast(rayOrigin, transform.right, out hit, maxDistance, Obstacle);

        Debug.DrawRay(rayOrigin, transform.right * maxDistance, Color.red);


        //障害物に当たったかを確認
        if (!isHit/* || hit.collider.gameObject.layer != 3*/) { Debug.Log("当たらなかった"); return false; }

        Debug.Log("当たった");

        Vector3 hitPos = hit.point;

        Collider col = GetComponent<Collider>();

        //プレイヤーの当たり判定右部分を代入
        Vector3 myPos = col.bounds.max;


        //距離を算出
        childToWallDistance = myPos.x - hitPos.x;

        //障害物との距離が一定以下である場合「true」,出ない場合「false」
        if (childToWallDistance > MAX_SPEED) { return false; }

        return true;
    }




    /// <summary>
    /// 状態を遷移させる
    /// </summary>
    /// <param name="newState"></param>
    void ICharcters.SetMyState(int newState) { state = (CharctersState)newState; }




    public void SetCanMove() 
    {
        Debug.Log($"あああ：{state}");
        if (state == CharctersState.MOVE) { child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE); return; }
        else if (state == CharctersState.IDLE) { child.myData.charctersInterface.SetMyState((int)CharctersState.MOVE); return; }
            
    }

    public void ResetPos()
    {
        child.myData.charctersInterface.SetMyState((int)CharctersState.IDLE);
        transform.position = child.myData.pos;

        Vector3 rigidVel = rigid.linearVelocity;
        rigidVel.x = 0;
        rigid.linearVelocity = rigidVel;
    }
}