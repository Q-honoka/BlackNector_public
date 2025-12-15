using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using static UnityEngine.InputManagerEntry;

/*
 * ノベルシステムのイベント取得者
 */
public class NovelTextView : MonoBehaviour
{
    private NovelSubject novelSubject;
    private static readonly float typeSpeed = 0.1f;  // タイピングスピード
    private static readonly float skipCoolTime = 0.5f;   // タイピングを開始してスキップ可能になるまでのクールタイム
    private static readonly string initText = "";    // 初期文字
    private static readonly int click = 0;   // デバッグ用(マウスボタン左)

    [SerializeField] NOVEL_CHAR present_character;  // このコンポーネントで表示するテキストに設定したキャラクター
    [SerializeField] NOVEL_KIND present_kind;   // このコンポーネントで表示するチュートリアルの種類
    [SerializeField] TextMeshProUGUI ugui;


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
                ugui.enabled = (kind == present_kind);
                Debug.Log($"小川：{kind == present_kind}に変えた", gameObject);
            });

        novelSubject.OnNextNovel
            .Where(asset => asset.charcter == present_character)
            .Subscribe(asset => { 
                View(asset);
                Debug.Log($"小川：テキストが進んだ", gameObject);
            });

        novelSubject.OnFinishedNovel
            .Subscribe(kind => { 
                ugui.enabled = false;
                Debug.Log($"小川：テキストが終わった", gameObject);
            });
    }

    private void View(NovelAsset asset)
    {
        TypeText(asset).Forget();
    }

    // テキストをタイピングのように表示する
    private async UniTaskVoid TypeText(NovelAsset asset)
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
        Debug.Log($"小川：タイピングを終えた{asset.text}", gameObject);
    }

    // テキストを即座に適用させる
    private void CompleteText(NovelAsset asset)
    {
        string text = asset.text;
    }
}
