using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "Scriptable Objects/CardAbility")]
public class CardAbility : ScriptableObject
{
    public string abilityName;
    public int value;
    public int duration;
    public string description;

}
