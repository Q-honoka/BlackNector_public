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

    // カメラから外れた瞬間にフラグを false にする
    private void OnBecameInvisible()
    {
        visible = false;
    }

    // カメラ内に入った瞬間にフラグを true にする
    private void OnBecameVisible()
    {
        visible = true;
    }
}
