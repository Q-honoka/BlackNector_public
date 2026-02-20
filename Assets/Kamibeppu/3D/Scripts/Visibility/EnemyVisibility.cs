using UnityEngine;

/*
 * このスクリプトは
 * 視界の中に「子どもがいるか いないか」
 * を判定するスクリプトです。
 * 
 * SphereColliderで簡易検知をしたあと
 * 半径 -> 角度 -> 障害物 の順で視界内か判定しています
 * 
 * IsWithinChildInVisibility()
 * を呼ぶと子どもがいるなら true
 * いないなら false を返します。
 * 
 * 視界の描画に MeshRenderer と MeshFilter が必要です。
 */

public class EnemyVisibility : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;   // 障害物のレイヤー
    [SerializeField] private float viewRadius = 5f;     // 視界の距離
    [SerializeField] private float viewAngle = 90f;     // 視界の角度(左右の合計)
    [SerializeField, Range(4, 128)] private int segmentCount = 48; // 分割数
    [SerializeField] private LayerMask excludeMask;     // 当たり判定を行わないレイヤー

    private GameObject child = null;    // 子どもの情報
    private bool isWithinChild = false; // 視界内に子どもがいるか
    private Mesh mesh = null;       // 扇形にするメッシュ
    private MeshFilter filter;      // メッシュを適用するMeshFilter

    private void Start()
    {
        filter = this.GetComponent<MeshFilter>();
        if (mesh == null) mesh = new Mesh();
        if (filter != null) filter.mesh = mesh;
    }

    private void Update()
    {
        // 視界の描画
        RenderVisibility();

        // 子どもを保持しているときは、より詳細に調べる
        if (child != null)
        {
            isWithinChild = IsWithinVisibility();
        }

    }

    // 視界の描画
    private void RenderVisibility()
    {
        // メッシュがない場合は処理しない
        if (mesh == null) return;
        mesh.Clear();

        float half = viewAngle * 0.5f;
        float start = -half;
        float step = viewAngle / segmentCount;

        Vector3[] vertices = new Vector3[2 + segmentCount];     // 頂点数(中心+扇の端+分割数)

        // 中心点を設定
        vertices[0] = Vector3.zero;

        // 各頂点の設定
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = (start + step * i) * Mathf.Deg2Rad;   // ラジアン
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            Vector3 origin = this.transform.position;
            Quaternion rotation = this.transform.rotation;

            Vector3 localDir = new Vector3(x, y, 0f);
            Vector3 dir = rotation * localDir;
            RaycastHit hit;
            Vector3 targetPoint;
            
            // 障害物にぶつかったらぶつかった場所を保存
            if (Physics.Raycast(origin, dir, out hit, viewRadius, excludeMask,QueryTriggerInteraction.Ignore))
            {
                targetPoint = hit.point;
            }
            // ぶつからなければ最大距離を保存
            else
            {
                targetPoint = origin + dir * viewRadius;
            }

            Vector3 localVer = this.transform.InverseTransformPoint(targetPoint);
            vertices[i + 1] = localVer;     // 頂点をセット
        }

        // 三角形の設定
        int[] triangles = new int[segmentCount * 3];
        for (int i = 0; i < segmentCount; i++)
        {
            triangles[i * 3] = 0;           // 中心
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // 頂点と三角形の情報をメッシュに格納する
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // 何かが視界に入ったとき
    private void OnTriggerStay(Collider other)
    {
        // 子どもだった場合は詳細な視界判定に入る
        if (other.CompareTag("Child"))
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
        if (other.CompareTag("Child"))
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
        bool hitObject = false;
        hitObject = Physics.Raycast(this.transform.position, direction, viewRadius, obstacleLayer);
        if(hitObject) return true;

        hitObject = Physics.Raycast(this.transform.position, direction, viewRadius, LayerMask.GetMask("Cage"));
        return hitObject;
    }

    // 角度内にいるか調べる
    private bool IsWithinAngle()
    {
        Vector2 toChild = new Vector2(
            child.transform.position.x - this.transform.position.x,
            child.transform.position.y - this.transform.position.y);

        Vector2 direction = new Vector2(transform.right.x, transform.right.y);

        float angle = Vector3.Angle(direction, toChild);

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
