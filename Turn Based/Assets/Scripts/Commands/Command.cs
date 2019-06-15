using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command {

	public static Queue<Command> CommandQueue = new Queue<Command>();
	public static bool playingQueue = false;

	public virtual void AddToQueue() {
		CommandQueue.Enqueue(this);
		if (!playingQueue) {
			PlayNextCommandFromQueue();
		}
	}

	public virtual void StartCommandExecution() {
		CommandExecutionComplete();
	}

	public static void CommandExecutionComplete() {
		if (CommandQueue.Count > 0) {
			PlayNextCommandFromQueue();
		} else {
			playingQueue = false;
		}
	}

	public static void PlayNextCommandFromQueue() {
		playingQueue = true;
		CommandQueue.Dequeue().StartCommandExecution();
	}
}
