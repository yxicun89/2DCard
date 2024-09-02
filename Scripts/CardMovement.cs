using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    bool canDrag = true;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        CardController card = GetComponent<CardController>();
        canDrag = true;

        if (card.model.FieldCard == false){
            if (card.model.canUse == false){
                canDrag = false;
            }
        }
        else{
            if (card.model.canAttack == false){
                canDrag = false;
            }
        }

        if (canDrag == false){
            return;
        }

        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (canDrag == false){
            return;
        }
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag == false){
            return;
        }

        transform.SetParent(cardParent,false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public IEnumerator AttackMotion(Transform target){
        Vector3 currentPosition = transform.position;
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent);
        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        transform.DOMove(currentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);

        transform.SetParent(cardParent);
    }
}
