using System;
using UnityEngine;

//The main level scripts. Contains all behaviour scripts and models.

public class LevelMain : Singleton<LevelMain>, ISingleton
{
	public BaseFieldController FieldController;
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
		AddComponentToSingleton<TapBehaviour>();
		FieldController = AddComponentToSingleton<BaseFieldController>();
	}

	public void CheckLevelSucces()
	{

	}
}

