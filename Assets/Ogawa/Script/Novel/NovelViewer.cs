using UnityEngine;

/*
 * ノベルマネージャーからノベルの情報を取得できます。
 */

public class NovelViewer : MonoBehaviour
{
    [SerializeField,Tooltip("イベントを取得するキャラクター")] protected NOVEL_CHAR[] injectNovelCharacter;
    [SerializeField,Tooltip("イベントを取得するノベル")] protected NOVEL_KIND[] injectNovelKind;


}
