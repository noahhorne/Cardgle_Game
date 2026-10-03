using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "Scriptable Objects/CardAbility")]
public class CardAbility : ScriptableObject
{
    public string abilityName;
    public int cost;
    public string description;

}
