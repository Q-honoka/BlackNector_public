using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using static UnityEngine.InputManagerEntry;

public enum ViewType
{
    None,
    Typing,
    Fadein,
    FadeinTyping,
    MAX
}

/*
 * ノベルシステムのイベント取得者
 */
public class NovelTextView : MonoBehaviour
{
    private NovelSubject novelSubject;
    private static readonly float typeSpeed = 0.1f;  // タイピングスピード
    private static readonly float faypeSpeed = 1.0f;  // タイピングスピード
    private static readonly float skipCoolTime = 0.5f;   // タイピングを開始してスキップ可能になるまでのクールタイム
    private static readonly string initText = "";    // 初期文字
    private static readonly int click = 0;   // デバッグ用(マウスボタン左)

    [SerializeField] NOVEL_CHAR present_character;  // このコンポーネントで表示するテキストに設定したキャラクター
    [SerializeField] NOVEL_KIND present_kind;   // このコンポーネントで表示するチュートリアルの種類
    [SerializeField] ViewType viewType = ViewType.Fadein;
    [SerializeField] TextMeshProUGUI ugui;
    [SerializeField, Range(0, 1)] float addAlpha = 0.05f;


    private void Start()
    {
        if(NovelSubject.Instance != null)
        {
            novelSubject = NovelSubject.Instance;
        }
        else
        {
            Debug.Log("小川：NovelSubjectが存在しません", gameObject);
        }

        ugui.enabled = false;

        novelSubject.OnPlayNovel
            .Subscribe(kind => { 
                ugui.text = initText;
                ugui.enabled = true;//(kind == present_kind)
                //Debug.Log($"小川：{kind == present_kind}に変えた", gameObject);
            });

        novelSubject.OnNextNovel
            .Where(asset => asset.charcter == present_character)
            .Subscribe(asset => { 
                View(asset);
                //Debug.Log($"小川：テキストが進んだ：{asset.text}", gameObject);
            });

        novelSubject.OnFinishedNovel
            .Subscribe(kind => {
                Close().Forget();
                //Debug.Log($"小川：テキストが終わった", gameObject);
            });
    }

    private void View(NovelAsset asset)
    {
        switch(viewType)
        {
            case ViewType.Typing:
                TypingText(asset).Forget();
                break;
            case ViewType.Fadein:
                FadeinText(asset).Forget();
                break;
            case ViewType.FadeinTyping:
                FadeinTypingText(asset).Forget();
                break;
        }
    }

    private async UniTaskVoid Close()
    {
        if(novelSubject.IsLockedAutoPlay)
        {
            await UniTask.WaitForSeconds(novelSubject.lockedAutoSpeed);
        }
        ugui.enabled = false;
    }

    // テキストをタイピングのように表示する
    private async UniTaskVoid TypingText(NovelAsset asset)
    {
        int textSize = asset.text.Length;
        float typeStart = Time.time;
        string text = initText;

        for (int t = 0; t < textSize; t++)
        {
            bool isClick = Input.GetMouseButtonDown(click);             //スキップクリックしたか
            bool isFinishedCT = Time.time - typeStart >= skipCoolTime;  // スキップできるまでのクールタイムが経過したか
            bool isLockedAutoPlay = novelSubject.IsLockedAutoPlay;  // ロックするか

            if (isClick && isFinishedCT && !isLockedAutoPlay)
            {
                CompleteText(asset);
                break;
            }

            text += asset.text[t];
            ugui.text = text;
            await UniTask.WaitForSeconds(typeSpeed);
        }
    }

    // フェードイン
    private async UniTaskVoid FadeinText(NovelAsset asset)
    {
        string text = initText;

        // テキストを透明で挿入
        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 0);
        ugui.text = asset.text;

        while(ugui.color.a < 1)
        {
            ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, ugui.color.a + addAlpha); // 透明度加算
            await UniTask.WaitForFixedUpdate();
        }

        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 1);    // 透明度調整
    }

    private async UniTaskVoid FadeinTypingText(NovelAsset asset)
    {
        string text = initText;

        // テキストを透明で挿入
        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 0);
        ugui.text = asset.text;

        while (ugui.color.a < 1)
        {
            ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, ugui.color.a + addAlpha); // 透明度加算
            await UniTask.WaitForFixedUpdate();
        }

        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 1);    // 透明度調整
    }

    // テキストを即座に適用させる
    private void CompleteText(NovelAsset asset)
    {
        ugui.text = asset.text;
    }
}
