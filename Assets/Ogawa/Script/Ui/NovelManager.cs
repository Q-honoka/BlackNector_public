using UnityEngine;
using UniRx;
using System;

public enum NOVEL_KIND
{
    Tutorial_1_1,
    Tutorial_1_2,
    MAX
}

/*
 * ノベルシステムの管理者
 * できるようになる予定の内容
 * ①外部から指定のキャライベントをサブスクライブしてアクションを取得
 * →テキストの発信、アイコンの変更命令、テキストボックスの表示非表示など
 * ②外部（今度作る）から指定したノベルデータの再生
 */

public class NovelManager : MonoBehaviour
{
    [SerializeField] NovelInfo[] novelData = new NovelInfo[(int)NOVEL_KIND.MAX];    // ノベルデータ

    private NOVEL_KIND playingNovel; // 現在再生中のノベル
    private int currentAssetsID;     // 次に再生するノベルアセットID

    /*ノベル再生発火用*/
    /***************************/
    private Subject<NOVEL_KIND> novelKindSubject = new Subject<NOVEL_KIND>();
    private Subject<int> novelAssetsSubject = new Subject<int>();
    private Subject<Unit> novelFinishedSubject = new Subject<Unit>();

    // イベントの購読側だけを公開

    // 再生するノベルのデータ
    public IObservable<NOVEL_KIND> OnPlayNovel
    {
        get { return novelKindSubject; }
    }

    // 再生するノベルのテキスト
    public IObservable<int> OnNextNovel
    {
        get { return novelAssetsSubject; }
    }

    // ノベルが終了した
    public IObservable<Unit> OnFinishedNovel
    {
        get { return novelFinishedSubject; }
    }
    /**************************/
    
    // ノベルの再生を開始する
    public void Play(NOVEL_KIND playNovel)
    {
        playingNovel = playNovel;
        currentAssetsID = 0;
        novelKindSubject.OnNext(playingNovel);
        novelAssetsSubject.OnNext(currentAssetsID);
    }

    // ノベルの再生を終了する
    public void Stop()
    {
        novelFinishedSubject.OnNext(Unit.Default);
    }
}
