using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public enum SCENE
    {
        TITLE,
        GAME,
        MAX
    }

    [SerializeField] private string[] scenes = new string[(int)SCENE.MAX];

    public void SceneChange(int scene)
    {
        SceneManager.LoadScene(scenes[scene]);
    }
}
