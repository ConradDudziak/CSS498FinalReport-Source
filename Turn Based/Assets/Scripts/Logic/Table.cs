using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {

	#region Singleton
	public static Table instance;

	private void Awake() {
		instance = this;
	}
	#endregion

	public TableVisual tableVisual;
	public Tile[,] tableTiles;

	private int _width;
	private int _height;

	// Map tiles to their corresponding creature logics
	private Dictionary<Tile, CreatureLogic> tilesWithCreatures = new Dictionary<Tile, CreatureLogic>();

	// Make Dictionary<int, Tile> tilesByID

	// Use this for initialization
	public void TableSetup() {
		_width = GlobalSettings.instance.maxWidth;
		_height = GlobalSettings.instance.maxHeight;
		tableTiles = new Tile[_width, _height];

		Tile[] tiles = tableVisual.transform.GetComponentsInChildren<Tile>();

		for (int i = 0; i < tiles.Length; i++) {
			tableTiles[tiles[i].x, tiles[i].y] = tiles[i];

			// Add ID to all table tiles
			int currentID = IDFactory.GetUniqueID();
			tableTiles[tiles[i].x, tiles[i].y].gameObject.AddComponent<IDHolder>().uniqueID = currentID;

			// Add the Tile GameObject to the tableVisual data
			tableVisual.tileGameObjectsByID.Add(currentID, tableTiles[tiles[i].x, tiles[i].y].gameObject);
		}

		GeneratePathFindingGraph();
	}

	public List<Tile> GeneratePathTo(Tile sourceTile, Tile targetTile) {
		Dictionary<Tile, float> weights = new Dictionary<Tile, float>();
		Dictionary<Tile, Tile> pointers = new Dictionary<Tile, Tile>();
		List<Tile> unvisited = new List<Tile>();

		weights[sourceTile] = 0;
		pointers[sourceTile] = null;

		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
				Tile current = tableTiles[x, y];
				if (current != null) {
					if (current != sourceTile) {
						weights[current] = Mathf.Infinity;
						pointers[current] = null;
					}
					unvisited.Add(current);
				}
			}
		}

		/*
		foreach (Tile tile in tableTiles) {
			if (tile != sourceTile) {
				Debug.Log(tile);
				weights[tile] = Mathf.Infinity;
				pointers[tile] = null;
			}
			unvisited.Add(tile);
		}*/

		while (unvisited.Count > 0) {
			Tile currentCheapestTile = null;

			foreach (Tile possibleCheapestTile in unvisited) {
				if (currentCheapestTile == null || weights[possibleCheapestTile] < weights[currentCheapestTile]) {
					currentCheapestTile = possibleCheapestTile;
				}
			}

			if (currentCheapestTile == targetTile) {
				break;
			}

			unvisited.Remove(currentCheapestTile);

			foreach (Tile tile in currentCheapestTile.neighbors) {
				float route = weights[currentCheapestTile] + CostToEnterTile(tile);
				if (route < weights[tile]) {
					weights[tile] = route;
					pointers[tile] = currentCheapestTile;
				}
			}
		}

		// Either found shortest route or there is no route to the target.
		if (pointers[targetTile] == null) {
			return null; // No route between target and source verticies.
		}

		List<Tile> currentPath = new List<Tile>();

		Tile temp = targetTile;

		while (temp != null) {
			currentPath.Add(temp);
			temp = pointers[temp];
		}

		// Current path needs to be inverted because it describes path from target to source.
		currentPath.Reverse();
		return currentPath;
	}

	public bool CanSummonHere(Tile target) {
		bool result = false;
		if (TurnManager.instance.whoseTurn.cardToBeSummoned.spawnTiles.Contains(target)
			&& !TileIsOccupied(target)) {
			result = true;
		}
		return result; 
	}

	public bool TileIsOccupied(Tile target) {
		return tilesWithCreatures.ContainsKey(target);
	}

	public bool CreatureCanEnterTile(Tile target) {
		return GlobalSettings.instance.tileTypes[target.typeIndex].isWalkable && !TileIsOccupied(target);
	}

	public void AddCreatureToTile(CreatureLogic creatureLogic, Tile target) {
		if (!tilesWithCreatures.ContainsKey(target)) {
			tilesWithCreatures.Add(target, creatureLogic);
			creatureLogic.currentTile = target;
		} else {
			Debug.Log("Cannot Add a creature to Table at Tile: " + target);
		}
	}

	public void RemoveCreatureFromTile(Tile target) {
		if (tilesWithCreatures.ContainsKey(target)) {
			tilesWithCreatures.Remove(target);
		} else {
			Debug.Log("Cannot Remove a creature from Table at Tile: " + target);
		}
	}

	private void GeneratePathFindingGraph() {
		for (int y = 0; y < _height; y++) {
			for (int x = 0; x < _width; x++) {
				Tile currentTile = tableTiles[x, y];
				if (currentTile != null) {
					if (currentTile.x < _width - 1) {
						if (tableTiles[currentTile.x + 1, currentTile.y] != null) { // rightNeighbor
							currentTile.neighbors.Add(tableTiles[currentTile.x + 1, currentTile.y]);
						}
					}
					if (currentTile.x > 0) {
						if (tableTiles[currentTile.x - 1, currentTile.y] != null) { // leftNeighbor
							currentTile.neighbors.Add(tableTiles[currentTile.x - 1, currentTile.y]);
						}
					}

					if (currentTile.y % 2 == 0) { // Even row
						if (currentTile.y < _height - 1) {
							if (tableTiles[currentTile.x, currentTile.y + 1] != null) { // upperRightNeighbor
								currentTile.neighbors.Add(tableTiles[currentTile.x, currentTile.y + 1]);
							}

							if (currentTile.x > 0) {
								if (tableTiles[currentTile.x - 1, currentTile.y + 1] != null) { // upperLeftNeighbor
									currentTile.neighbors.Add(tableTiles[currentTile.x - 1, currentTile.y + 1]);
								}
							}
						}

						if (currentTile.y > 0) {
							if (tableTiles[currentTile.x, currentTile.y - 1] != null) { // lowerRightNeighbor
								currentTile.neighbors.Add(tableTiles[currentTile.x, currentTile.y - 1]);
							}

							if (currentTile.x > 0) {
								if (tableTiles[currentTile.x - 1, currentTile.y - 1] != null) { // lowerLeftNeighbor
									currentTile.neighbors.Add(tableTiles[currentTile.x - 1, currentTile.y - 1]);
								}
							}
						}
					} else { // Odd row
						if (currentTile.y < _height - 1) {
							if (tableTiles[currentTile.x, currentTile.y + 1] != null) { // upperLeftNeighbor
								currentTile.neighbors.Add(tableTiles[currentTile.x, currentTile.y + 1]);
							}

							if (currentTile.x < _width - 1) {
								if (tableTiles[currentTile.x + 1, currentTile.y + 1] != null) { // upperRightNeighbor
									currentTile.neighbors.Add(tableTiles[currentTile.x + 1, currentTile.y + 1]);
								}
							}
						}

						if (currentTile.y > 0) {
							if (tableTiles[currentTile.x, currentTile.y - 1] != null) { // lowerLeftNeighbor
								currentTile.neighbors.Add(tableTiles[currentTile.x, currentTile.y - 1]);
							}

							if (currentTile.x < _width - 1) {
								if (tableTiles[currentTile.x + 1, currentTile.y - 1] != null) { // lowerRightNeighbor
									currentTile.neighbors.Add(tableTiles[currentTile.x + 1, currentTile.y - 1]);
								}
							}
						}
					}
				}
			}
		}
	}

	private float CostToEnterTile(Tile target) {
		if (!CreatureCanEnterTile(target)) {
			return Mathf.Infinity;
		}
		float cost = GlobalSettings.instance.tileTypes[target.typeIndex].movementCost;

		return cost;
	}
}
