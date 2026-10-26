using UnityEngine;

    public abstract class CardEffectScript : ScriptableObject
    {
        public abstract void ApplyEffect(
            CardEffect effect,
            CardObject target
        );
    }

