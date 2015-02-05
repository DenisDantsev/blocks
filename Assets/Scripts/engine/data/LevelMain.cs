using System;
using UnityEngine;
using System.Collections.Generic;
//The main level scripts. Contains all behaviour scripts and models.

public class LevelMain : Singleton<LevelMain>, ISingleton
{
	public BaseFieldController FieldController;
	public List<MonoBehaviour> RegisteredBehaviours;
	public SloughingController SloughtController;
	public FieldMatrix Field;
	public LevelModel Level;

	public void LoadLevel(int leveNumber)
	{
		Level = Game.Instanse.ConfigLoader.Load(leveNumber);
		Field = Game.Instanse.ProgressLoader.LoadProgress(leveNumber);
	}

	public void Init()
	{
		RegisteredBehaviours = new List<MonoBehaviour>();
		RegisteredBehaviours.Add (AddComponentToSingleton<TapBehaviour>());

		FieldController = AddComponentToSingleton<BaseFieldController>();
	}

	public void CheckLevelSucces()
	{

	}
}

