using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * このスクリプトは
 * 「暗闇にいるエンティティを判定」
 * するスクリプトです。
 * 
 * 「ライトが当たっていない」
 * ことを暗闇と定義しています。
 */

// エンティティの取得が終わってから実行するために処理を遅らせる
[DefaultExecutionOrder(+30)]
public class DarknessSensor : MonoBehaviour
{
    /// <summary>
    /// 暗闇判定センサーのシングルトン
    /// </summary>
    public static DarknessSensor Instance { get; private set; }

    [SerializeField]
    private float checkInterval = 1f;

    // エンティティトラッカーのシングルトン
    private DarknessEntityTracker entityTracker;
    // ライトトラッカーのシングルトン
    private ActiveLightTracker lightTracker;
    private IReadOnlyList<DarknessTarget> targetEntities;   // 判定対象エンティティのリスト
    private IReadOnlyList<Light> targetLights;            // 判定に使用するライトのリスト
    private List<DarknessTarget> entitiesInDarkness;        // 暗闇にいるエンティティのリスト

    // 子どもの暗闇フラグ
    private bool isDarknessChild = false;
    // 子どものオブジェクト
    private GameObject child = null;

    private void Awake()
    {
        // シングルトン化
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityTracker = DarknessEntityTracker.Instance;
        lightTracker = ActiveLightTracker.Instance;
        targetEntities = entityTracker.GetDarknessEntities();
        targetLights = lightTracker.GetActiveLights();

        entitiesInDarkness = new();

        InvokeRepeating(nameof(CheckDarknessEntities), 0f, checkInterval);
    }

    /// <summary>
    /// 渡されたエンティティが暗闇にいるかどうかを返す
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public bool GetSelfIsDarkness(DarknessTarget self)
    {
        if (entitiesInDarkness.Contains(self))
            return true;

        return false;
    }

    /// <summary>
    /// 渡されたエンティティをリストから削除する
    /// </summary>
    /// <param name="self"></param>
    public void RemoveSelfInDarkness(DarknessTarget self)
    {
        if (self != null && entitiesInDarkness.Contains(self))
            entitiesInDarkness.Remove(self);
    }

    // 暗闇にいるエンティティのリストを初期化する
    private void ClearEntitiesInDarkness()
    {
        bool preIsDarknessChild = isDarknessChild;

        // 子どもが暗闇にいるか取得する
        isDarknessChild = IsInChildEntity();

        // 変更が確認されたら、処理を分岐する
        if (preIsDarknessChild != isDarknessChild && child != null)
        {
            // 子どもの暗闇処理関数を呼び出す

        }

        entitiesInDarkness.Clear();
    }

    // 暗闇にいるエンティティの中から子どもを探す
    private bool IsInChildEntity()
    {
        foreach (DarknessTarget entity in entitiesInDarkness)
        {
            // エンティティのタグが子どもなら、 true を返す
            if (entity.CompareTag("Child"))
            {
                // 初めて子どもが暗闇に入ったらゲームオブジェクトを取得
                if (child == null)
                {
                    child = entity.gameObject;
                }
                return true;
            }
        }
        return false;
    }

    // 暗闇にいるエンティティを調べる
    private void CheckDarknessEntities()
    {
        ClearEntitiesInDarkness();

        // すべてのエンティティに対して暗闇判定を行う
        //foreach (DarknessTarget entity in targetEntities)
        //{
        //    bool isDarkness = true;
        //    foreach (Light light2D in targetLights)
        //    {
        //        if (IsWithinLightArea(entity, light2D) == true && IsLightPathBlocked(entity, light2D) == false)
        //        {
        //            isDarkness = false;
        //        }
        //    }

        //    // 暗闇フラグが true のままだったらリストに加える
        //    if (isDarkness == true)
        //    {
        //        entitiesInDarkness.Add(entity);
        //        Debug.Log($"{entity.name}を暗闇リストに加えます");
        //        Debug.Log($"暗闇にいるエンティティの数：{entitiesInDarkness.Count}");
        //    }
        //}

    }

    // エンティティがライトの照射範囲内にいるか調べる
    //private bool IsWithinLightArea(DarknessTarget entity, Light2D light2D)
    //{
    //    // ライトがスポットライトの照射角度内にいたら true を返す
    //    if (light2D.lightType == Light2D.LightType.Point)
    //    {
    //        return IsWithinSpotLightArea(entity, light2D);
    //    }

    //    // ライトがフリーフォームの範囲内にいたら true を返す
    //    if (light2D.lightType == Light2D.LightType.Freeform)
    //    {
    //        return IsWithinFreeformLightPath(entity, light2D);
    //    }

    //    // ライトのタイプが スポットライト と フリーフォーム のどちらでもない場合は false を返す
    //    Debug.Log("タイプが合致しません");
    //    return false;
    //}

    //// エンティティがスポットライトの照射角度内にいるか調べる
    //private bool IsWithinSpotLightArea(DarknessTarget entity, Light2D light2D)
    //{
    //    Vector2 entityPos = entity.gameObject.transform.position;
    //    Vector2 light2DPos = light2D.transform.position;
    //    // ライトのエンティティの距離を計算（照射範囲の簡易チェックに使用）
    //    float distance = (entityPos - light2DPos).sqrMagnitude;
    //    float radius = light2D.pointLightOuterRadius;

    //    // 半径より距離が大きい場合は処理を終了
    //    if (radius * radius < distance)
    //        return false;

    //    Vector2 light2DDir = light2D.transform.up;
    //    Vector2 toEntity = (entityPos - light2DPos).normalized;     // ライトからエンティティへの方向ベクトル

    //    float angle = Vector2.Angle(light2DDir, toEntity);
    //    float spotAngle = light2D.pointLightOuterAngle;

    //    // ライトの角度内にいるかどうかを判定（スポットライトの範囲）
    //    return angle <= spotAngle / 2f;
    //}

    //// エンティティがフリーフォームの範囲内にいるか調べる
    //private bool IsWithinFreeformLightPath(DarknessTarget entity, Light2D light2D)
    //{
    //    Vector3 entityPos = entity.transform.position;
    //    Vector3[] light2DPath = light2D.shapePath;      // ライトの照射範囲の各頂点座標を取得
    //    int lightPathCount = light2DPath.Length;        // ライトの頂点数
    //    bool isWithinFreeformLight = true;              // 範囲内にいるかフラグ

    //    // ライトの各頂点と調べる
    //    for (int i = 0; i < lightPathCount; i++)
    //    {
    //        // 現在のライト頂点 から 次のライト頂点 への方向ベクトルを求める
    //        int nextLightIndex = (i + 1) % light2DPath.Length;
    //        Vector3 lightLocalPos = new Vector3(light2DPath[i].x, light2DPath[i].y, 0);
    //        Vector3 lightWorldPos = transform.TransformPoint(lightLocalPos);            // ローカル座標をワールド座標に変換
    //        Vector3 nextLightLocalPos = new Vector3(light2DPath[nextLightIndex].x, light2DPath[nextLightIndex].y, 0);
    //        Vector3 nextLightWorldPos = transform.TransformPoint(nextLightLocalPos);    // ローカル座標をワールド座標に変換

    //        // 外積を使って方向ベクトルの左右どちらにいるか調べる
    //        Vector3 nextLightDir = (nextLightWorldPos - lightWorldPos).normalized;      // 次のライトへの方向ベクトル
    //        Vector3 toEntityDir = (entityPos - lightWorldPos).normalized;               // エンティティへの方向ベクトル
    //        Vector3 cross = Vector3.Cross(nextLightDir, toEntityDir);

    //        // 外積の値が負の値なら範囲の外(左側)にいるため false を代入する
    //        if (cross.z < 0)
    //        {
    //            isWithinFreeformLight = false;
    //        }
    //    }

    //    // 最終結果を返す
    //    return isWithinFreeformLight;
    //}

    //// ライトとエンティティの間で光が遮られているか調べる
    //private bool IsLightPathBlocked(DarknessTarget entity, Light2D light2D)
    //{
    //    Vector2 start = light2D.transform.position;
    //    Vector2 end = entity.transform.position;

    //    RaycastHit2D[] hit = Physics2D.LinecastAll(start, end);

    //    // 衝突したオブジェクトすべてと 光が遮られているか 調べる
    //    foreach (RaycastHit2D ray in hit)
    //    {
    //        Collider2D collider = ray.collider;
    //        if (collider == null)
    //            continue;

    //        // エンティティでなく かつ 光を遮っているなら true を返す
    //        if (collider.gameObject.GetComponent<DarknessTarget>() == null &&
    //            collider.gameObject.GetComponent<ShadowCaster2D>() != null)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
