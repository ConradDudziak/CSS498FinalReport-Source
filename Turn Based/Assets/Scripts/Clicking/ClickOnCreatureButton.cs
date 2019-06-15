using UnityEngine;
using UnityEngine.UI;

// This "clicking action" is different from others because it does not need a Clickable component.
// This "clicking action" relies on a UI button.
// Dose NOT inherit from clickingAction.
public class ClickOnCreatureButton : MonoBehaviour {

	public Player playerOwner {
		get {
			if (tag.Contains("Blue")) {
				return GlobalSettings.instance.bluePlayer;
			} else if (tag.Contains("Red")) {
				return GlobalSettings.instance.redPlayer;
			} else {
				Debug.LogError("Untagged Card or creature " + transform.parent.name);
				return null;
			}
		}
	}

	public void OnClick() {
		if (playerOwner == TurnManager.instance.whoseTurn && GlobalSettings.instance.CanControlThisPlayer(TurnManager.instance.whoseTurn)) {

			playerOwner.playerVisual.PreviewACard(GetComponent<IDHolder>().uniqueID);
		}
	}
}
