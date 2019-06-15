using System.Collections;
using UnityEngine;

public abstract class ClickingActions : MonoBehaviour {

	public abstract void OnClick();
	public abstract void OnEndClick();
	public abstract void OnClickingInUpdate();

	public virtual bool canClick {
		get {
			return GlobalSettings.instance.CanControlThisPlayer(playerOwner);
		}
	}

	protected virtual Player playerOwner {
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

	protected abstract bool ClickSuccessful();
}
