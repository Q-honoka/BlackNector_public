using UnityEngine;

public class GimmickElectric_stage6 : GimmickBase
{
    private GameObject Light;    // ライトオブジェクト
    private void Start()
    {
        Light = this.GetComponentInChildren<Light>().gameObject;
        if (Light == null) { Debug.LogWarning($"{this.name}: ライトオブジェクトがない"); }
        Light.SetActive( true );
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        Light.SetActive(false);
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        Light.SetActive(true);
    }
}
