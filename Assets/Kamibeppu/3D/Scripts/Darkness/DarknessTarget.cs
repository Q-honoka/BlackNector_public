using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * 暗闇判定をしたいエンティティには
 * 必ずこのスクリプトをアタッチしてください。
 * 
 * 暗闇に入ったときにネスターに襲われる処理を記述しています。
 * 
 */

public class DarknessTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;                    // オブジェクトを表示するカメラ
    [SerializeField] private RectTransform canvas;          // ネスターUIを表示するCanvas
    [SerializeField] private float destroyTime = 3f;        // 消滅までの時間
    [SerializeField] private Image nestorPrefab;            // ネスターのPrefab
    [SerializeField] private float initializeRadius = 100f;   // ネスターと対象の距離（半径）
    [SerializeField] private int spawnedNestorCount = 10;  // ネスターの生成個数

    private DarknessSensor darknessSensor;
    private DarknessEntityTracker tracker;
    private List<Image> nestors = new List<Image>();  // 生成されたネスターのリスト
    private List<float> angles = new List<float>();             // 各ネスターの生成角度
    private float angleStep;                // 生成する角度の間隔
    private float elapsedTime = 0f;         // 経過時間
    private float radius = 0f;
    private float decreaseRadius;           // 減らす半径
    private GameObject nestorContainer;     // ネスターUIを管理する親オブジェクト
    private bool isInsideCamera;            // カメラ内にいるかどうか

    private void Start()
    {
        // センサーのインスタンス取得
        darknessSensor = DarknessSensor.Instance;
        tracker = DarknessEntityTracker.Instance;

        angleStep = 360f / spawnedNestorCount;      // 生成する角度の間隔
        radius = initializeRadius;

        // 半径の減少速度を求める [ (初期半径 - 半径の下限) / 消滅までの時間 ]
        decreaseRadius = (initializeRadius - 20f) / destroyTime;

        if(cam == null) cam = Camera.main;
    }

    private void Update()
    {
        // カメラの範囲内 かつ 暗闇にいる場合はアニメーションをする
        bool isInDark = darknessSensor.GetSelfIsDarkness(this);
        if (isInsideCamera == true && isInDark == true)
        {
            DarknessAction();
        }
        else
        {
            ResetDarknessAnimation();
        }
    }

    // 暗闇に入ったときの演出
    public void DarknessAction()
    {
        elapsedTime += Time.deltaTime;
        
        // 経過時間が消滅までの時間を越していないならアニメーションを再生する
        if (elapsedTime <= destroyTime)
        {
            DarknessAnimation();
        }
        else
        {
            if(nestorContainer != null) Destroy(nestorContainer);
            Destroy(this.gameObject);
        }
    }

    // ネスターに襲われるアニメーション
    private void DarknessAnimation()
    {
        if (nestors.Count == 0)
        {
            // ネスターを生成
            SpawnNestors();
        }
        else
        {
            // ネスターを移動させる
            MoveNestors();
        }
    }

    // ネスターの生成処理
    private void SpawnNestors()
    {
        // ネスターUIを入れるコンテナオブジェクトを生成
        if(nestorContainer != null) Destroy(nestorContainer);

        nestorContainer = new GameObject($"{this.gameObject.name}_nestors");
        nestorContainer.transform.SetParent(canvas, false);

        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        // ネスターを生成する
        for (int i = 0; i < spawnedNestorCount; i++)
        {
            float degAngle = i * angleStep;
            // 上方向を基準にするため角度から -90度する
            float radAngle = (degAngle - 90f) * Mathf.Deg2Rad;

            // ネスター生成
            Image spawned = Instantiate(nestorPrefab, nestorContainer.transform);
            SetNestor(spawned, screenPos, radAngle, radius);

            nestors.Add(spawned);
            angles.Add(radAngle);
        }
    }

    // ネスターの移動処理
    private void MoveNestors()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(this.transform.position);

        radius -= decreaseRadius * Time.deltaTime;
        radius = Mathf.Max(radius, 1f);         // 半径が1を下回らないように制御

        for (int i = 0; i < nestors.Count;i++)
        {
            if(nestors[i] == null) continue;

            if (nestors[i].enabled == false) nestors[i].enabled = true;
            SetNestor(nestors[i], screenPos, angles[i], radius);
        }

        // ネスターが表示されていなければ表示する
        foreach (var nestor in nestors)
        {
            if (nestor.enabled == false)
            {
                nestor.enabled = true;
            }
        }
    }

    // ネスターの位置と回転設定
    private void SetNestor(Image nestor, Vector3 center, float rad, float radDirection)
    {
        // 三角関数を使って円形に配置
        float x = center.x + Mathf.Cos(rad) * radius;
        float y = center.y + Mathf.Sin(rad) * radius;

        nestor.transform.position = new Vector3(x, y, 10f);
        nestor.transform.localScale = Vector3.one;

        // 逆関数を使ってターゲットの方向を向く
        Vector3 diff = center - nestor.transform.position;
        float angleToTarget = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
        nestor.transform.rotation = Quaternion.Euler(0, 0, angleToTarget);
    }

    // アニメーションのリセット
    private void ResetDarknessAnimation()
    {
        // それまで出現していたネスターを非表示にする
        foreach (var nestor in nestors)
        {
            if(nestor != null) nestor.enabled = false;
        }
        elapsedTime = 0f;
        radius = initializeRadius;
    }

    // 自身を消去したらセンサーのリストからも削除する
    private void OnDestroy()
    {
        if(nestorContainer != null) Destroy(nestorContainer);
        NotifyLightStateChanged();
    }

    // エンティティの状態が変化したことを通知する
    private void NotifyLightStateChanged()
    {
        if (tracker != null)
        {
            tracker?.UpdateActiveEntities(this);
        }
    }

    // エンティティが無効になったときにトラッカーに通知
    private void OnDisable()
    {
        NotifyLightStateChanged();
    }

    // エンティティが有効になったときにトラッカーに通知
    private void OnEnable()
    {
        NotifyLightStateChanged();
    }

    // カメラから外れた瞬間にフラグを false にする
    private void OnBecameInvisible()
    {
        isInsideCamera = false;
    }

    // カメラ内に入った瞬間にフラグを true にする
    private void OnBecameVisible()
    {
        isInsideCamera = true;
    }
}