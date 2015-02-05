using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Main class
public class Game :  Singleton<Game> , ISingleton
{
	public ConfigLoader ConfigLoader;
	public LevelStateManager LevelStateManager;
	public ProgressLoader ProgressLoader;
	public BlockFactory factory;
	public AssetManager AssetManager;
	public List<LevelModel> Levels;
	public UserModel User;

	public EventManager<LevelEvents> LevelEventManager;
	public EventManager<UserEvent> UserEventManager;
	public EventManager<BlockEvent> BlockEventManager;

	int _goLevel;

	void Awake()
	{
		Init ();
	}

	public void Init()
	{
		LevelStateManager = new LevelStateManager();
		UserEventManager = new EventManager<UserEvent>();
		LevelEventManager = new EventManager<LevelEvents>();
		BlockEventManager = new EventManager<BlockEvent>();
		ConfigLoader = new ConfigLoader();
		ProgressLoader = new ProgressLoader();
		AssetManager = new AssetManager();
		User = new UserModel();
	}

	public void LoadLevel(int levelNumber)
	{
		if(!ConfigLoader.IsLoaded(levelNumber))
		{
			ConfigLoader.Load(levelNumber);
		}
		if(AssetManager.GetProgress(levelNumber + "") != 1f)
		{
			AssetManager.LoadAsset(levelNumber + "", AssetPriority.Instantly);
			StartCoroutine(ShowPreloader(levelNumber));
		}
		else
		{
			goLevel(levelNumber);
		}
	}

	IEnumerator ShowPreloader(int levelNumber)
	{
		Application.LoadLevel("Preloader");
		while(AssetManager.GetProgress(levelNumber + "") != 1f)
		{
			yield return new WaitForEndOfFrame();
		}
		goLevel(levelNumber);
	}

	void goLevel(int levelNumber)
	{
		Application.LoadLevel("Level");
		LevelMain.Instanse.LoadLevel(levelNumber);
	}

	public void LoadMainMenu(int levelNumber)
	{
		Application.LoadLevel("MainMenu");
	}	
}
