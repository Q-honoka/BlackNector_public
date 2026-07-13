using UnityEngine;

/*
 * 暗闇判定をしたいエンティティのRendererがついているオブジェクトに
 * 必ずこのスクリプトをアタッチしてください。
 * 
 * オブジェクトがカメラに映っているかどうかを保持します。
 */

public class CameraVisible : MonoBehaviour
{
    public bool visible;

    /// <summary>
    /// カメラから外れた瞬間にフラグを false にする
    /// </summary>
    private void OnBecameInvisible()
    {
        visible = false;
    }

    /// <summary>
    /// カメラ内に入った瞬間にフラグを true にする
    /// </summary>
    private void OnBecameVisible()
    {
        visible = true;
    }
}
