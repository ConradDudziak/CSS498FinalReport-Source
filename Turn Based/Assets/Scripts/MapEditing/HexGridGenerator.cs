using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour {

	private int _width;
	private int _height;

	[SerializeField]
	private float _xOffset = 1.732f;
	[SerializeField]
	private float _zOffset = 1.5f;

	private Stack<Tile> _deletedTiles;
	private Stack<Vector3> _deletedTilePositions;

	void Start () {
		_deletedTiles = new Stack<Tile>();
		_deletedTilePositions = new Stack<Vector3>();
		_width = GlobalSettings.instance.maxWidth;
		_height = GlobalSettings.instance.maxHeight;
		GenerateMapVisual();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Z) && _deletedTiles.Count > 0) {
			Tile deletedTile = _deletedTiles.Pop();
			MakeTile(deletedTile, _deletedTilePositions.Pop());
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		// Does ray hit something? Fill hit info.
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject hitObject = hitInfo.collider.transform.parent.gameObject;
			Tile hitTile = hitObject.GetComponentInParent<Tile>();

			if (Input.GetMouseButtonDown(0)) {
				ChangeTile(hitTile);
			}

			if (Input.GetMouseButtonDown(1)) {
				_deletedTiles.Push(hitTile);
				_deletedTilePositions.Push(hitObject.transform.position);
				Destroy(hitObject);
			}

		}
	}

	void GenerateMapVisual() {
		for (int y = 0; y < _height; y++) {
			for (int x = 0; x < _width; x++) {
				float xPos = x * _xOffset;

				// Is odd row?
				if (y % 2 != 0) {
					xPos += _xOffset / 2f;
				}

				MakeTile(x, y, 0, new Vector3(xPos, 0, y * _zOffset));
			}
		}
	}

	void ChangeTile(Tile tile) {
		int typeIndex = tile.typeIndex;
		int nextIndex = typeIndex + 1;
		if (nextIndex >= GlobalSettings.instance.tileTypes.Length) {
			nextIndex = 0;
		}

		MakeTile(tile, nextIndex, tile.gameObject.transform.position);
		Destroy(tile.gameObject);
	}

	void MakeTile(Tile otherTile, Vector3 position) {
		GameObject tileGameObject = (GameObject)Instantiate(GlobalSettings.instance.tileTypes[otherTile.typeIndex].tilePrefab, position, Quaternion.identity);
		Tile tile = tileGameObject.GetComponent<Tile>();
		tile.x = otherTile.x;
		tile.y = otherTile.y;
		tile.typeIndex = otherTile.typeIndex;
		tileGameObject.transform.SetParent(this.transform);
	}

	void MakeTile(Tile otherTile, int tileTypeIndex, Vector3 position) {
		GameObject tileGameObject = (GameObject)Instantiate(GlobalSettings.instance.tileTypes[tileTypeIndex].tilePrefab, position, Quaternion.identity);
		Tile tile = tileGameObject.GetComponent<Tile>();
		tile.x = otherTile.x;
		tile.y = otherTile.y;
		tile.typeIndex = tileTypeIndex;
		tileGameObject.transform.SetParent(this.transform);
	}

	void MakeTile(int x, int y, int tileTypeIndex, Vector3 position) {
		GameObject tileGameObject = (GameObject)Instantiate(GlobalSettings.instance.tileTypes[tileTypeIndex].tilePrefab, position, Quaternion.identity);
		Tile tile = tileGameObject.GetComponent<Tile>();
		tile.x = x;
		tile.y = y;
		tile.typeIndex = tileTypeIndex;
		tileGameObject.transform.SetParent(this.transform);
	}
}
