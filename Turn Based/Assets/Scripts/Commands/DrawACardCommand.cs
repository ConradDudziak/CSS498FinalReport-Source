using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACardCommand : Command {

	private Player player;
	private CardLogic cardLogic;

	public DrawACardCommand(CardLogic cl, Player p) {
		this.cardLogic = cl;
		this.player = p;
	}

	public override void StartCommandExecution() {
		player.playerVisual.handVisual.GivePlayerACard(cardLogic.cardAsset, cardLogic.uniqueCardID);

		CommandExecutionComplete(); // end command exeuction for DrawACardCommand;
	}
}
