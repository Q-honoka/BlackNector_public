using UnityEngine;
using UnityEngine.Playables;

public class GameOverManager : MonoBehaviour
{
    [Header("敵の発見タイムライン (要素0:かかし, 1:マリオネット, 2:ブリキ人形)")]
    public PlayableDirector[] enemyTimelines;

    [Header("暗闇のタイムライン")]
    public PlayableDirector darknessTimeline;

    [Header("終了で表示する一枚絵")]
    public GameObject EndImage;

    private bool isEnd = false;

    private void Update()
    {
        if(isEnd)
        {
            if(Input.anyKeyDown)
            {
                isEnd = false;
                EndImage.SetActive(false);
                SceneController.instance.SceneChange(0);
            }
        }
    }

    // 敵に見つかった演出
    public void PlayEnemyGameOver(int enemyType, Animator spottedEnemyAnimator)
    {
        var director = enemyTimelines[enemyType];

        foreach (var output in director.playableAsset.outputs)
        {
            if (output.streamName == "EnemyTrack")
            {
                director.SetGenericBinding(output.sourceObject, spottedEnemyAnimator);
            }
        }

        director.Play();
    }

    // 暗闇に入った演出
    public void PlayDarknessGameOver()
    {
        darknessTimeline.Play();
    }

    // タイムラインを止める
    public void StopDarknessGameOver()
    {
        StopAllCoroutines();
        darknessTimeline.Stop();
        darknessTimeline.time = 0;
        darknessTimeline.Evaluate();
    }

    // リトライ
    public void RetrySignal()
    {
        Retry.instance.CallRetry();
    }


    // エンド画面の表示
    public void ShowEnd()
    {
        EndImage.SetActive(true);
        isEnd = true;
    }
}