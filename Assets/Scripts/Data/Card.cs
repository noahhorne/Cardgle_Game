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
        public Sprite cardArt;
        public int health;
        public int damage;
        public int healthAttack;
        public int speed;
        public int cost;
        public CardAbility activeAbility;
        public CardAbility passiveAbility;            

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