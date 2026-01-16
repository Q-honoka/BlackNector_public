using UnityEngine;

public class GimmickCurtain : GimmickBase
{
    private Light Light;
    private Animator anim;

    private void Start()
    {
        Light = GetComponentInChildren<Light>();
        if (Light != null) { Light.enabled = false; }
        anim = this.gameObject.transform.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        if (anim != null) anim.SetBool("IsOpen", true);
        // ライトをつける
        if (Light != null) { Light.enabled = true; }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        if (anim != null) anim.SetBool("IsOpen", false);
        // ライトを消す
        if (Light != null) { Light.enabled = false; }
    }
}
