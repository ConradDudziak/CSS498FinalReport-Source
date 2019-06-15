using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTile : ClickingActions {
	public override void OnClick() {
		Debug.Log("Clicked!");
		throw new System.NotImplementedException();
	}

	public override void OnClickingInUpdate() {
		throw new System.NotImplementedException();
	}

	public override void OnEndClick() {
		throw new System.NotImplementedException();
	}

	protected override bool ClickSuccessful() {
		throw new System.NotImplementedException();
	}
}
