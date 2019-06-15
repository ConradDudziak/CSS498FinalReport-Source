using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveACreatureCommand : Command {

	private CreatureLogic creatureLogic;
	private List<Tile> path;
	private Player player;

	public MoveACreatureCommand(CreatureLogic creatureLogic, Player player, List<Tile> path) {
		this.creatureLogic = creatureLogic;
		this.player = player;
		this.path = path;
	}

	public override void StartCommandExecution() {
		GameObject creatureGameObject = Table.instance.tableVisual.creatureGameObjectsByID[creatureLogic.uniqueCreatureID];
		creatureGameObject.GetComponent<OneCreatureManager>().Move(creatureLogic, path);
		
		player.currentCreature = null;
		player.playerState = PlayerState.Idle;

		CommandExecutionComplete(); // Completes the MoveACreatureCommand.
	}
}
