using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();
        CardController defenceCard = GetComponent<CardController>();
        GameManager.instance.CardBattle(attackCard, defenceCard);
    }
}