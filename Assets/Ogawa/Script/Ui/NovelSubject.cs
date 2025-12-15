using UnityEngine;
using UniRx;
using System;
using static UnityEngine.InputManagerEntry;
using Cysharp.Threading.Tasks;

public enum NOVEL_KIND  // ノベルパートの種類
{
    Debug_1_1,
    Debug_1_2,
    Tutorial_1_1,
    Tutorial_2_1,
    Event_1_1,
    Event_1_2,
    MAX
}

/*
 * ノベルシステムの管理者
 * できるようになる予定の内容
 * ①外部から指定のキャライベントをサブスクライブしてアクションを取得
 * →テキストの発信、アイコンの変更命令、テキストボックスの表示非表示など
 * ②外部（今度作る）から指定したノベルデータの再生
 */

public class NovelSubject : MonoBehaviour
{
    public static NovelSubject Instance;    // シングルトン

    [SerializeField] NovelInfo[] novelData = new NovelInfo[(int)NOVEL_KIND.MAX];    // ノベルデータ

    private NOVEL_KIND playingNovel; // 現在再生中のノベル
    private int currentAssetsID;     // 次に再生するノベルアセットID

    /*ノベル再生発火用*/
    private Subject<NOVEL_KIND> novelKindSubject = new Subject<NOVEL_KIND>();
    private Subject<NovelAsset> novelAssetsSubject = new Subject<NovelAsset>();
    private Subject<Unit> novelFinishedSubject = new Subject<Unit>();
    private Subject<bool> autoChangeSubject = new Subject<bool>();

    // 操作不可オート再生するか
    private bool _IsLockedAutoPlay = false;
    public bool IsLockedAutoPlay 
    {
        get
        {
            return _IsLockedAutoPlay;
        }
        set // 状態切り替え時のみイベント発火
        {
            if(value!=_IsLockedAutoPlay)
            {
                autoChangeSubject.OnCompleted();
                _IsLockedAutoPlay = value;
            }
        }
    }
    // 操作不可オート再生速度（少なすぎると正常に動作しない可能性）
    private readonly float lockedAutoSpeed = 1.0f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnStart()
    {
        Play(NOVEL_KIND.Event_1_1).Forget();  // デバッグ用
    }


    /***************************/
    // イベントの購読側だけを公開

    // 再生するノベルのデータ
    public IObservable<NOVEL_KIND> OnPlayNovel
    {
        get {
            Debug.Log($"小川：{novelKindSubject}だった", gameObject);
            return novelKindSubject; 
        }
    }

    // 再生するノベルのテキスト
    public IObservable<NovelAsset> OnNextNovel
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
    public async UniTask Play(NOVEL_KIND playNovel)
    {
        playingNovel = playNovel;
        currentAssetsID = 0;
        novelKindSubject.OnNext(playingNovel);
        IsLockedAutoPlay = novelData[(int)playingNovel];


        if (IsLockedAutoPlay)
        {
            for(int i = 0; i < novelData[(int)playingNovel].novel.Length; i++)
            {
                novelAssetsSubject.OnNext(novelData[(int)playingNovel].novel[currentAssetsID]);
                Debug.Log($"小川：読んだ({novelData[(int)playingNovel].novel[currentAssetsID].text})");
                await UniTask.WaitForSeconds(lockedAutoSpeed);  // ここ直す！！currentAssetsID++;されてない
            }
        }
        else
        {
            novelAssetsSubject.OnNext(novelData[(int)playingNovel].novel[currentAssetsID]);
        }
    }

    // ノベルを進める
    public void Next()
    {
        if(currentAssetsID + 1 >= novelData[(int)playingNovel].novel.Length)
        {
            Stop();
            return;
        }

        currentAssetsID++;
        novelAssetsSubject.OnNext(novelData[(int)playingNovel].novel[currentAssetsID]);
    }

    // ノベルの再生を終了する
    public void Stop()
    {
        novelFinishedSubject.OnNext(Unit.Default);
    }
}
