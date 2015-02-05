using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelStateManager
{
	LevelStates state;
	List<MonoBehaviour> recentlyProccesedAction;

	public LevelStateManager()
	{
		state = LevelStates.Idle;
		recentlyProccesedAction = new List<MonoBehaviour>();
	}

	public void BehaviourProcessed(MonoBehaviour behaviour)
	{
		if(state == LevelStates.Idle)
		{
			recentlyProccesedAction = new List<MonoBehaviour>();
			state = LevelStates.ProccessSteps;
			Game.Instanse.LevelEventManager.Broadcast(LevelEvents.NextStepProccesed);
		}
		else
		{
			if(!recentlyProccesedAction.Contains(behaviour))
			{
				recentlyProccesedAction.Add (behaviour);
			}
			if(recentlyProccesedAction.Count == LevelMain.Instanse.RegisteredBehaviours.Count)
			{
				state = LevelStates.Idle;
				Game.Instanse.LevelEventManager.Broadcast(LevelEvents.AllStepsProcessed);
			}
		}
	}
}
