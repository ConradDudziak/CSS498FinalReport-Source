using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnMaker : TurnMaker {
	
	public override void OnTurnStart() {
		base.OnTurnStart();
		// dispay a message that it is enemy`s turn
		new ShowMessageCommand("Enemy`s Turn!", 2.0f).AddToQueue();
		StartCoroutine(MakeAITurn());
	}

	// THE LOGIC FOR AI
	IEnumerator MakeAITurn() {
		while (MakeOneAIMove()) {
			yield return null;
		}
		MoveCreature();
		InsertDelay(1f);

		TurnManager.instance.EndTurn();
	}

	
	bool MakeOneAIMove() {
		return AttackWithACreature() || PlayACardFromHand();
	}

	bool PlayACardFromHand() {
		foreach (CardLogic c in player.hand.cardsInHand) {
			if (c.canBePlayed) {
				Tile spawnTile = chooseSpawnTile(c);
				if (spawnTile != null) {
					player.SummonACreature(c, spawnTile);
					InsertDelay(1.5f);
					return true;
				}
			}
		}
		Debug.Log("Nothing to summon");
		return false;
	}

	bool MoveCreature() {
		//Debug.Log("Try move creature");
		CreatureLogic creatureLogic = chooseACreature();
		if (creatureLogic != null) {
			Debug.Log("Found a creature");
			Tile tile = chooseTile();
			if (tile != null) {
				Debug.Log("Found tile to go to: " + tile.x + ", " + tile.y);
				player.SelectCreature(creatureLogic);
				player.MoveCurrentCreature(tile);
				InsertDelay(1.5f);
				return true;
			}
		}
		Debug.Log("Nothing to move");
		return false;
	}

	bool AttackWithACreature() {
		return false;
	}

	void InsertDelay(float delay) {
		new DelayCommand(delay).AddToQueue();
	}

	private Tile chooseSpawnTile(CardLogic c) {
		foreach (Tile tile in c.spawnTiles) {
			if (!Table.instance.TileIsOccupied(tile)) {
				return tile;
			}
		}
		return null;
	}

	private CreatureLogic chooseACreature() {
		List<CreatureLogic> myCreatures = new List<CreatureLogic>();
		foreach (KeyValuePair<int, CreatureLogic> entry in CreatureLogic.creaturesCreatedThisGame) {
			if (entry.Value.owner == player) {
				myCreatures.Add(entry.Value);
			}
		}

		if (myCreatures.Count <= 0) {
			return null;
		}
		return myCreatures[Random.Range(0, myCreatures.Count)];
	}

	private Tile chooseTile() {
		List<Tile> availableTiles = new List<Tile>();
		foreach (KeyValuePair<int, GameObject> entry in Table.instance.tableVisual.tileGameObjectsByID) {
			Tile currentTile = entry.Value.GetComponent<Tile>();
			if (Table.instance.CreatureCanEnterTile(currentTile)) {
				availableTiles.Add(currentTile);
			}
		}

		if (availableTiles.Count <= 0) {
			return null;
		}

		return availableTiles[Random.Range(0, availableTiles.Count)];
	}
}
