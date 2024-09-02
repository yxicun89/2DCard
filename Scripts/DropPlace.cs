using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card!= null)
        {
            if (card.model.canUse == true){
                // card.cardParent = this.transform;
                card.movement.cardParent = this.transform;
                card.DropField();
            }
        }
    }
}
