using UnityEngine;

public class Command : MonoBehaviour
{
    public void LogOutForGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
