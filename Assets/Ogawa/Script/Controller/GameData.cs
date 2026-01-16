using System;
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
    Stage8_1,
    MAX
}

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private Subject<SaveSpotKind> SaveSpotUpdateSubject = new Subject<SaveSpotKind>();    // セーブスポットが変更された時のイベント

    public IObservable<SaveSpotKind> OnSaveSpotUpdate
    {
        get
        {
            return SaveSpotUpdateSubject;
        }
    }

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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private SaveSpotKind _saveSpot = SaveSpotKind.Stage1_1;
    public  SaveSpotKind saveSpot => _saveSpot;

    // 最終セーブ地点更新
    public void SaveFromSaveSpot(SaveSpotKind spot)
    {
        if (spot == SaveSpotKind.MAX)
        {
            Debug.Log("セーブ失敗", gameObject);
            return;
        }
        if (spot == _saveSpot)
        {
            Debug.Log("セーブ地点が同じ。更新しない", gameObject);
            return;
        }


        _saveSpot = spot;
        SaveSpotUpdateSubject.OnNext(_saveSpot);  
        Debug.Log("セーブ成功",gameObject);
    }
}
