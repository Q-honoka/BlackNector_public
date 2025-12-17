using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GameMenuPanel : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log("è¨êÏÅFÇ≈ÇΩÅI");
        if(m_Animator == null) return;
        m_Animator.SetTrigger("OnView");
    }
}
