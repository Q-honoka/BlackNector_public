using RaruLib;
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
    private Sound _sound => Sound.instance;
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

    /// <summary>
    /// 敵に見つかった演出
    /// </summary>
    /// <param name="enemyType">敵の種類</param>
    /// <param name="spottedEnemyAnimator">再生するアニメーション</param>
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

    /// <summary>
    /// 暗闇に入った演出
    /// </summary>
    public void PlayDarknessGameOver()
    {
        darknessTimeline.Play();
    }

    /// <summary>
    /// タイムラインを止める
    /// </summary>
    public void StopDarknessGameOver()
    {
        StopAllCoroutines();
        darknessTimeline.Stop();
        darknessTimeline.time = 0;
        darknessTimeline.Evaluate();
        _sound.Stop("SE", "Badfeeling");
    }

    /// <summary>
    /// リトライ
    /// </summary>
    public void RetrySignal()
    {
        Retry.instance.CallRetry();
    }
     
    // SEPlay Nestor_hand01
    public void Nestorbig_SE()
    {
        _sound.Play("SE", "Nestor_hand01");
    }

    // SEPlay Badfeeling
    public void Nestor_Badfeeling_SE()
    {
        _sound.Play("SE", "Badfeeling");
    }

    // SEPlay Rumble
    public void Rumble_SE()
    {
        _sound.Play("SE", "Rumble");
    }

    // SEPlay Boone
    public void Boone_SE()
    {
        _sound.Play("SE", "Boone");
    }

    /// <summary>
    /// エンド画面の表示
    /// </summary>
    public void ShowEnd()
    {
        EndImage.SetActive(true);
        isEnd = true;
    }
}