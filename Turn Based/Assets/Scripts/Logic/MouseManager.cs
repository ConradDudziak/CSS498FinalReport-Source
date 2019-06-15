using UnityEngine;

public class MouseManager : MonoBehaviour {

	[SerializeField]
	private Camera mainCamera;

	// private CreatureLogic _selectedCreatureLogic;
	
	// Update is called once per frame
	void Update () {
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		// Does ray hit something? Fill hitInfo.
		if (Physics.Raycast(ray, out hitInfo)) {
			IDHolder hitIDHolder = hitInfo.collider.transform.parent.gameObject.GetComponent<IDHolder>();

			if (hitIDHolder == null) {
				Debug.Log("Mouse Over Something Without ID");
			} else {
				TableVisual tableVisual = TurnManager.instance.whoseTurn.playerVisual.tableVisual;

				GameObject result = null;
				// Check who this ID belongs to
				if (tableVisual.tileGameObjectsByID.TryGetValue(hitIDHolder.uniqueID, out result)) { // ID belongs to a Tile
					MouseOverTile(result.GetComponent<Tile>());
				} else if (tableVisual.creatureGameObjectsByID.TryGetValue(hitIDHolder.uniqueID, out result)) { // ID belongs to a Creature
					MouseOverCreature(hitIDHolder.uniqueID);
				}
			}
		}

		if (Input.GetMouseButtonDown(1) && GlobalSettings.instance.CanControlThisPlayer(TurnManager.instance.whoseTurn)) {
			TurnManager.instance.whoseTurn.cardToBeSummoned = null;
		}
	}

	void MouseOverTile(Tile tile) {
		Player currentPlayer = TurnManager.instance.whoseTurn;

		if (GlobalSettings.instance.CanControlThisPlayer(currentPlayer)) {
			if (Input.GetMouseButtonDown(0) && currentPlayer.playerState == PlayerState.Summoning) {
				if (Table.instance.CanSummonHere(tile)) {
					currentPlayer.SummonACreature(currentPlayer.cardToBeSummoned, tile);
				}
			} else if (Input.GetMouseButtonDown(0) && currentPlayer.playerState == PlayerState.CreatureSelect) {
				currentPlayer.MoveCurrentCreature(tile);
			}
		}
	}

	void MouseOverCreature(int uniqueID) {
		Player currentPlayer = TurnManager.instance.whoseTurn;

		if (Input.GetMouseButtonDown(0) && GlobalSettings.instance.CanControlThisPlayer(currentPlayer)) {
			CreatureLogic clickedCreature = CreatureLogic.creaturesCreatedThisGame[uniqueID];
			if (clickedCreature.owner == currentPlayer) {
				currentPlayer.SelectCreature(clickedCreature);
			}
		}
	}
}
