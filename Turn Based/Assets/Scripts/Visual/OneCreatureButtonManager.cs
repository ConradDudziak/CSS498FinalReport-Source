using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneCreatureButtonManager : MonoBehaviour {

	public CardAsset cardAsset;
	public Image creatureImage;
	public Image highlightGlowImage;

	private bool _canBePlayedNow = false;
	public bool canBePlayedNow {
		get {
			return _canBePlayedNow;
		}

		set {
			_canBePlayedNow = value;

			highlightGlowImage.enabled = value;
		}
	}

	public void ReadInfoFromAsset(CardAsset cardAsset) {
		this.cardAsset = cardAsset;
		creatureImage.sprite = cardAsset.creatureSprite;
	}
}
