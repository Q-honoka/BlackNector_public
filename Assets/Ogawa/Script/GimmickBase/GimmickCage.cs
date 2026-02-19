using RaruLib;
using UnityEngine;

public class GimmickCage : GimmickBase
{
    private Rigidbody rigid;
    private Sound _sound => Sound.instance;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        _sound.Play("SE","Open_cage");
        rigid.useGravity = true;
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        
    }
}
