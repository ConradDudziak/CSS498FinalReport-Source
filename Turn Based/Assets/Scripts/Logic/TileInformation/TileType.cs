using UnityEngine;

[System.Serializable]
public class TileType {

	public GameObject tilePrefab;
	public bool isWalkable = true;
	public float movementCost = 1;

}
