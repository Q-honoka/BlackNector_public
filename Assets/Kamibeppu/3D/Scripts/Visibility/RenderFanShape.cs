using UnityEngine;

/*
 *  このスクリプトは扇形のメッシュを描画します。
 *  外部から情報をもらうと描画できます。
 */

public class RenderFanShape : MonoBehaviour
{
    [SerializeField]
    private bool RenderUpdate = false;      // 毎フレーム描画しなおすか

    private const int triangleVertexCount = 3;      // 三角形の頂点数

    private Mesh mesh;          // 扇形を生成するメッシュ
    private MeshFilter filter;  // メッシュを適用するフィルター

    private float radius = 5.0f;        // 半径
    private float angle = 90f;          // 角度
    private int segmentCount = 48;      // 扇形の滑らかさ
    private LayerMask obstacleLayer;    // 障害物レイヤー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        if (mesh == null) mesh = new Mesh();
        if (filter != null) filter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if(RenderUpdate)
        {
            RenderShape();
        }
    }

    /// <summary>
    /// メッシュの描画情報を設定する
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="ang">角度</param>
    /// <param name="segCount">扇形の滑らかさ</param>
    /// <param name="ObLayer">貫通させないレイヤー</param>
    public void SetRenderInfo(float r, float ang, int segCount, LayerMask ObLayer, bool updateRender)
    {
        radius = r;
        angle = ang;
        segmentCount = segCount;
        obstacleLayer = ObLayer;
        RenderUpdate = updateRender;

        // 情報をセットしたらメッシュを描画する
        RenderShape();
    }

    /// <summary>
    /// 視界の描画
    /// </summary>
    private void RenderShape()
    {
        // メッシュがない場合は処理しない
        if (mesh == null) return;
        mesh.Clear();

        float half = angle * 0.5f;
        float start = -half;
        float step = angle / segmentCount;

        Vector3[] vertices = new Vector3[2 + segmentCount];     // 頂点数(中心+扇の端+分割数)

        // 中心点を設定
        vertices[0] = Vector3.zero;

        // 各頂点の設定
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = (start + step * i) * Mathf.Deg2Rad;   // ラジアン
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            Vector3 origin = transform.position;
            Quaternion rotation = this.transform.rotation;

            Vector3 localDir = new Vector3(x, y, 0f);
            Vector3 dir = rotation * localDir;
            RaycastHit hit;
            Vector3 targetPoint;

            // 障害物にぶつかったらぶつかった場所を保存
            if (Physics.Raycast(origin, dir, out hit, radius))
            {
                // 檻もしくは障害物があったら檻の場所を保存
                if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = origin + dir * radius;
                }
            }
            // ぶつからなければ最大距離を保存
            else
            {
                targetPoint = origin + dir * radius;
            }

            Vector3 localVer = this.transform.InverseTransformPoint(targetPoint);
            vertices[i + 1] = localVer;     // 頂点をセット
        }

        // 三角形の設定
        int[] triangles = new int[segmentCount * triangleVertexCount];
        for (int i = 0; i < segmentCount; i++)
        {
            triangles[i * triangleVertexCount] = 0;           // 中心
            triangles[i * triangleVertexCount + 1] = i + 1;
            triangles[i * triangleVertexCount + 2] = i + 2;
        }

        // 頂点と三角形の情報をメッシュに格納する
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
