using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	#region Singleton
	public static MessageManager Instance;

	void Awake() {
		Instance = this;
		MessagePanel.SetActive(false);
	}
	#endregion

	public Text MessageText;
	public GameObject MessagePanel;

	public void ShowMessage(string Message, float Duration) {
		StartCoroutine(ShowMessageCoroutine(Message, Duration));
	}

	IEnumerator ShowMessageCoroutine(string Message, float Duration) {
		//Debug.Log("Showing some message. Duration: " + Duration);
		MessageText.text = Message;
		MessagePanel.SetActive(true);

		yield return new WaitForSeconds(Duration);

		MessagePanel.SetActive(false);
		Command.CommandExecutionComplete();
	}

	// Test Purposes only
	void Update() {
		/*
		if (Input.GetKeyDown(KeyCode.Y))
			ShowMessage("Your Turn", 3f);
		if (Input.GetKeyDown(KeyCode.E))
			ShowMessage("Enemy Turn", 3f);
		*/
	}
}
