using UnityEngine;

public class GimmickButton : GimmickBase
{
    [SerializeField]
    GimmickCollision collision;
    bool myState = true;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (collision == null) { return; }

        if (collision.awakeGimmick)
        {
            myState ^= true;
            isState = myState;
            collision.awakeGimmick = false;
        }

    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        // true にしたときに起動したいギミックの State を true にする
        foreach (GameObject obj in stateTrueObj)
        {
            if (obj != null)
            {
                GimmickBase gimmick = obj.GetComponent<GimmickBase>();
                if(gimmick != null)
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

}
