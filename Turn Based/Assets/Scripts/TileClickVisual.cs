using DG.Tweening;
using UnityEngine;

public class TileClickVisual : MonoBehaviour {

	public SpriteRenderer targetSprite;

	// Use this for initialization
	void Start () {

		Sequence fadeSequence = DOTween.Sequence();
		fadeSequence.Append(DOTween.ToAlpha(() => targetSprite.color, x => targetSprite.color = x, 0, 2));
		fadeSequence.OnComplete(() => {
			Destroy(this.gameObject);
		});
	}
}
