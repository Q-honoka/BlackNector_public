using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveSpot : MonoBehaviour
{
    [SerializeField] SaveSpotKind saveSpotKind = SaveSpotKind.Stage1_1;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("小川：衝突！",gameObject);
        if (!other.gameObject.CompareTag("Child"))
        {
            Debug.Log("小川：プレイヤー以外。セーブしない",gameObject);
            return;
        }

        if(GameData.instance == null)
        {
            Debug.Log("小川：ゲームデータない。セーブしない", gameObject);
            return;
        }

        Debug.Log("小川：セーブするね", gameObject);
        GameData.instance.SaveFromSaveSpot(saveSpotKind);
    }
}
