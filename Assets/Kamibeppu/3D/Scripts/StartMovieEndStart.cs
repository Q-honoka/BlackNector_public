using UnityEngine;
using UnityEngine.Playables;

public class StartMovieEndStart : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector endStartDirector;

    /// <summary>
    /// 子どもに触れたら、最終ステージの演出を開始する
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Child"))
        {
            endStartDirector.Play();
        }
    }
}
