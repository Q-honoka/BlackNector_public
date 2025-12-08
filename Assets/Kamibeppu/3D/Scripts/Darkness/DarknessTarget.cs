using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float destroyTime = 3f;        // 消滅までの時間
    [SerializeField] private GameObject nestorPrefab;       // ネスターのPrefab
    [SerializeField] private float initializeRadius = 3f;   // ネスターと対象の距離（半径）
    [SerializeField] private int spawnedNestorCount = 10;  // ネスターの生成個数

    private DarknessSensor darknessSensor;
    private DarknessEntityTracker tracker;
    private DarknessTarget target;
    private List<GameObject> nestors = new List<GameObject>();  // 生成されたネスターのリスト
    private List<float> angles = new List<float>();             // 各ネスターの生成角度
    private float angleStep;                // 生成する角度の間隔
    private Vector3 centerPosition;
    private float elapsedTime = 0f;         // 経過時間
    private float radius = 0f;
    private float decreaseRadius;           // 減らす半径

    private void Start()
    {
        // センサーのインスタンス取得
        darknessSensor = DarknessSensor.Instance;

        if (darknessSensor == null)
        {
            Debug.Log("センサースクリプトのインスタンス化に失敗");
        }

        tracker = DarknessEntityTracker.Instance;
        target = this.GetComponent<DarknessTarget>();

        radius = initializeRadius;
        angleStep = 360f / spawnedNestorCount;      // 生成する角度の間隔

        // 半径の減少速度を求める [ (初期半径 - 半径の下限) / 消滅までの時間 ]
        decreaseRadius = (initializeRadius - 1) / destroyTime;
    }

    private void Update()
    {
        //bool isInDarkness = false;

        //// センサーに自分自身を渡して暗闇にいるか取得する
        //if (darknessSensor != null)
        //{
        //    isInDarkness = darknessSensor.GetSelfIsDarkness(this);
        //}

        //// 暗闇にいたら処理をする
        //if (isInDarkness == true)
        //{
        //    DarknessAction();
        //}
        //else
        //{
        //    ResetDarknessAnimation();
        //}
    }

    // 暗闇に入ったときの演出
    public void DarknessAction()
    {
        // 経過時間が消滅までの時間を越していないならアニメーションを再生する
        if (elapsedTime <= destroyTime)
        {
            DarknessAnimation();
        }
        else
        {
            Destroy(this.gameObject);
        }

        elapsedTime += Time.deltaTime;
    }

    // ネスターに襲われるアニメーション
    private void DarknessAnimation()
    {
        centerPosition = this.transform.position;

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
        for (float angle = 0f; angle <= 360f; angle += angleStep)
        {
            // 上方向を基準にするため角度から -90度する
            float radAngle = (angle - 90f) * Mathf.Deg2Rad;

            // 三角関数を用いて生成位置を求める
            float spawnX = centerPosition.x + Mathf.Cos(radAngle) * radius;
            float spawnY = centerPosition.y + Mathf.Sin(radAngle) * radius;
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, centerPosition.z);

            // 逆三角関数を用いて生成角度を求める
            Vector3 direction = centerPosition - spawnPosition;      // 対象との距離
            float angleToTarget = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;    // 回転角度
            Quaternion spawnRotation = Quaternion.Euler(0, 0, angleToTarget);

            // ネスター生成
            GameObject spawned = Instantiate(nestorPrefab, spawnPosition, spawnRotation);
            spawned.transform.parent = this.transform;
            nestors.Add(spawned);
            angles.Add(radAngle);
        }
    }

    // ネスターの移動処理
    private void MoveNestors()
    {
        // 移動先の座標を求める
        for (int i = 0; i < nestors.Count; i++)
        {
            float moveX = centerPosition.x + Mathf.Cos(angles[i]) * radius;
            float moveY = centerPosition.y + Mathf.Sin(angles[i]) * radius;
            nestors[i].transform.position = new Vector3(moveX, moveY, nestors[i].transform.position.z);
        }

        radius -= decreaseRadius * Time.deltaTime;
        radius = Mathf.Max(radius, 1f);         // 半径が1を下回らないように制御

        // ネスターが表示されていなければ表示する
        foreach (var nestor in nestors)
        {
            if (nestor.activeSelf == false)
            {
                nestor.SetActive(true);
            }
        }
    }

    // アニメーションのリセット
    private void ResetDarknessAnimation()
    {
        // それまで出現していたネスターを非表示にする
        foreach (var nestor in nestors)
        {
            nestor.SetActive(false);
        }
        elapsedTime = 0f;
        radius = initializeRadius;
    }

    // 自身を消去したらセンサーのリストからも削除する
    private void OnDestroy()
    {
        NotifyLightStateChanged();
    }

    // エンティティの状態が変化したことを通知する
    private void NotifyLightStateChanged()
    {
        if (tracker != null)
        {
            tracker?.UpdateActiveEntities(target);
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

}