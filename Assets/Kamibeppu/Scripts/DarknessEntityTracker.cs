using UnityEngine;
using System.Collections.Generic;

/*
 * このスクリプトは
 * 「暗闇判定したいエンティティ」
 * を管理するスクリプトです。
 * 
 * シーンの始めに対象のエンティティをすべて取得したあと
 * 定期的に存在を調べます。
 * 存在が消されたら、リストから削除します。
 */

[DefaultExecutionOrder(-5)]
public class DarknessEntityTracker : MonoBehaviour
{
    /// <summary>
    /// エンティティトラッカーのシングルトン
    /// </summary>
    public static DarknessEntityTracker Instance { get; private set; }

    [SerializeField]
    private float rescanInterval = 1.0f;    // 再スキャンの間隔

    // アクティブなエンティティのリスト
    private List<DarknessTarget> darknessEntities = new();

    private void Awake()
    {
        // シングルトン化
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン内のすべての DarknessTarget クラスをもつオブジェクトを取得する
        DarknessTarget[] objects = FindObjectsByType<DarknessTarget>(FindObjectsSortMode.None);
        
        // 判定対象としてリストに加える
        foreach(DarknessTarget obj in objects)
        {
            darknessEntities.Add(obj);
        }

        InvokeRepeating(nameof(ReScanList), 0f, rescanInterval);
    }

    // リスト内を再スキャンする関数
    private void ReScanList()
    {
        // Destroyされたエンティティをリストから削除する
        darknessEntities.RemoveAll(entity => entity == null);
    }

    /// <summary>
    /// 対象エンティティのリストを返す
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<DarknessTarget> GetDarknessEntities()
    {
        return darknessEntities.AsReadOnly();
    }
}
