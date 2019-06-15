using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingOptions {
	NoTarget,
	AllCreatures,
	EnemyCreatures,
	YourCreatures,
	AllCharacters,
	EnemyCharacters,
	YourCharacters
}

[CreateAssetMenu(fileName = "New CardAsset", menuName = "CardAsset")]
public class CardAsset : ScriptableObject {
	// this object will hold the info about the most general card
	[Header("General info")]
	public CharacterAsset characterAsset;  // if this is null, it`s a neutral card
	[TextArea(2, 3)]
	public string description;  // Description for spell or character
	public GameObject creaturePrefab;
	public Sprite creatureSprite;
	public int manaCost;
	public int bloodCost;
	public int earthCost;

	[Header("Creature Info")]
	public int maxHealth; // = 0 => spell card
	public int attack;
	public int attacksForOneTurn = 1;
	//public bool taunt;
	//public bool charge;
	public string creatureScriptName;
	public int specialCreatureAmount;

	[Header("SpellInfo")]
	public string spellScriptName;
	public int specialSpellAmount;
	public TargetingOptions Targets;
}
