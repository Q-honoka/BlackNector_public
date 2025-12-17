using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using static UnityEngine.InputManagerEntry;
using System;

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
    private static readonly float skipCoolTime = 0.5f;   // タイピングを開始してスキップ可能になるまでのクールタイム
    private static readonly string initText = "";    // 初期文字
    private static readonly int click = 0;   // デバッグ用(マウスボタン左)

    [SerializeField] NOVEL_CHAR present_character;  // このコンポーネントで表示するテキストに設定したキャラクター
    //[SerializeField] NOVEL_KIND present_kind = NOVEL_KIND.MAX;   // このコンポーネントで表示するチュートリアルの種類(使用やめた)
    [SerializeField] ViewType viewType = ViewType.Fadein;
    [SerializeField] TextMeshProUGUI ugui;

    [Header("タイピング")]
    [SerializeField] float typeSpeed = 0.1f;
    [Header("フェードイン")]
    [SerializeField, Range(0, 1), Tooltip("秒間透明度増加率")] float addAlpha = 0.05f;
    [Header("フェードイン＆タイピング")]
    [SerializeField, Header("文字の表示間隔")] float charInterval = 0.001f;
    [SerializeField, Header("透明度が変化する時間")] float fadeTime = 0.02f;


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

    // タイピング＆フェードイン
    private async UniTaskVoid FadeinTypingText(NovelAsset asset)
    {
        ugui.text = asset.text;
        ugui.ForceMeshUpdate();

        var textInfo = ugui.textInfo;
        int textCount = textInfo.characterCount;

        // すべて透明
        for(int i = 0; i < textCount;i++)
        {
            SetCharAlpha(textInfo, i, 0);
        }

        ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);  // 頂点更新

        // 一文字ずつフェードイン
        for (int i = 0; i < textCount; i++)
        {
            FadeChar(textInfo, i).Forget();
            await UniTask.WaitForSeconds(charInterval);
        }
    }

    // 文字のフェードアウト
    private async UniTask FadeChar(TMP_TextInfo textInfo, int index)
    {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
        if (!charInfo.isVisible) return;

        int matIndex = charInfo.materialReferenceIndex;
        int vertIndex = charInfo.vertexIndex;

        Color32[] colors = textInfo.meshInfo[matIndex].colors32;

        byte alpha = 0;
        while (alpha < 255)
        {
            alpha += (byte)(addAlpha * sizeof(byte));

            for (int i = 0; i < 4; i++)
            {
                colors[vertIndex + i].a = alpha;
            }

            ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            await UniTask.WaitForFixedUpdate();
        }
    }

    // １文字の透明度を変更（text.colorとは別なので注意）
    private void SetCharAlpha(TMP_TextInfo textInfo, int index, byte alpha)
    {
        var charInfo = textInfo.characterInfo[index];
        if (!charInfo.isVisible) return;

        int matIndex = charInfo.materialReferenceIndex;
        int vertIndex = charInfo.vertexIndex;

        Color32[] colors = textInfo.meshInfo[matIndex].colors32;
        for (int i = 0; i < 4; i++)
        {
            colors[vertIndex + i].a = alpha;
        }

        return;
    }

    // テキストを即座に適用させる
    private void CompleteText(NovelAsset asset)
    {
        ugui.text = asset.text;
    }
}
