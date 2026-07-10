using UnityEngine;
using UnityEngine.Playables;

public class StartMovieEndStart : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector endStartDirector;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Child"))
        {
            endStartDirector.Play();
        }
    }
}
