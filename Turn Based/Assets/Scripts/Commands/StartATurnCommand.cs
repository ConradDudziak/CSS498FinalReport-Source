using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartATurnCommand : Command {

	private Player _player;

	public StartATurnCommand(Player player) {
		_player = player;
	}

	public override void StartCommandExecution() {
		// This Command is completed instantly. 
		// Setting whoseTurn calls the OnTurnStart function of the Player value.
		TurnManager.instance.whoseTurn = _player;

		CommandExecutionComplete();
	}
}
