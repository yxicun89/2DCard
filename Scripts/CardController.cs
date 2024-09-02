using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    public CardView view;
    public CardModel model;
    public CardMovement movement;

    private void Awake(){
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID,bool playerCard){
        model = new CardModel(cardID,playerCard);
        view.ShowCard(model);
    }

    public void DestroyCard(CardController card){
        Destroy(card.gameObject);
    }

    public void DropField(){
        GameManager.instance.UseMana(model.cardCost);
        model.FieldCard = true;
        model.canUse = false;
        view.setCanUsePanel(model.canUse);
    }
}
