using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardLogic {

	// Reference to a player who holds this card.
	public Player owner;
	public int uniqueCardID;
	// Reference to the card asset that stores all the info about this card.
	public CardAsset cardAsset;
	// A script of type spell effect that will be attached to this card we it's created.
	public SpellEffect effect;
	public List<Tile> spawnTiles = new List<Tile>(); // These tiles are locations where this card can be played. INCLUDES TILES THAT MAY BE OCCUPIED. 
	// STATIC (for managing IDs)
	public static Dictionary<int, CardLogic> cardsCreatedThisGame = new Dictionary<int, CardLogic>();

	private int baseManaCost;

	// PROPERTIES
	public int currentManaCost { get; set; }
	public int currentBloodCost { get; set; }
	public int currentEarthCost { get; set; }

	public bool canBePlayed {
		get {
			bool ownersTurn = (TurnManager.instance.whoseTurn == owner);
			return ownersTurn && (currentManaCost <= owner.manaCount) 
							  && (currentBloodCost <= owner.bloodCount) 
							  && (currentEarthCost <= owner.earthCount);
		}
	}

	// CONSTRUCTOR
	public CardLogic(CardAsset cardAsset, Player owner) {
		this.owner = owner;
		this.cardAsset = cardAsset;
		uniqueCardID = IDFactory.GetUniqueID();
		SetManaCost();

		// Create an instance of SpellEffect with a name from our CardAsset and attach it to this cards effect.
		if (cardAsset.spellScriptName != null && cardAsset.spellScriptName != "") {
			effect = System.Activator.CreateInstance(System.Type.GetType(cardAsset.spellScriptName)) as SpellEffect;
		}

		SetSpawnTiles();

		// TODO: Add this card to a dictionary with its ID as a key.
		cardsCreatedThisGame.Add(uniqueCardID, this);
	}

	public void SetManaCost() {
		currentManaCost = cardAsset.manaCost;
		currentBloodCost = cardAsset.bloodCost;
		currentEarthCost = cardAsset.earthCost;
	}

	public void SetSpawnTiles() {
		// Add the neighbors of the players homeTile to the spawnTiles List
		foreach (Tile tile in owner.homeTile.neighbors) {
			spawnTiles.Add(tile);
		}
	}
}
