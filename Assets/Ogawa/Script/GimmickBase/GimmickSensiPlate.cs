using Unity.VisualScripting;
using UnityEngine;

public class GimmickSensiPlate : GimmickBase
{
    [SerializeField] private GameObject button;     // 押されたときに引っ込むゲームオブジェクト

    private Vector3 initPos;        // 初期位置
    private GameObject visitor;     // 侵入したオブジェクト

    private void Start()
    {
        initPos = transform.position;
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        if (button != null) { button.transform.localPosition = new Vector3(0, 0, 0); }
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
        if (button != null) { button.transform.position = initPos; }
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

    private void OnTriggerStay(Collider other)
    {
        // 鳥以外がトリガーに触れたら、起動する
        if (other.CompareTag("Child") || other.CompareTag("enemy"))
        {
            isState = true;
            visitor = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 侵入者がトリガーから出たら、終了する
        if (other.gameObject == visitor)
        {
            isState = false;
        }
    }
}
