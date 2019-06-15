using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMaker : TurnMaker {

	public override void OnTurnStart() {
		base.OnTurnStart();
		// Display a message that it is player's turn
		new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
	}
}
