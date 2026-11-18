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
