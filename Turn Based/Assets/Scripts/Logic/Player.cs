using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
	Idle,
	Moving,
	Summoning,
	CreatureSelect
}

public class Player : MonoBehaviour {

	public int playerID;

	public PlayerVisual playerVisual;
	public Deck deck;
	public Hand hand;

	public int manaCount;
	public int bloodCount;
	public int earthCount;

	public PlayerState playerState;

	public Tile homeTile;

	public static Player[] Players;

	public Player otherPlayer {
		get {
			if (Players[0] == this) {
				return Players[1];
			} else {
				return Players[0];
			}
		}
	}

	private CardLogic _cardToBeSummoned;
	public CardLogic cardToBeSummoned {
		get {
			return _cardToBeSummoned;
		}
		set {
			if (value == null) {
				playerState = PlayerState.Idle;
				playerVisual.EndCardPreview();
				if (_cardToBeSummoned != null) {
					playerVisual.tableVisual.HighlightPlayableTiles(_cardToBeSummoned, true);
				}
			} else {
				playerState = PlayerState.Summoning;
				playerVisual.tableVisual.HighlightPlayableTiles(value);
			}
			_cardToBeSummoned = value;
		}
	}

	public CreatureLogic currentCreature;

	private void Awake() {
		// Find all scripts of type Player and store them in Players array.
		Players = GameObject.FindObjectsOfType<Player>();
		// Obtain unique ID from IDFactory
		playerID = IDFactory.GetUniqueID();
	}

	public virtual void OnTurnStart() {
		playerState = PlayerState.Idle;
		manaCount++;
		bloodCount++;
		earthCount++;

		playerVisual.resourcesVisual.SetResourcesVisual(manaCount, bloodCount, earthCount);
	}

	public virtual void OnTurnEnd() {
		cardToBeSummoned = null;
	}

	public void SelectCreature(CreatureLogic creature) {
		playerState = PlayerState.CreatureSelect;
		currentCreature = creature;
	}

	public void MoveCurrentCreature(Tile destination) {
		//if (CanMoveCurrentCreature() && GlobalSettings.instance.CanControlThisPlayer(this)) {
			Debug.Log("working to move");
			List<Tile> path = Table.instance.GeneratePathTo(currentCreature.currentTile, destination);
			new MoveACreatureCommand(currentCreature, this, path).AddToQueue();
		//}
	}

	public bool CanMoveCurrentCreature() {
		if (currentCreature == null) {
			return false;
		}

		return currentCreature.owner == this;
	}

	public void SummonACreature(int uniqueID, Tile tile) {
		SummonACreature(CardLogic.cardsCreatedThisGame[uniqueID], tile);
	}

	public void SummonACreature(CardLogic cardLogic, Tile tile) {
		// Withdraw from Players Resources
		manaCount -= cardLogic.currentManaCost;
		bloodCount -= cardLogic.currentBloodCost;
		earthCount -= cardLogic.currentEarthCost;
		// Update visual player resources
		playerVisual.resourcesVisual.SetResourcesVisual(manaCount, bloodCount, earthCount);

		// Create CreatureLogic and add it to the table.
		CreatureLogic newCreature = new CreatureLogic(this, cardLogic.cardAsset);

		// Add the CreatureLogic to the table Logic at the tile.
		Table.instance.AddCreatureToTile(newCreature, tile);

		new SummonACreatureCommand(cardLogic, this, tile, newCreature.uniqueCreatureID).AddToQueue();

		if (newCreature.effect != null) {
			// newCreature.effect.WhenACreatureIsPlayed();
		}

		playerState = PlayerState.Idle;
		cardToBeSummoned = null;
	}

	public void TransmitInfoAboutPlayerToVisual() {
		playerVisual.gameObject.AddComponent<IDHolder>().uniqueID = playerID;
		if (GetComponent<TurnMaker>() is AITurnMaker) {
			// turn off turn making for this character
			playerVisual.allowedToControlThisPlayer = false;
		} else {
			// allow turn making for this character
			playerVisual.allowedToControlThisPlayer = true;
		}
	}
}
