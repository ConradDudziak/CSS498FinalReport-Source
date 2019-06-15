using UnityEngine;
using UnityEngine.UI;

// This "clicking action" is different from others because it does not need a Clickable component.
// This "clicking action" relies on a UI button.
// Dose NOT inherit from clickingAction.
public class ClickOnSummonButton : MonoBehaviour {

	public CardLogic cardToBeSummoned;

	public void OnClick() {
		if (cardToBeSummoned.canBePlayed && GlobalSettings.instance.CanControlThisPlayer(TurnManager.instance.whoseTurn)) {
			Debug.Log("Choose a tile to play " + cardToBeSummoned.cardAsset.name + " " + cardToBeSummoned.uniqueCardID);
			cardToBeSummoned.owner.cardToBeSummoned = cardToBeSummoned;
		} else {
			Debug.Log("Cannot play " + cardToBeSummoned.cardAsset.name + " " + cardToBeSummoned.uniqueCardID);
		}
	}
}
