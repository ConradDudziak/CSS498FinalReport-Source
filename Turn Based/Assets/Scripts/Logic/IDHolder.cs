﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDHolder : MonoBehaviour {

	public int uniqueID;
	private static List<IDHolder> allIDHolders = new List<IDHolder>();

	private void Awake() {
		allIDHolders.Add(this);
	}

	public static GameObject GetGameObjectWithID(int ID) {
		foreach (IDHolder i in allIDHolders) {
			if (i.uniqueID == ID)
				return i.gameObject;
		}
		return null;
	}

	public static void ClearIDHoldersList() {
		allIDHolders.Clear();
	}
}
