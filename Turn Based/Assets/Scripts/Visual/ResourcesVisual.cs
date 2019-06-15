using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesVisual : MonoBehaviour {

	public Team owner;

	public Text manaText;
	public Text bloodText;
	public Text earthText;

	public void SetResourcesVisual(int manaCount, int bloodCount, int earthCount) {
		SetManaVisual(manaCount);
		SetBloodVisual(bloodCount);
		SetEarthVisual(earthCount);
	}

	public void SetManaVisual(int manaCount) {
		manaText.text = manaCount.ToString();
	}

	public void SetBloodVisual(int bloodCount) {
		bloodText.text = bloodCount.ToString();
	}

	public void SetEarthVisual(int earthCount) {
		earthText.text = earthCount.ToString();
	}

}
