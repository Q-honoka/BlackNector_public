using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] LayerMask ObstacleLayer;
    [SerializeField]
    private float checkInterval = 1f;       // 暗闇をチェックする間隔

    // エンティティトラッカーのシングルトン
    private DarknessEntityTracker entityTracker;
    // ライトトラッカーのシングルトン
    private ActiveLightTracker lightTracker;

    private IReadOnlyList<DarknessTarget> targetEntities;   // 判定対象エンティティのリスト
    private IReadOnlyList<Light> targetLights;              // 判定に使用するライトのリスト
    private List<DarknessTarget> entitiesInDarkness;        // 暗闇にいるエンティティのリスト

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
    /// 暗闇にいるエンティティを調べる
    /// </summary>
    private void CheckDarknessEntities()
    {
        entitiesInDarkness.Clear();

        // すべてのエンティティに対して暗闇判定を行う
        foreach (DarknessTarget entity in targetEntities)
        {
            bool isDarkness = true;
            foreach (Light light in targetLights)
            {
                if (IsWithinSpotLightArea(entity, light) == true)
                {
                    isDarkness = false;
                }
            }

            // 暗闇フラグが true のままだったらリストに加える
            if (isDarkness == true)
            {
                entitiesInDarkness.Add(entity);
            }
        }

    }

    /// <summary>
    /// エンティティがスポットライトの照射範囲内にいるか調べる
    /// </summary>
    /// <param name="entity">調べる対象のエンティティ</param>
    /// <param name="light">調べる対象のライト</param>
    /// <returns></returns>
    private bool IsWithinSpotLightArea(DarknessTarget entity, Light light)
    {
        // 半径で簡易チェック
        Vector3 lightPos = light.gameObject.transform.position;
        Vector3 entityPos = entity.gameObject.transform.position;
        float distance = Vector3.Distance( entityPos, lightPos );
        float range = light.range;

        // 半径よりも距離が大きい場合は暗いため false を返す
        if (distance > range)
        {
            return false;
        }

        // エンティティがライト内にいるか角度を調べる
        Vector3 lightDir = light.gameObject.transform.forward;
        Vector3 toEntity = (entityPos - lightPos).normalized;
        float angle = Vector3.Angle(lightDir, toEntity);

        // ライトとエンティティの角度がライトの角度より大きければ暗いため false を返す
        if (angle > light.spotAngle / 2)
        {
            return false;
        }

        // レイをとばして間に遮蔽物があれば false なければ true を返す
        return Physics.Raycast(lightPos, toEntity, range, ObstacleLayer);
    }
}
