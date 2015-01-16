using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AssetManager : Singleton<AssetManager>, ISingleton
{
	Dictionary<string, AssetBundle> Assets;
	Dictionary<string, Progress> AssetProgress;

	Queue<string> assetsToDownload;

	public void Init()
	{

	}
	public bool GetAsset(string id, out AssetBundle asset)
	{
		asset = null;
		return false;
	}

	public float GetProgress(string id)
	{
		if(!AssetProgress.ContainsKey(id)) return 0f;
		return AssetProgress[id].percentDownload;
	}

	public void LoadAsset(string id, AssetPriority priority)
	{

	}

	void LoadFromCache()
	{

	}

	public class Progress
	{
		public string assetID;
		public float percentDownload;
	}
}
