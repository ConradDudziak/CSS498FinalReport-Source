using System.Collections;
using UnityEngine;

public class TurnMaker : MonoBehaviour {

	protected Player player;

	void Awake() {
		player = GetComponent<Player>();
	}

	public virtual void OnTurnStart() {
		player.OnTurnStart();
		Debug.Log(this.name + "'s Turn!");
	}
}
