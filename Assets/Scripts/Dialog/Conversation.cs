using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Line
{
    public CharacterDialog character;

    [TextArea(2,5)]
    public string text;
}


[CreateAssetMenu(fileName = "Conversation", menuName = "Catarsis/Conversation", order = 0)]
public class Conversation : ScriptableObject {
    public CharacterDialog speakerLeft;
    public CharacterDialog speakerRight;
    public Line[] lines;
}