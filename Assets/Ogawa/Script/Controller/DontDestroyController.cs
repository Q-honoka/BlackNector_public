using Unity.VisualScripting;
using UnityEngine;

public class DontDestroyController : MonoBehaviour
{
    private static DontDestroyController instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
