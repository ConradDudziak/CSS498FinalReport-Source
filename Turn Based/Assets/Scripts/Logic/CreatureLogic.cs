using System.Collections.Generic;
using UnityEngine;

public enum CreatureState {
	Idle,
	Moving,
	Attacking
}

[System.Serializable]
public class CreatureLogic {

	public Player owner;
	public CardAsset cardAsset;
	public CreatureEffect effect;
	public int uniqueCreatureID;

	public Tile currentTile;
	public CreatureState creatureState;

	// STATIC For managing IDs
	public static Dictionary<int, CreatureLogic> creaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

	public CreatureLogic(Player owner, CardAsset cardAsset) {
		this.owner = owner;
		this.cardAsset = cardAsset;
		uniqueCreatureID = IDFactory.GetUniqueID();
		if (cardAsset.creatureScriptName != null && cardAsset.creatureScriptName != "") {
			effect = System.Activator.CreateInstance(System.Type.GetType(cardAsset.creatureScriptName), new System.Object[] { owner, this, cardAsset.specialCreatureAmount }) as CreatureEffect;
			// effect.RegisterEventEffect();
		}
		creaturesCreatedThisGame.Add(uniqueCreatureID, this);
	}

	private void Awake() {
		creatureState = CreatureState.Idle;
	}
}
