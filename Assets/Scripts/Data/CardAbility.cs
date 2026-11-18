using NUnit.Framework.Constraints;
using UnityEngine;

public enum AbilityType { None, Burn, Poison, Thorns }

[CreateAssetMenu(fileName = "CardAbility", menuName = "Scriptable Objects/CardAbility")]
public class CardAbility : ScriptableObject
{
    public string abilityName;
    public int cost;
    public string description;

    public AbilityType type;
    public int value = 1;
    public int duration = 2;

    public bool TargetAllies => type == AbilityType.Thorns;

}
