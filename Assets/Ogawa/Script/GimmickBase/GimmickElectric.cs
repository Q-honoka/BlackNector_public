using UnityEngine;

public class GimmickElectric : GimmickBase
{
    private GameObject Light;    // ライトオブジェクト
    [SerializeField] bool isOn;
    private void Start()
    {
        Light = this.GetComponentInChildren<Light>().gameObject;
        if (Light == null) { Debug.LogWarning($"{this.name}: ライトオブジェクトがない"); }

        Light.SetActive(isOn);
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        isOn = !isOn;
        Light.SetActive(isOn);
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        isOn = !isOn;
        Light.SetActive(isOn);
    }
}
