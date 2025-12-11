using UnityEngine;

public class GimmickButton : GimmickBase
{
    [SerializeField]
    GimmickCollision collision;
    bool myState = true;

    private void Start()
    {
        isState = false;
        isState = true;
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
        isState = true;
        stateTrueObj.SetActive(true);
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        stateTrueObj.SetActive(false);
    }

}
