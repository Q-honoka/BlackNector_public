using UnityEngine;
using UniRx;

public class PlayerData : MonoBehaviour
{
    private Retry retry;

    [SerializeField] Vector3[] retrySpot = new Vector3[(int)SaveSpotKind.MAX];

    private void Start()
    {
        if (Retry.instance != null)
        {
            retry = Retry.instance;
        }
        else
        {
            Debug.Log("小川：Retryが存在しません", gameObject);
        }

        retry.OnRetry
            .Subscribe(kind => {
                MoveResetSpot();
            })
            .AddTo(this); ;
    }

    // 位置をリセット
    private void MoveResetSpot()
    {
        if(GameData.instance == null)
        {
            Debug.Log("プレイヤー位置リセット失敗。ゲームデータがない", gameObject);
        }

        transform.position = retrySpot[(int)GameData.instance.saveSpot];
    }
}
