using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject changeTurnPanel;
    [SerializeField] Text changeTurnText;

    public IEnumerator ShowChangeTurnPanel(){
        changeTurnPanel.SetActive(true);

        if (GameManager.instance.isPlayerTurn){
            changeTurnText.text = "Your Turn";
        } else {
            changeTurnText.text = "Enemy Turn";
        }
        yield return new WaitForSeconds(2);
        changeTurnPanel.SetActive(false);
    }
}
