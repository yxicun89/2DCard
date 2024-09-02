using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackLeader : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();
        GameManager.instance.AttackToLeader(attackCard, true);
    }
}
