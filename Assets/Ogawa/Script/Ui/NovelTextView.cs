using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using static UnityEngine.InputManagerEntry;
using System;
using System.Threading;
using System.Globalization;

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

    private CancellationToken token;

    [SerializeField] NOVEL_CHAR present_character;  // このコンポーネントで表示するテキストに設定したキャラクター
    //[SerializeField] NOVEL_KIND present_kind = NOVEL_KIND.MAX;   // このコンポーネントで表示するチュートリアルの種類(使用やめた)
    [SerializeField] ViewType viewType = ViewType.Fadein;
    [SerializeField] TextMeshProUGUI ugui;

    [Header("タイピング")]
    [SerializeField] float typeSpeed = 0.1f;
    [Header("フェードイン")]
    [SerializeField, Tooltip("秒間不透明度増加率(1で不透明度100%)")] float addAlpha_Fadein_Sec = 0.05f;   // n秒で不透明度100になるかにすればよかった
    [Header("フェードイン＆タイピング")]
    [SerializeField, Header("文字の表示間隔")] float charInterval = 0.05f;
    [SerializeField, Range(0, 1024), Tooltip("秒間不透明度増加率(255で不透明度100%)")] byte addAlpha_FadeinType_Sec = 10; // n秒で不透明度100になるかにすればよかった


    private void Start()
    {
        if(NovelSubject.instance != null)
        {
            novelSubject = NovelSubject.instance;
        }
        else
        {
            Debug.Log("小川：NovelSubjectが存在しません", gameObject);
        }

        token = this.GetCancellationTokenOnDestroy();

        ugui.text = initText;
        ugui.enabled = false;

        novelSubject.OnPlayNovel
            .Subscribe(kind =>
            {
                ugui.text = initText;
                ugui.enabled = true;
            })
            .AddTo(this);

        novelSubject.OnNextNovel
            .Where(asset => asset.charcter == present_character)
            .Subscribe(asset => { 
                View(asset);
            })
            .AddTo(this);

        novelSubject.OnFinishedNovel
            .Subscribe(kind => {
                Close().Forget();
            })
            .AddTo(this);
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
            await UniTask.WaitForSeconds(novelSubject.lockedAutoSpeed, cancellationToken: token);
        }
        ugui.enabled = false;
    }

    // タイピング
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
            await UniTask.WaitForSeconds(typeSpeed, cancellationToken: token);
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
            ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, ugui.color.a + addAlpha_Fadein_Sec); // 透明度加算
            await UniTask.Yield(cancellationToken: token);
        }

        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 1);    // 透明度調整
    }

    // タイピング＆フェードイン
    private async UniTaskVoid FadeinTypingText(NovelAsset asset)
    {
        float uguiAlpha = ugui.color.a;
        ugui.color = new Color(ugui.color.r, ugui.color.g, ugui.color.b, 0);
        ugui.text = asset.text;
        ugui.ForceMeshUpdate();

        var textInfo = ugui.textInfo;
        int textCount = textInfo.characterCount;

        // テキストのメッシュアルファを０にする
        for (int i = 0; i < textCount; i++)
        {
            await ChangeCharacterAlpha(textInfo, i, 0);
        }

        ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        // テキストのメッシュアルファを一文字ずつフェードインする
        for (int i = 0; i < textCount; i++)
        {
            ChangeCharacterAlphaFadein(textInfo, i).Forget();
            await UniTask.WaitForSeconds(charInterval, cancellationToken: token);
        }
        ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    // 文字の透明度を変える
    private UniTask ChangeCharacterAlpha(TMP_TextInfo textInfo,int index,byte alpha)
    {
        var charInfo = textInfo.characterInfo[index];
        Color32[] colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
        int vertIndex = charInfo.vertexIndex;

        for (int c = 0; c < 4; c++)
        {
            colors[vertIndex + c].a = alpha;
        }

        ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        return UniTask.CompletedTask;
    }

    // 単体文字透明度をフェードインしながら変える
    private async UniTask ChangeCharacterAlphaFadein(TMP_TextInfo textInfo, int index)
    {
        float elapseld = 0;
        float movetime = (float)(byte.MaxValue / addAlpha_FadeinType_Sec);

        while (elapseld < movetime)
        {
            elapseld += Time.deltaTime;
            byte alpha = (byte)Mathf.Min((float)((float)(elapseld / movetime) * byte.MaxValue), (float)byte.MaxValue);

            await ChangeCharacterAlpha(textInfo, index, alpha);

            await UniTask.Yield(cancellationToken: token);
        }
        ugui.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    // テキストを即座に適用させる
    private void CompleteText(NovelAsset asset)
    {
        ugui.text = asset.text;
    }
}
