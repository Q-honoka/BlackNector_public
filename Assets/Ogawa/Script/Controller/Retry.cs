using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public static Retry instance;

    /// <summary>
    /// 今回のシーン読み込みが「リトライによるものか」
    /// </summary>
    public bool IsRetryRequested { get; private set; }

    private Subject<Unit> retrySubject = new Subject<Unit>();

    /// <summary>
    /// リトライイベント
    /// </summary>
    public IObservable<Unit> OnRetry => retrySubject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// リトライを呼ぶ
    /// </summary>
    public void CallRetry()
    {
        if (SceneController.instance == null)
        {
            Debug.Log("リトライ失敗。シーン遷移用スクリプトがない", gameObject);
            return;
        }

        IsRetryRequested = false;            
        retrySubject.OnNext(Unit.Default);
        SceneController.instance.SceneReLoad();
    }
}