using UnityEngine;

/*
 * 暗闇判定をしたいエンティティのRendererがついているオブジェクトに
 * 必ずこのスクリプトをアタッチしてください。
 * 
 * オブジェクトがカメラに映っているかどうかを保持します。
 */

public class CameraVisible : MonoBehaviour
{
    private SkinnedMeshRenderer targetRenderer;     // 対象のレンダー
    private Camera targetCamera;      // 対象のカメラ

    private void Awake()
    {
        targetRenderer = GetComponent<SkinnedMeshRenderer>();
        targetCamera = Camera.main;
    }

    /// <summary>
    /// カメラに映っているかどうかを返す
    /// </summary>
    public bool Visible
    {
        get
        {
            if (targetRenderer == null) return false;
            if (targetCamera == null) targetCamera = Camera.main;
            if (targetCamera == null) return false;

            // ビューポート座標に変換する
            Vector3 viewportPos = targetCamera.WorldToViewportPoint(targetRenderer.bounds.center);

            // X と Y が 0.0 〜 1.0 の範囲内にあるか判定
            bool inX = viewportPos.x >= 0.0f && viewportPos.x <= 1.0f;
            bool inY = viewportPos.y >= 0.0f && viewportPos.y <= 1.0f;

            return inX && inY;
        }
    }
}
