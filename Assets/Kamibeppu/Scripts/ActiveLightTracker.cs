using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * このスクリプトは
 * 「アクティブなライト」
 * を管理するスクリプトです。
 * 
 * ライトがオンになったら
 * オンのリストに加えます。
 * 
 * ライトがオフになったら
 * オンのリストから削除します。
 */

[DefaultExecutionOrder(-5)]
public class ActiveLightTracker : MonoBehaviour
{
    /// <summary>
    /// ライトトラッカーのシングルトン
    /// </summary>
    public static ActiveLightTracker Instance { get; private set; }

    // アクティブなライトのリスト
    private List<Light2D> activeLights = new();

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

    /// <summary>
    /// ライトの状態に応じてアクティブなライトのリストを更新します。
    /// ライトが有効なら追加、無効なら削除します。
    /// </summary>
    /// <param name="light"></param>
    public void UpdateActiveLights(Light2D light)
    {
        if (light.gameObject.activeSelf)
        {
            AddActiveLight(light);
        }
        else
        {
            RemoveActiveLight(light);
        }
    }

    /// <summary>
    /// アクティブなライトのリストを返す関数
    /// (読み取り専用)
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Light2D> GetActiveLights()
    {
        return activeLights.AsReadOnly();
    }

    // アクティブなライトをリストに追加する関数
    private void AddActiveLight(Light2D light2D)
    {
        // リストにそのライトがない かつ ライトオブジェクトが有効の場合は追加する
        if (!activeLights.Contains(light2D) && light2D.gameObject.activeSelf)
        {
            activeLights.Add(light2D);
        }
    }

    // 非アクティブなライトをリストから削除する関数
    private void RemoveActiveLight(Light2D light2D)
    {
        // リストにそのライトがある かつ ライトオブジェクトが無効の場合は削除する
        if (activeLights.Contains(light2D) && !light2D.gameObject.activeSelf)
        {
            activeLights.Remove(light2D);
        }
    }
}
