using System;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{

    public AbilityType Type;
    public int Value;
    public int Remaining;

    public StatusEffect(AbilityType type, int value, int remaining)
    {
        Type = type;
        Value = value;
        Remaining = remaining;
    }

    public string Label
    {
        get
        {
            switch (Type)
            {
                case AbilityType.Burn: return "Burning";
                case AbilityType.Poison: return "Poisoned";
                case AbilityType.Thorns: return "Thorns";
                default: return Type.ToString();
            }
        }
    }

    public string ColorHex
    {
        get
        {
            switch (Type)
            {
                case AbilityType.Burn: return "FF6A2B";
                case AbilityType.Poison: return "7ED957";
                case AbilityType.Thorns: return "C8C8C8";
                default: return "FFFFFF";
            }
        }
    }
}
