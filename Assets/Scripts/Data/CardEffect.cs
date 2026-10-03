using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "Scriptable Objects/CardEffect")]
public class CardEffect : ScriptableObject
{
    public string description;
    public int value;
    public int duration;
    public CardEffectScript effectScript;
}
