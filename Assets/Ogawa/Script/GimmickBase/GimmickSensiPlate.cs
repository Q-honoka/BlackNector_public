using UnityEngine;

public class GimmickSensiPlate : GimmickBase
{
    private Animator anim;
    private Collider target;    // 感圧板を踏んでいるコライダー

    private void Start()
    {
        anim = this.gameObject.transform.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        anim.SetBool("IsPushed", true);

        // true にしたときに起動したいギミックの State を true にする
        foreach (GameObject obj in stateTrueObj)
        {
            if (obj != null)
            {
                GimmickBase gimmick = obj.GetComponent<GimmickBase>();
                if (gimmick != null)
                {
                    gimmick.isState = true;
                }
            }
        }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        anim.SetBool("IsPushed", false);

        // false にしたときに停止したいギミックの State を false にする
        foreach (GameObject obj in stateFalseObj)
        {
            if (obj != null)
            {
                GimmickBase gimmick = obj.GetComponent<GimmickBase>();
                if (gimmick != null)
                {
                    gimmick.isState = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵もしくは子どもが踏んだら true
        if (other.CompareTag("enemy") || other.CompareTag("Child"))
        {
            target = other;
            isState = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (target == null) return;

        // 踏んだ相手が離れたら false
        if(other.CompareTag(target.gameObject.tag))
        {
            target = null;
            isState = false;
        }
    }
}
