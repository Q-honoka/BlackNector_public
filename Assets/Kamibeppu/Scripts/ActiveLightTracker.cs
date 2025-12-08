using System.Collections.Generic;
using UnityEngine;

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
    private List<Light> activeLights = new();

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
    public void UpdateActiveLights(Light light3D)
    {
        if (light3D.gameObject.activeSelf)
        {
            AddActiveLight(light3D);
        }
        else
        {
            RemoveActiveLight(light3D);
        }
    }

    /// <summary>
    /// アクティブなライトのリストを返す関数
    /// (読み取り専用)
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Light> GetActiveLights()
    {
        return activeLights.AsReadOnly();
    }

    // アクティブなライトをリストに追加する関数
    private void AddActiveLight(Light light3D)
    {
        // リストにそのライトがない かつ ライトオブジェクトが有効の場合は追加する
        if (!activeLights.Contains(light3D) && light3D.gameObject.activeSelf)
        {
            activeLights.Add(light3D);
        }
    }

    // 非アクティブなライトをリストから削除する関数
    private void RemoveActiveLight(Light light3D)
    {
        // リストにそのライトがある かつ ライトオブジェクトが無効の場合は削除する
        if (activeLights.Contains(light3D) && !light3D.gameObject.activeSelf)
        {
            activeLights.Remove(light3D);
        }
    }
}
