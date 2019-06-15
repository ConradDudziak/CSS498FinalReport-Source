using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneCardManager : MonoBehaviour {

	public CardAsset cardAsset;
	//public OneCardManager previewManager;
	[Header("GameObject (Transform) References")]
	public GameObject resources;
	public GameObject creaturePreviewLocation;
	[Header("Sprite References")]
	public Sprite manaSprite;
	public Sprite bloodSprite;
	public Sprite earthSprite;
	[Header("Text Component References")]
	public Text nameText;
	public Text descriptionText;
	public Text healthText;
	public Text attackText;
	[Header("Image References")]
	public Image cardBodyImage;
	[Header("Button References")]
	public Button summonButton;

	// PRIVATE : a list of all resources. 
	// NEVER LARGER THAN 4
	private List<Sprite> _resourceSprites = new List<Sprite>();

	private bool _canBePlayedNow = false;
	public bool canBePlayedNow {
		get {
			return _canBePlayedNow;
		}
		set {
			_canBePlayedNow = value;
		}
	}

	private void Awake() {
		// ReadCardFromAsset
		if (cardAsset != null) {
			ReadCardFromAsset();
		}
	}

	public void ReadCardFromAsset() {
		ResetCard();

		if (cardAsset.characterAsset != null) {
			cardBodyImage.color = cardAsset.characterAsset.classCardTint;
		}
		// 2) add card name
		nameText.text = cardAsset.name;
		// 3) add mana cost
		AddResourceCost(cardAsset.manaCost, cardAsset.bloodCost, cardAsset.earthCost);
		// 4) add description
		descriptionText.text = cardAsset.description;
		// 5) add the card creature preview
		GameObject creatureGFX = null;
		foreach (Transform child in cardAsset.creaturePrefab.transform) {
			if (child.CompareTag("GFX")) {
				creatureGFX = child.gameObject;
				break;
			}
		}

		if (creatureGFX == null) {
			Debug.LogError("No GFX label set for " + cardAsset.creaturePrefab);
		}

		GameObject creaturePreview = Instantiate(creatureGFX, creaturePreviewLocation.transform.position, creaturePreviewLocation.transform.rotation) as GameObject;
		creaturePreview.transform.SetParent(creaturePreviewLocation.transform);
		creaturePreview.AddComponent<Spin>().speed = 80;
		creaturePreview.layer = creaturePreviewLocation.layer;

		if (cardAsset.maxHealth != 0) {
			// this is a creature
			attackText.text = cardAsset.attack.ToString();
			healthText.text = cardAsset.maxHealth.ToString();
		}
	}

	private void AddResourceCost(int manaCost, int bloodCost, int earthCost) {
		if (manaCost != 0) {
			for (int i = manaCost; i > 0; i--) {
				_resourceSprites.Insert(0, manaSprite);
			}
		}

		if (bloodCost != 0) {
			for (int i = bloodCost; i > 0; i--) {
				_resourceSprites.Insert(0, bloodSprite);
			}
		}

		if (earthCost != 0) {
			for (int i = earthCost; i > 0; i--) {
				_resourceSprites.Insert(0, earthSprite);
			}
		}

		PlaceResourcesOnSlots();
	}

	private void PlaceResourcesOnSlots() {
		SameDistanceChildren slots = resources.GetComponentInChildren<SameDistanceChildren>();
		int currentSlot = 0;
		foreach (Sprite s in _resourceSprites) {
			Image currentSlotImage = slots.children[currentSlot].GetComponent<Image>();
			currentSlotImage.sprite = s;
			Color tempColor = currentSlotImage.color;
			tempColor.a = 1;
			currentSlotImage.color = tempColor;
			currentSlot++;
		}

		UpdatePlacementOfSlots(slots);
	}

	// move Slots GameObject according to the number of resources
	private void UpdatePlacementOfSlots(SameDistanceChildren slots) {
		slots.setDistances();

		float posX;
		if (_resourceSprites.Count > 0)
			posX = slots.children[0].GetComponent<RectTransform>().anchoredPosition.x - slots.children[_resourceSprites.Count - 1].GetComponent<RectTransform>().anchoredPosition.x;
		else
			posX = 0f;

		RectTransform slotsRectTransform = slots.gameObject.GetComponent<RectTransform>();
		slotsRectTransform.anchoredPosition += new Vector2 (posX / 2, 0);
	}

	private void ResetCard() {
		ResetResourceSlots();
		ResetCreaturePreview();
	}

	private void ResetResourceSlots() {
		SameDistanceChildren slots = resources.GetComponentInChildren<SameDistanceChildren>();
		// move the Slots GameObject back to its origin.
		ResetSlotRectTransform(slots);
		// Clear the sprite list
		_resourceSprites.Clear();
		// reset the art
		foreach (Transform imageTransform in slots.children) {
			Image currentSlotImage = imageTransform.GetComponent<Image>();
			currentSlotImage.sprite = null;
			Color tempColor = currentSlotImage.color;
			tempColor.a = 0;
			currentSlotImage.color = tempColor;
		}
	}

	private void ResetCreaturePreview() {
		// Destroy all gameObjects in the preview location.
		if(creaturePreviewLocation.transform.childCount > 0) {
			Destroy(creaturePreviewLocation.transform.GetChild(0).gameObject);
		}
	}

	private void ResetSlotRectTransform(SameDistanceChildren slots) {
		float backX;
		if (_resourceSprites.Count > 0)
			backX = slots.children[0].GetComponent<RectTransform>().anchoredPosition.x - slots.children[_resourceSprites.Count - 1].GetComponent<RectTransform>().anchoredPosition.x;
		else
			backX = 0f;

		RectTransform slotsRectTransform = slots.gameObject.GetComponent<RectTransform>();
		slotsRectTransform.anchoredPosition -= new Vector2(backX / 2, 0);
	}
}
