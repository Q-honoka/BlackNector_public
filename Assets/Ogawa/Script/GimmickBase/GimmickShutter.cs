using UnityEngine;

public class GimmickShutter : GimmickBase
{
    [SerializeField] bool isOpen = false;   // 開いているか
    [SerializeField, Header("デバッグ用扉")] GameObject door;

    private void Update()
    {
        if(isOpen)
        {
            // 開くアニメーション(未実装)
            door.SetActive(false);
        }
        else
        {
            // 閉まるアニメーション(未実装)
            door.SetActive(true);
        }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        isOpen = !isOpen;
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        isOpen = !isOpen;
    }
}
