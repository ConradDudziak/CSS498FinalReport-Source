using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisual : MonoBehaviour {

	public Team owner;

	public GameObject buttonLayout;

	public void GivePlayerACard(CardAsset cardAsset, int uniqueCardID) {
		
		GameObject card = GameObject.Instantiate(GlobalSettings.instance.creatureButtonPrefab, this.transform.position, Quaternion.identity) as GameObject;
		card.name = owner + " CardButton_" + uniqueCardID;
		card.transform.SetParent(buttonLayout.transform, false);
		card.tag = owner.ToString();
		card.GetComponent<OneCreatureButtonManager>().ReadInfoFromAsset(cardAsset);

		// pass a unique ID to this card.
		IDHolder id = card.AddComponent<IDHolder>();
		id.uniqueID = uniqueCardID;
		
	}

	public void DisplayACardPreview(int uniqueCardID) {

	}
}
