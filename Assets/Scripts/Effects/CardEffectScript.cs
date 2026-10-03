using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectScript", menuName = "Scriptable Objects/CardEffectScript")]

    public abstract class CardEffectScript : ScriptableObject
    {
        public abstract void ApplyEffect(
            CardEffect effect
            // CardObject target **REPLACE**
        );
    }

