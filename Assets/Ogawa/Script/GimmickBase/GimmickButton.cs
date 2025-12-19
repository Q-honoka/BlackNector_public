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
        foreach (GameObject obj in stateTrueObj)
        {
            if (obj != null)
                obj.SetActive(!obj.activeSelf);
        }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        isState = false;
        foreach (GameObject obj in stateTrueObj)
        {
            if (obj != null)
                obj.SetActive(!obj.activeSelf);
        }
    }

}
