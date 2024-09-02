using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public int cardid;
    public string cardName;
    public int cardCost;
    public int cardAttack;
    public Sprite cardIcon;
}
