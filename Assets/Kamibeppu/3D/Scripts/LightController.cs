using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Material mat;
    private const float FirstStageBrightness = -5.0f;
    private const float otherStageBrightness = -1.8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameData.instance != null && mat != null)
        {
            if(GameData.instance.saveSpot == SaveSpotKind.Stage1_1)
            {
                // –¾‚é‚­‚·‚é
                mat.SetFloat("_FilterAmount", FirstStageBrightness);
            }
            else
            {
                // ˆÃ‚­‚·‚é
                mat.SetFloat("_FilterAmount", -otherStageBrightness);
            }
        }
    }
}
