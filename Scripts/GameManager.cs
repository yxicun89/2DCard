using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;
    [SerializeField] Text playerManaPointText;
    [SerializeField] Text playerDefaultManaPointText;
    [SerializeField] Transform playerLeaderTransform, enemyLeaderTransform;
    public int playerManaPoint;
    public int playerDefaultManaPoint;
    public bool isPlayerTurn = true;
    List<int> PlayerDeck= new List<int>{1,2,3,2,3,2,1,3,2,1,1,2,3,1,1,2,1};
    List<int> EnemyDeck = new List<int>{1,2,3,2,3,2,1,3,2,1,1,2,3,1,1,2,1};
    public static GameManager instance;
    public int playerLeaderHP;
    public int enemyLeaderHP;

    public void Awake(){
        if(instance == null){
            instance = this;
        }
    }
    
    void Start()
    {
        StartGame();
    }

    void StartGame(){
        enemyLeaderHP = 13000;
        playerLeaderHP = 20000;
        ShowLeaderHP();

        playerManaPoint = 0;
        playerDefaultManaPoint = 0;
        ShowManaPoint();

        SetStartHand();
        StartCoroutine(TurnCalc());
    }

    void ShowManaPoint(){
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }

    public void UseMana(int useMana){
        playerManaPoint -= useMana;
        ShowManaPoint();

        SetCanPanelHand();
    }

    void SetCanPanelHand(){
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerHandCardList){
            if(card.model.cardCost <= playerManaPoint){
                card.model.canUse = true;
                card.view.setCanAttackPanel(true);

            }else{
                card.model.canUse = false;
                card.view.setCanAttackPanel(false);
            }
        }
    }

    void CreateCard(int cardID, Transform place){
        CardController card = Instantiate(cardPrefab, place);
        if (place == playerField){
            card.Init(cardID,true);
        }else{
            card.Init(cardID,false);
        }
    }

    void DrawCard(Transform hand){
        if(PlayerDeck.Count == 0){
            return;
        }

        CardController[] playerHnadCardList = playerHand.GetComponentsInChildren<CardController>();
        
        if (playerHnadCardList.Length <= 5){
            int cardID = PlayerDeck[0];
            PlayerDeck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        SetCanPanelHand();
    }

    void SetStartHand(){
        for(int i = 0; i < 3; i++){
            DrawCard(playerHand);
            // DrawCard(enemyField);
        }
    }

    IEnumerator TurnCalc(){
        yield return StartCoroutine(uiManager.ShowChangeTurnPanel());
        if(isPlayerTurn){
            PlayerTurn();
        }else{
            StartCoroutine(EnemyTurn());
        }
    }

    public void ChangeTurn(){
        isPlayerTurn = !isPlayerTurn;
        StartCoroutine(TurnCalc());
    }

    void PlayerTurn(){
        Debug.Log("Playerのターン");
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList,true);
        playerDefaultManaPoint++;
        playerManaPoint = playerDefaultManaPoint;
        ShowManaPoint();
        DrawCard(playerHand);
    }
    IEnumerator EnemyTurn(){
        Debug.Log("Enemyのターン");
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
    
        yield return new WaitForSeconds(1f);
    
        SetAttackableFieldCard(enemyFieldCardList,true);
    
        yield return new WaitForSeconds(1f);
        
        if(enemyFieldCardList.Length < 5){
            int cardID = EnemyDeck[0];
            EnemyDeck.RemoveAt(0);
            CreateCard(cardID,enemyField);
        }
    
        CardController[] enemyFieldCardListSecond = enemyField.GetComponentsInChildren<CardController>();
    
        yield return new WaitForSeconds(1f);
    
    
        while (Array.Exists(enemyFieldCardListSecond, card => card != null && card.model.canAttack))
        {
    
            // 攻撃可能カードを取得
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardListSecond, card => card != null && card.model.canAttack);
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
    
            if (enemyCanAttackCardList.Length == 0)
            {
                yield break;
            }
    
            CardController attackCard = enemyCanAttackCardList[0];
    
            if (attackCard == null)
            {
                yield break;
            }
    
            if (playerFieldCardList.Length > 0) // プレイヤーの場にカードがある場合
            {
                int defenceCardNumber = UnityEngine.Random.Range(0, playerFieldCardList.Length);
                CardController defenceCard = playerFieldCardList[defenceCardNumber];
                yield return StartCoroutine(attackCard.movement.AttackMotion(defenceCard.transform));
                if (defenceCard != null)
                {
                    CardBattle(attackCard, defenceCard);
                    Debug.Log("プレイヤー側にカードがある");
                }
            }
            else
            {
                yield return StartCoroutine(attackCard.movement.AttackMotion(playerLeaderTransform));
                AttackToLeader(attackCard, false);
                Debug.Log("攻撃対象がいないのでリターン");
            }
    
            yield return new WaitForSeconds(1f);
    
            enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
    
        }
    
        ChangeTurn();
    }


    public void CardBattle(CardController attackCard, CardController defenceCard){
        
        if (attackCard.model.canAttack == false){
            return;
        }

        // if (defenceCard.transform.parent == playerHand || defenceCard.transform.parent == playerField){
        //     return;

        // }


        if (attackCard.model.cardAttack > defenceCard.model.cardAttack){
            // StartCoroutine(attackCard.movement.AttackMotion(defenceCard.transform));
            defenceCard.DestroyCard(defenceCard);
        }

        if (attackCard.model.cardAttack < defenceCard.model.cardAttack){
            // StartCoroutine(attackCard.movement.AttackMotion(defenceCard.transform));
            attackCard.DestroyCard(attackCard);
        }

        if (attackCard.model.cardAttack == defenceCard.model.cardAttack){
            // StartCoroutine(attackCard.movement.AttackMotion(defenceCard.transform));
            attackCard.DestroyCard(attackCard);
            defenceCard.DestroyCard(defenceCard);
        }
        
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard){
            return;
        }

        attackCard.model.canAttack = false;
        attackCard.view.setCanAttackPanel(false);
    }

    void SetAttackableFieldCard(CardController[] cardList,bool canAttack){
        foreach(CardController card in cardList){
            card.model.canAttack = canAttack;
            card.view.setCanAttackPanel(canAttack);
        }
    }

    public void AttackToLeader(CardController attackCard, bool isPlayerCard){
        // StartCoroutine(AttackToLeaderCoroutine(attackCard,isPlayerCard));

        if(attackCard.model.canAttack == false){
            return;
        }

        if (attackCard.model.PlayerCard == true){
            enemyLeaderHP -= attackCard.model.cardAttack;
        }
        else{
            playerLeaderHP -= attackCard.model.cardAttack;
        }


        enemyLeaderHP -= attackCard.model.cardAttack;
        attackCard.model.canAttack = false;
        attackCard.view.setCanAttackPanel(false);
        Debug.Log("敵のHP:" + enemyLeaderHP);
        ShowLeaderHP();
    }

    public void ShowLeaderHP(){
        if (playerLeaderHP <= 0){
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0){
            enemyLeaderHP = 0;
        }

        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
    }
}
