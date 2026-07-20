using UnityEngine;

/*
 * このスクリプトは
 * ライトの「オン」「オフ」
 * をトラッカーに通知するスクリプトです。
 * 
 * 暗闇判定に使用する
 * ライトオブジェクトに
 * 必ず付けてください。
 * 
 * オブザーバーパターンを使用して
 * オン または オフになったときに
 * ライトを管理するスクリプトに通知します。
 */

[DefaultExecutionOrder(-10)]
public class LightNotifier : MonoBehaviour
{
    [SerializeField]
    private LayerMask ObstacleLayer;    // 障害物レイヤー

    private Light light3D;
    private static ActiveLightTracker tracker;  // ライトのトラッカースクリプト
    private RenderFanShape renderFan;   // メッシュ描画クラスのインスタンス
    private int segmentCount = 32;      // 扇形の滑らかさ

    private void Start()
    {
        light3D = GetComponent<Light>();
        renderFan = GetComponentInChildren<RenderFanShape>();

        // トラッカーを取得
        if (tracker == null)
        {
            tracker = ActiveLightTracker.Instance;
        }

        // 現在のライトの状態を通知する
        if (tracker != null)
        {
            tracker?.UpdateActiveLights(light3D);
        }

        if(renderFan != null)
        {
            renderFan.SetRenderInfo(light3D.range, light3D.spotAngle, segmentCount, ObstacleLayer, true);
        }
    }

    /// <summary>
    /// ライトの状態が変化したことを通知する
    /// </summary>
    private void NotifyLightStateChanged()
    {
        if (tracker != null)
        {
            tracker?.UpdateActiveLights(light3D);
        }
    }

    /// <summary>
    /// ライトオブジェクトが無効になったときにトラッカーに通知
    /// </summary>
    private void OnDisable()
    {
        NotifyLightStateChanged();
        if(renderFan != null) renderFan.enabled = false;
    }

    /// <summary>
    /// ライトオブジェクトが有効になったときにトラッカーに通知
    /// </summary>
    private void OnEnable()
    {
        NotifyLightStateChanged();
        if(renderFan != null) renderFan.enabled = true;
    }
}
