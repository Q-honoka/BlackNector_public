using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public IEnemy enemyInterface;
    public CharctersData myData;    //キャラクター共通のデータ
    public float ViewDistance;                 //視界の範囲                                    
    public bool collision;                 //当たり判定
}
