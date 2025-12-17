using UniRx;
using UnityEngine;

public enum SaveSpotKind
{
    Stage1_1,
    Stage2_1,
    Stage3_1,
    Stage4_1,
    Stage5_1,
    Stage6_1,
    Stage7_1,
    MAX
}

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private SaveSpotKind _saveSpot;
    public  SaveSpotKind saveSpot => _saveSpot;

    // 最終セーブ地点更新
    public void SaveFromSaveSpot(SaveSpotKind spot)
    {
        if (spot == SaveSpotKind.MAX)
        {
            Debug.Log("セーブ失敗", gameObject);
            return;
        }

        _saveSpot = spot;
    }
}
