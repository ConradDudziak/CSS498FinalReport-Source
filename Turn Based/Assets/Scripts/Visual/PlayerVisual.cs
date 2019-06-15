using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { Blue, Red }

public class PlayerVisual : MonoBehaviour {

	public Team owner;
	public bool controlsOn = true;
	public HandVisual handVisual;
	public ResourcesVisual resourcesVisual;
	public TableVisual tableVisual;
	public OneCardManager previewCreatureCardManager;

	public bool allowedToControlThisPlayer {
		get;
		set;
	}

	public void EndCardPreview() {
		previewCreatureCardManager.gameObject.SetActive(false);
	}

	public void PreviewACard(int uniqueID) {
		PreviewACard(CardLogic.cardsCreatedThisGame[uniqueID]);
	}

	public void PreviewACard(CardLogic cardLogic) {
		previewCreatureCardManager.gameObject.SetActive(false);
		previewCreatureCardManager.cardAsset = cardLogic.cardAsset;
		previewCreatureCardManager.ReadCardFromAsset();
		previewCreatureCardManager.summonButton.GetComponent<ClickOnSummonButton>().cardToBeSummoned = cardLogic;
		previewCreatureCardManager.gameObject.SetActive(true);
	}

	// METHODS TO SHOW GLOW HIGHLIGHTS
	public void HighlightPlayableCards(bool removeAllHighlights = false) {
		foreach (CardLogic cl in GlobalSettings.instance.players[owner].hand.cardsInHand) {
			GameObject g = IDHolder.GetGameObjectWithID(cl.uniqueCardID);
			if (g != null) {
				g.GetComponent<OneCreatureButtonManager>().canBePlayedNow = cl.canBePlayed && !removeAllHighlights;
			}
		}
	}
}
