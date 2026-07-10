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

    // アクティブなエンティティのリスト
    private List<DarknessTarget> darknessEntities = new();

    private void Awake()
    {
        // シングルトン化
        if (Instance != null && Instance != this)
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
        foreach (DarknessTarget obj in objects)
        {
            UpdateActiveEntities(obj);
        }
    }

    /// <summary>
    /// エンティティの状態に応じてアクティブなエンティティのリストを更新します。
    /// エンティティが有効なら追加、無効なら削除します。
    /// </summary>
    /// <param name="entity"></param>
    public void UpdateActiveEntities(DarknessTarget entity)
    {
        if (entity.gameObject.activeSelf)
        {
            AddActiveEntity(entity);
        }
        else
        {
            RemoveActiveEntity(entity);
        }
    }

    /// <summary>
    /// 対象エンティティのリストを返す
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<DarknessTarget> GetDarknessEntities()
    {
        return darknessEntities.AsReadOnly();
    }

    // アクティブなエンティティをリストに追加する関数
    private void AddActiveEntity(DarknessTarget entity)
    {
        // リストにそのエンティティがない かつ エンティティが有効の場合は追加する
        if (!darknessEntities.Contains(entity) && entity.gameObject.activeSelf)
        {
            darknessEntities.Add(entity);
        }
    }

    // 非アクティブなエンティティをリストから削除する関数
    private void RemoveActiveEntity(DarknessTarget entity)
    {
        // リストにそのエンティティがある かつ エンティティが無効の場合は削除する
        if (darknessEntities.Contains(entity) && !entity.gameObject.activeSelf)
        {
            darknessEntities.Remove(entity);
        }
    }

}
