using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour {

	private bool _clicking = false;

	// distance from the center of this Game Object to the point where we clicked to start dragging 
	private Vector3 pointerDisplacement;

	// distance from camera to mouse on Z axis 
	private float zDisplacement;

	// reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
	private ClickingActions clickingActions;

	// STATIC property that returns the instance of Draggable that is currently being dragged
	private static Clickable _clickingThis;
	public static Clickable clickingThis {
		get {
			return _clickingThis;
		}
	}

	// MONOBEHAVIOUR METHODS
	void Awake() {
		clickingActions = GetComponent<ClickingActions>();
	}

	void OnMouseDown() {
		if (clickingActions != null && clickingActions.canClick) {
			_clicking = true;
			_clickingThis = this;
			clickingActions.OnClick();
			zDisplacement = -Camera.main.transform.position.z + transform.position.z;
			pointerDisplacement = -transform.position + MouseInWorldCoords();
		}
	}

	// Update is called once per frame
	void Update() {
		if (_clicking) {
			Vector3 mousePos = MouseInWorldCoords();
			transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
			clickingActions.OnClickingInUpdate();
		}
	}

	void OnMouseUp() {
		if (_clicking) {
			_clicking = false;
			_clickingThis = null;
			clickingActions.OnEndClick();
		}
	}

	// returns mouse position in World coordinates for our GameObject to follow. 
	private Vector3 MouseInWorldCoords() {
		var screenMousePos = Input.mousePosition;
		//Debug.Log(screenMousePos);
		screenMousePos.z = zDisplacement;
		return Camera.main.ScreenToWorldPoint(screenMousePos);
	}
}
