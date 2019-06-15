using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	#region Singleton
	public static TurnManager instance;

	private void Awake() {
		instance = this;
		// TODO: Timer.
		timer = GetComponent<RopeTimer>();
	}
	#endregion

	// TODO Timer: Add Timer
	private RopeTimer timer;
	// TODO CreatureLogic: Clear Static variables OnGameStart
	// TODO CardLogic: Clear Statics
	private Player _whoseTurn;
	public Player whoseTurn {
		get {
			return _whoseTurn;
		}
		set {
			_whoseTurn = value;
			
			// Timer
			timer.StartTimer();
			GlobalSettings.instance.EnableEndTurnButtonOnStart(_whoseTurn);

			TurnMaker turnMaker = whoseTurn.GetComponent<TurnMaker>();
			// player`s method OnTurnStart() will be called in tm.OnTurnStart();
			turnMaker.OnTurnStart();
			// HIGHLIGHT AND REMOVE HIGHLIGHTS
			
			if (turnMaker is PlayerTurnMaker) {
				whoseTurn.playerVisual.HighlightPlayableCards();
			}
			whoseTurn.otherPlayer.playerVisual.HighlightPlayableCards(true);
		}
	}

	private void Start() {
		OnGameStart();
	}

	public void OnGameStart() {

		Table.instance.TableSetup();
		//CardLogic.cardsCreatedThisGame.Clear();
		//CreatureLogic.CreaturesCreatedThisGame.Clear();

		
		foreach (Player p in Player.Players) {
			DrawPlayerCards(p);
			p.TransmitInfoAboutPlayerToVisual();
			p.manaCount = 0;
			p.bloodCount = 0;
			p.earthCount = 0;
			//p.ManaThisTurn = 0;
			//p.ManaLeft = 0;
			//p.LoadCharacterInfoFromAsset();
			//p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
			// move both portraits to the center
			//p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
		}

		// determine who starts the game.
		int rnd = Random.Range(0, 2);  // 2 is exclusive boundary
									   // Debug.Log(Player.Players.Length);
		Player whoGoesFirst = Player.Players[rnd];
		Debug.Log(whoGoesFirst.name + " Goes First!");
		Player whoGoesSecond = whoGoesFirst.otherPlayer;

		new StartATurnCommand(whoGoesFirst).AddToQueue();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			EndTurn();
		}
	}

	public void EndTurn() {
		// stop timer
		timer.StopTimer();
		// send all commands in the end of current player`s turn
		whoseTurn.OnTurnEnd();

		new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
	}

	public void StopTheTimer() {
		// timer.StopTimer();
	}

	private void DrawPlayerCards(Player p) {
		foreach (CardAsset ca in p.deck.cards) {
			CardLogic newCard = new CardLogic(ca, p);
			p.hand.cardsInHand.Add(newCard);
			new DrawACardCommand(newCard, p).AddToQueue();
		}
	}
}
