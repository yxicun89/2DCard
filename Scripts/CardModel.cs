using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardModel : MonoBehaviour
{
    public int cardid;
    public string cardName;
    public int cardCost;
    public int cardAttack;
    public Sprite cardIcon;
    public bool canUse = false;
    public bool PlayerCard = false;
    public bool FieldCard = false;
    public bool canAttack = false;

    public CardModel(int cardID,bool playerCard){
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        cardid = cardEntity.cardid;
        cardName = cardEntity.cardName;
        cardCost = cardEntity.cardCost;
        cardAttack = cardEntity.cardAttack;
        cardIcon = cardEntity.cardIcon;

        PlayerCard = playerCard;
    }
}
