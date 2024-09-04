using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [SerializeField] CardController cardPrefab;

    public int PlayerCapacity = 10;
    public int EnemyCapacity = 10;
    List<int> PlayerDeck;//= new List<int>(Capacity);
    List<int> EnemyDeck;// = new List<int>(Capacity);
    public Transform PlayerHand, PlayerField, EnemyHand,EnemyField;
    public int PlayerScore = 0;
    public int EnemyScore = 0;
    public bool PlayerSkill = false;
    public bool EnemySkill = false;
    public int FirstHand = 3;

    public void Awake() {
        PlayerDeck = new List<int>(PlayerCapacity);
        EnemyDeck = new List<int>(EnemyCapacity);
    }

    void Start() {
        // StartGame();
        SetStartHand();
        // DrawCard(playerHand);
    }

    //  void StartGame(){
        // SetStartHand();
    // }
    
    void SetStartHand(){
        // プレイヤーのデッキを乱数で作成
        Debug.Log("StartGame");
        for (int i = 0; i < PlayerCapacity; i++){
            int PlayerCardID = UnityEngine.Random.Range(0, PlayerCapacity);
            PlayerDeck.Add(PlayerCardID);
            Debug.Log(PlayerCardID);
        }

        // 相手のデッキを乱数で作成
        for (int i = 0; i <EnemyCapacity ; i++){
            int EnemyCardID = UnityEngine.Random.Range(0, EnemyCapacity);
            EnemyDeck.Add(EnemyCardID);
            Debug.Log(EnemyCardID);
        }
        // プレイヤーの手札を3枚用意
        // Debug.Log();
        // Debug.log();

        for (int i = 0; i < 3; i++){
            int PlayerDeckNumber = UnityEngine.Random.Range(0,PlayerCapacity);
            int cardID = PlayerDeck[PlayerDeckNumber];
            // Debug.Log(cardID);
            CreateCard(cardID, PlayerHand);
            PlayerDeck.RemoveAt(PlayerDeckNumber);
            PlayerCapacity--;
            Debug.Log("Playerhand");
        }
        
        // 相手の手札を3枚用意
        for (int i = 0; i < FirstHand; i++){
            int EnemyDeckNumber = UnityEngine.Random.Range(0, EnemyCapacity);
            int cardID = EnemyDeck[EnemyDeckNumber];
            CreateCard(cardID, EnemyHand);
            EnemyDeck.RemoveAt(cardID);
            EnemyCapacity--;
            Debug.Log("Enemyhand");
        }
    }

    // カードを生成する(オブジェクトを)
    void CreateCard(int cardID, Transform place){
        CardController card = Instantiate(cardPrefab, place);
        if (place == PlayerField){
            card.Init(cardID,place);
        }else{
            card.Init(cardID,place);
        }
    }

    // 次ラウンドから手札の補充
    void DrawCard(Transform hand){

        if(PlayerDeck.Count == 0){
            return;
        }

        CardController[] PlayerHandCardList = PlayerHand.GetComponentsInChildren<CardController>();
        CardController[] EnemyHandCardList = EnemyHand.GetComponentsInChildren<CardController>();

        // プレイヤー手札が減った時に補充
        if (PlayerHandCardList.Length <= 3){
            int PlayerDeckNumber = UnityEngine.Random.Range(0, PlayerCapacity);
            int cardID = PlayerDeck[PlayerDeckNumber];
            CreateCard(cardID, PlayerHand);
            PlayerDeck.RemoveAt(cardID);
            PlayerCapacity--;
        }

        // 相手の手札が減った時に補充
        if (EnemyHandCardList.Length <= 3){
            int EnemyDeckNumber = UnityEngine.Random.Range(0, EnemyCapacity);
            int cardID = EnemyDeck[EnemyDeckNumber];
            CreateCard(cardID, EnemyHand);
            EnemyDeck.RemoveAt(cardID);
            EnemyCapacity--;
        }

    }


    void PlayerTurn(){
        Debug.Log("Playerのターン");
        CardController[] playerFieldCardList = PlayerField.GetComponentsInChildren<CardController>();
        DrawCard(playerFieldCardList[0].transform);
    }



    IEnumerator EnemyTurn(){
        Debug.Log("Enemyのターン");
        CardController[] EnemyHandCardList = EnemyHand.GetComponentsInChildren<CardController>();
        yield return new WaitForSeconds(1f);
        
        int EnemyhandNumber = UnityEngine.Random.Range(0, EnemyHandCardList.Length);
        // 敵フィールドにカード生成して手札のオブジェクト削除
        CardController EnemyCard = EnemyHandCardList[EnemyhandNumber];
        EnemyCard.transform.SetParent(EnemyField, false);
        yield return new WaitForSeconds(1f);
        
        CardController[] playerFieldCardList = PlayerField.GetComponentsInChildren<CardController>();
        // バトル処理
        CardBattle(EnemyCard, playerFieldCardList[0]);
        EnemyHandCardList[EnemyhandNumber].DestroyCard(EnemyHandCardList[EnemyhandNumber]);
        NextRound();
    }


    public void CardBattle(CardController attackCard, CardController defenceCard){
    
        if (attackCard.model.cardAttack > defenceCard.model.cardAttack){           
            PlayerScore++;
        }

        if (attackCard.model.cardAttack < defenceCard.model.cardAttack){
            EnemyScore++;
        }
        attackCard.DestroyCard(attackCard);
        defenceCard.DestroyCard(defenceCard);
        NextRound();
    }

    public void NextRound(){
        if (PlayerScore >= 3){
            Debug.Log("Playerの勝利");
        }else if(EnemyScore >= 3){
            Debug.Log("Enemyの勝利");
        }else{
            PlayerTurn();
        }
    }
}
