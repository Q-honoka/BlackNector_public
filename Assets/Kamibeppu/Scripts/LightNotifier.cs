using UnityEngine;
using UnityEngine.Rendering.Universal;

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
[RequireComponent(typeof(Light2D))]
public class LightNotifier : MonoBehaviour
{
    private Light2D light2D;
    private bool isEnable;          // Light2Dがついているオブジェクトの状態
    private static ActiveLightTracker tracker;  // ライトのトラッカースクリプト

    private void Start()
    {
        light2D = GetComponent<Light2D>();
        isEnable = light2D.gameObject.activeSelf;

        // トラッカーを取得
        if (tracker == null)
        {
            tracker = ActiveLightTracker.Instance;
        }

        // 現在のライトの状態を通知する
        if (tracker != null)
        {
            tracker?.UpdateActiveLights(light2D);
        }
    }

    // ライトの状態が変化したことを通知する
    private void NotifyLightStateChanged()
    {
        if (tracker != null)
        {
            tracker?.UpdateActiveLights(light2D);
        }
    }

    // ライトオブジェクトが無効になったときにトラッカーに通知
    private void OnDisable()
    {
        NotifyLightStateChanged();
    }

    // ライトオブジェクトが有効になったときにトラッカーに通知
    private void OnEnable()
    {
        NotifyLightStateChanged();
    }
}
