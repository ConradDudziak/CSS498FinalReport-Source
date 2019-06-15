using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettings : MonoBehaviour {

	#region Singleton
	public static GlobalSettings instance;

	private void Awake() {
		players.Add(Team.Blue, bluePlayer);
		players.Add(Team.Red, redPlayer);
		instance = this;
	}
	#endregion

	public Player bluePlayer;
	public Player redPlayer;

	public TileType[] tileTypes;

	public GameObject creatureButtonPrefab;
	public GameObject tableCreaturePrefab;
	public GameObject clickVisualPrefab;

	public int maxHeight;
	public int maxWidth;

	public Material highlightedTileMaterial;
	public Material normalTileMaterial;

	public Button EndTurnButton;

	public Dictionary<Team, Player> players = new Dictionary<Team, Player>();

	public bool CanControlThisPlayer(Team owner) {
		bool playersTurn = (TurnManager.instance.whoseTurn == players[owner]);
		return players[owner].playerVisual.allowedToControlThisPlayer && players[owner].playerVisual.controlsOn && playersTurn;
	}

	public bool CanControlThisPlayer(Player ownerPlayer) {
		bool playersTurn = (TurnManager.instance.whoseTurn == ownerPlayer);
		return ownerPlayer.playerVisual.allowedToControlThisPlayer && ownerPlayer.playerVisual.controlsOn && playersTurn;
	}

	public void EnableEndTurnButtonOnStart(Player p) {
		if (CanControlThisPlayer(p)) {
			EndTurnButton.interactable = true;
		} else {
			EndTurnButton.interactable = false;
		}
	}
}
