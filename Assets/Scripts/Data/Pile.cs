using UnityEngine;
using System.Collections.Generic;
using CardgleGame;

[CreateAssetMenu(fileName = "Pile", menuName = "Scriptable Objects/Pile")]
public class Pile : ScriptableObject
{
    public string pileType;
    public List<Card> cards;
    public int cardCount => cards.Count;
}