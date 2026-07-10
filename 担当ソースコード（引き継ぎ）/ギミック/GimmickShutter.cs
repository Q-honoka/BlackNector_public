using RaruLib;
using UnityEngine;

public class GimmickShutter : GimmickBase
{
    [SerializeField] bool isOpen = false;
    private Animator anim;
    private Sound _sound => Sound.instance;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsOpen", isOpen);

        // 始めから開いている場合はアニメーションをスキップ
        if(isOpen)
        {
            anim.Play("DoorOpen", 0, 1.0f);
        }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        _sound.Play("SE","Open_door");
        isOpen = !isOpen;
        anim.SetBool("IsOpen", isOpen);
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        _sound.Play("SE", "Close_door");
        isOpen = !isOpen;
        anim.SetBool("IsOpen", isOpen);
    }
}
