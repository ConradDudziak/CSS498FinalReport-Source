using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDFactory {

	private static int count;

	public static int GetUniqueID() {
		count++;
		return count;
	}

	public static void ResetIDs() {
		count = 0;
	}
}
