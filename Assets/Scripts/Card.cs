using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardgleGame
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public Rarity rarity;
        public CardType cardType;
        public int health;
        public int damage;
        public int healthAttack;
        public int speed;
        public DamageType damageType;

        public enum CardType
        {
            Melee,
            Range,
            Healer
        }

        public enum DamageType
        {
            Melee,
            Range,
            Healer,

        }
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary
        }
    }
}