using UnityEngine;
using RaruLib;

public class RetryEntity : MonoBehaviour
{
    protected Retry retry;
    private Sound _sound => Sound.instance;

    [SerializeField] protected Vector3[] retrySpot = new Vector3[(int)SaveSpotKind.MAX];

    protected virtual void Awake()
    {
        retry = Retry.instance;
    }

    protected virtual void Start()
    {
        if (retry == null)
        {
            Debug.Log("Retryが存在しません", gameObject);
            return;
        }

        if (!retry.IsRetryRequested)
        {
            MoveResetSpot();
        }
    }

    /// <summary>
    /// 位置をリセット
    /// </summary>
    protected virtual void MoveResetSpot()
    {
        if (GameData.instance == null)
        {
            Debug.Log("プレイヤー位置リセット失敗。ゲームデータがない", gameObject);
            return;
        }

        _sound.Stop("SE", "Whitenoise");
        _sound.Stop("SE", "Warning");
        _sound.Stop("SE", "Walk_child");
        _sound.Stop("SE", "Badfeeling");
        _sound.Stop("SE", "Rumble");

        transform.position = retrySpot[(int)GameData.instance.saveSpot];
    }
}