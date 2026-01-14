using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveSpot : MonoBehaviour
{
    [SerializeField] SaveSpotKind saveSpotKind = SaveSpotKind.Stage1_1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Child"))
        {
            return;
        }

        if(GameData.instance == null)
        {
            return;
        }

        GameData.instance.SaveFromSaveSpot(saveSpotKind);
    }
}
