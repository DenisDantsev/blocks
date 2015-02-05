using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour {

	protected void OnExecuted()
	{
		Game.Instanse.LevelStateManager.BehaviourProcessed(this);
	}
}

