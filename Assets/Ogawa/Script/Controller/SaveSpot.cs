using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveSpot : MonoBehaviour
{
    [SerializeField] SaveSpotKind saveSpotKind = SaveSpotKind.Stage1_1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Child"))
        {
            Debug.Log($"K:子ども以外が触れた。{other.gameObject.name}");
            return;
        }

        Debug.Log($"K:インスタンスがあるか調べる");

        if (GameData.instance == null)
        {
            Debug.Log($"K:GameDataのインスタンスがnull");
            return;
        }

        Debug.Log($"K:セーブスポット{saveSpotKind}を子どもが通った");
        GameData.instance.SaveFromSaveSpot(saveSpotKind);
    }
}
