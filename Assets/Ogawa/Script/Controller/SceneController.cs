using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public enum SCENE
{
    TITLE,
    GAME,
    MAX
}

public class SceneController : MonoBehaviour
{
    private Retry retry;
    private GameData gameData;

    [SerializeField] private string[] scenes = new string[(int)SCENE.MAX];

    public static SceneController instance;

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

        if (GameData.instance != null)
        {
            gameData = GameData.instance;
        }
        else
        {
            Debug.Log("小川：GameDataが存在しません", gameObject);
        }

        retry.OnRetry
            .Subscribe(kind => {
                OnRetry();
            });
    }

    // リトライイベント受信
    public void OnRetry()
    {
        if (gameData == null)
        {
            return;
        }
        SceneChange((int)gameData.saveSpot);
    }

    public void SceneChange(int scene)
    {
        SceneManager.LoadScene(scenes[scene]);
    }


}
