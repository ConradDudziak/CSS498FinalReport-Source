using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVisual : MonoBehaviour {

	public GameObject creaturesParent;

	// Location for all Creature GameObjects
	public Dictionary<int, GameObject> creatureGameObjectsByID = new Dictionary<int, GameObject>();

	// Location for all Tile GameObjects
	public Dictionary<int, GameObject> tileGameObjectsByID = new Dictionary<int, GameObject>();

	// Highlight tiles that a player can play on.
	public void HighlightPlayableTiles(CardLogic cardLogic, bool removeAllHighlights = false) {
		foreach (Tile tile in cardLogic.spawnTiles) {
			if (!Table.instance.TileIsOccupied(tile) && !removeAllHighlights) {
				tile.GetComponentInChildren<Renderer>().material = GlobalSettings.instance.highlightedTileMaterial;
			} else if (removeAllHighlights) {
				tile.GetComponentInChildren<Renderer>().material = GlobalSettings.instance.normalTileMaterial;
			}
		}
	}

	public void PlaceCreatureOnTile(Team owner, CardAsset cardAsset, int uniqueCreatureID, Tile tile) {
		// GameObject creature = Instantiate(GlobalSettings.instance.tableCreaturePrefab, tile.transform.position, Quaternion.identity) as GameObject;
		GameObject creature = Instantiate(cardAsset.creaturePrefab, tile.transform.position, Quaternion.identity) as GameObject;
		OneCreatureManager oneCreatureManager = creature.GetComponent<OneCreatureManager>();
		creature.AddComponent<IDHolder>().uniqueID = uniqueCreatureID;

		// Add the visual Creature GameObject to a list of Creature GameObjects
		creatureGameObjectsByID.Add(uniqueCreatureID, creature);

		// Visually Construct this Creature
		oneCreatureManager.ReadCreatureFromAsset(cardAsset);

		// add tag according to owner
		foreach (Transform t in creature.GetComponentsInChildren<Transform>()) {
			t.tag = owner.ToString() + "Creature";
		}

		creature.transform.SetParent(creaturesParent.transform);
	}
}
