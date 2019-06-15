using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCreatureManager : MonoBehaviour {

	public CardAsset cardAsset;

	public void ReadCreatureFromAsset(CardAsset cardAsset) {
		this.cardAsset = cardAsset;
	}

	private LineRenderer _lineRenderer;

	private void Awake() {
		// creatureState = CreatureState.Idle;
		_lineRenderer = GetComponentInChildren<LineRenderer>();
	}

	public void Move(CreatureLogic creatureLogic, List<Tile> currentPath) {
		if (creatureLogic.creatureState == CreatureState.Idle) {
			if (currentPath != null) {
				DrawPath(currentPath);
				Table.instance.RemoveCreatureFromTile(creatureLogic.currentTile);
				Table.instance.AddCreatureToTile(creatureLogic, currentPath[currentPath.Count - 1]);
				creatureLogic.creatureState = CreatureState.Moving;

				Sequence moveSequence = DOTween.Sequence();
				int currNode = 1;
				while (currNode < currentPath.Count) {

					Vector3 currentTarget = currentPath[currNode].transform.position;
					moveSequence.Append(transform.DOMove(currentTarget, 1));

					currNode++;
				}

				moveSequence.Play();
				moveSequence.OnComplete(() => {
					creatureLogic.currentTile = currentPath[currentPath.Count - 1];
					creatureLogic.creatureState = CreatureState.Idle;
					_lineRenderer.enabled = false;
				});
			}
		}
	}

	void DrawPath(List<Tile> currentPath) {
		if (currentPath != null) {
			_lineRenderer.positionCount = currentPath.Count;
			_lineRenderer.SetPosition(0, this.transform.position + new Vector3(0, 0.75f, 0));
			int currNode = 1;
			while (currNode < currentPath.Count) {
				Vector3 end = currentPath[currNode].transform.position + new Vector3(0, 0.75f, 0);

				_lineRenderer.SetPosition(currNode, end);

				currNode++;
			}

			Transform lastTile = currentPath[currNode - 1].gameObject.transform;
			Instantiate(GlobalSettings.instance.clickVisualPrefab, lastTile.position, Quaternion.identity);

			_lineRenderer.enabled = true;
			_lineRenderer.DOColor(new Color2(Color.white, Color.white), new Color2(Color.red, Color.red), 1);
			//_lineRenderer.DOColor(new Color2(Color.white, Color.white), new Color2(Color.red, Color.green), 1);
			//DOTween.ToAlpha(() => _lineRenderer.COLO)
		}
	}
}
