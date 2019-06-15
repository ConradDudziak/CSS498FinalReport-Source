using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonACreatureCommand : Command {

	private CardLogic cardLogic;
	private Tile tile;
	private Player player;
	private int uniqueCreatureID;

	public SummonACreatureCommand(CardLogic cardLogic, Player player, Tile tile, int uniqueCreatureID) {
		this.cardLogic = cardLogic;
		this.player = player;
		this.tile = tile;
		this.uniqueCreatureID = uniqueCreatureID;
	}

	public override void StartCommandExecution() {
		player.playerVisual.tableVisual.PlaceCreatureOnTile(player.playerVisual.owner, cardLogic.cardAsset, uniqueCreatureID, tile);

		player.playerVisual.EndCardPreview();
		player.playerVisual.HighlightPlayableCards();

		CommandExecutionComplete(); // Completes the SummonACreatureCommand.
	}
}
