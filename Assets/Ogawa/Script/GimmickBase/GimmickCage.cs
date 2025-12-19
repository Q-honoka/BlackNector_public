using UnityEngine;

public class GimmickCage : GimmickBase
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Collider coll;
    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        rigid.useGravity = true;
        rigid.isKinematic = false;
        coll.isTrigger = true;
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {

    }
}
