using TMPro;
using UnityEngine;

public enum TextCharacter
{
    MainCharacter,
    Bird,
    Scarecrow,
    Marionet,
    MAX
}

[CreateAssetMenu(fileName = "TextInfo", menuName = "Scriptable Objects/UI/TextInfo")]
public class TextInfo : ScriptableObject
{
    public TextMeshProUGUI[] text;
}
