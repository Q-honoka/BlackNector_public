using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public enum NOVEL_CHAR  // ノベルに登場するキャラクター
{
    MainCharacter,
    Bird,
    Scarecrow,
    Marionette,
    MAX
}

[Serializable]
public struct NovelAsset
{
    public NOVEL_CHAR charcter;
    public string text;
}

[CreateAssetMenu(fileName = nameof(NovelInfo), menuName = nameof(ScriptableObject) + "/UI/" + nameof(NovelInfo))]
public class NovelInfo : ScriptableObject
{
    public bool isLockedAutoPlay = false;
   public NovelAsset[] novel;
}
