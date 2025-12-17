using UniRx;
using UnityEngine;

public enum SaveSpot
{
    Stage1_1,
    Stage2_1,
    Stage3_1,
    Stage4_1,
    Stage5_1,
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

    private SaveSpot _saveSpot;
    public  SaveSpot saveSpot => _saveSpot;

    // 最終セーブ地点更新
    public void SaveFromSaveSpot(SaveSpot spot)
    {
        if (spot == SaveSpot.MAX)
        {
            Debug.Log("セーブ失敗", gameObject);
            return;
        }

        _saveSpot = spot;
    }
}
