using UnityEngine;

public class GimmickCurtain : GimmickBase
{
    private Light Light;
    private void Start()
    {
        Light = GetComponentInChildren<Light>();
        if (Light != null) { Light.enabled = false; }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        // 開くアニメーション

        // ライトをつける
        if (Light != null) { Light.enabled = true; }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {

    }
}
