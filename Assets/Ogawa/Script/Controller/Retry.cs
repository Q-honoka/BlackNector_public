using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public static Retry instance;

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

    private Subject<Unit> retrySubject = new Subject<Unit>();

    // リトライイベント
    public IObservable<Unit> OnRetry
    {
        get
        {
            return retrySubject;
        }
    }

    // リトライを呼ぶ
    public void CallRetry()
    {
        if(SceneController.instance == null)
        {
            Debug.Log("リトライ失敗。シーン遷移用スクリプトがない",gameObject);
            return;
        }

        CalledRetry().Forget();

    }

    private async UniTaskVoid CalledRetry()
    {
        SceneController.instance.SceneChange((int)SCENE.GAME);
        await UniTask.WaitForSeconds(1); しねーー
        retrySubject.OnNext(Unit.Default);
    }
}
