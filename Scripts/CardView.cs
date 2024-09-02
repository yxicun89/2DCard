using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, costText, attackText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel,canUsePanel;

    public void ShowCard(CardModel cardModel){
        nameText.text = cardModel.cardName;
        costText.text = cardModel.cardCost.ToString();
        attackText.text = cardModel.cardAttack.ToString();
        iconImage.sprite = cardModel.cardIcon;
    }

    public void setCanAttackPanel(bool isActive){
        canAttackPanel.SetActive(isActive);
    }

    public void setCanUsePanel(bool isActive){
        canUsePanel.SetActive(isActive);
    }
}
