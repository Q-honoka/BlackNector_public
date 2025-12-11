using UnityEngine;

/*
 * このスクリプトは
 * 視界の中に「子どもがいるか いないか」
 * を判定するスクリプトです。
 * 
 * SphereColliderで簡易検知をしたあと
 * 半径 -> 角度 -> 障害物 の順で視界内か判定しています
 */

public class EnemyVisibility : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;   // 障害物のレイヤー
    [SerializeField] private float viewRadius = 5f;     // 視界の距離
    [SerializeField] private float viewAngle = 90f; // 視界の角度(左右の合計)

    private GameObject child = null;    // 子どもの情報
    private bool isWithinChild = false; // 視界内に子どもがいるか

    private void Update()
    {
        // 子どもを保持しているときは、より詳細に調べる
        if(child != null)
        {
            isWithinChild = IsWithinVisibility();
        }

        Debug.Log("こども: " + (isWithinChild ? "いる" : "いない"));
    }

    // 何かが視界に入ったとき
    private void OnTriggerStay(Collider other)
    {
        // 子どもだった場合は詳細な視界判定に入る
        if(other.CompareTag("Child"))
        {
            // 子どもを保持
            if (child == null) child = other.gameObject;
            isWithinChild = IsWithinVisibility();
        }
    }

    // 何かが視界から出たとき
    private void OnTriggerExit(Collider other)
    {
        // 子どもだった場合かつフラグがtrueのときは false にする
        if(other.CompareTag("Child"))
        {
            if (isWithinChild) isWithinChild = false;
            // 子どもを解放
            if (child != null) child = null;
        }
    }

    // 視界内にいるか調べる
    private bool IsWithinVisibility()
    {
        // 子どもが未取得なら処理しない
        if (child == null) return false;

        // 半径内にいなければ false を返す
        if (IsWithinRange() == false) return false;

        // 角度内にいなければ false を返す
        if (IsWithinAngle() == false) return false;

        // 障害物に当たったら false を返す
        if (IsBlocked()) return false;

        return true;
    }

    // 半径内にいるか調べる
    private bool IsWithinRange()
    {
        float distance = Vector3.Distance(child.transform.position, this.transform.position);

        // 距離が半径以下なら true を返す
        return distance <= viewRadius;
    }

    // 障害物があるか調べる
    private bool IsBlocked()
    {
        Vector3 direction = (child.transform.position - this.transform.position).normalized;
        // 障害物と衝突したら true を返す
        return Physics.Raycast(this.transform.position, direction, viewRadius, obstacleLayer);
    }

    // 角度内にいるか調べる
    private bool IsWithinAngle()
    {
        Vector2 toChild = new Vector2(
            child.transform.position.x - this.transform.position.x,
            child.transform.position.y - this.transform.position.y);

        Vector2 direction = new Vector2(transform.right.x, transform.right.y);

        float angle = Vector3.Angle(direction, toChild);
        Debug.Log(angle);

        // 角度が視野角以下なら true を返す
        return angle <= viewAngle * 0.5f;
    }

    /// <summary>
    /// 視界内に子どもがいるかを返す関数
    /// </summary>
    /// <returns></returns>
    public bool IsWithinChildInVisibility()
    {
        return isWithinChild;
    }
}
