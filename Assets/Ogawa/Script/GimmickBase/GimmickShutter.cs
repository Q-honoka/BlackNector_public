using UnityEngine;

public class GimmickShutter : GimmickBase
{
    [SerializeField] bool isOpen = false;   // 開いているか
    [SerializeField, Header("デバッグ用扉")] GameObject door;

    private void Update()
    {
        if(isOpen)
        {
            // 開くアニメーション
            Debug.Log($"{name}: 開く");
            door.SetActive(false);
        }
        else
        {
            // 閉まるアニメーション
            Debug.Log($"{name}: 閉まる");
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
