using UnityEngine;
using System.Collections;
using System;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour, ISingleton
{
	static T _instanse;
	static GameObject go;
	static object _lock = new object();
	public static T Instanse
	{
		get
		{
			if(_instanse != null)
				return _instanse;

			lock(_lock)
			{
				if(FindObjectsOfType(typeof(T)).Length != 0)
				{
					_instanse = FindObjectOfType<T>();
					return _instanse;
				}
				_instanse = Create ();
				return _instanse;
			}
		}
	}

	public static M AddComponentToSingleton<M>() where M : MonoBehaviour
	{
		var comp = Instanse;
		var objects = FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < objects.Length; i++) 
		{
			GameObject objGO = objects[i] as GameObject;
			if(objGO.GetComponent<T>() != null)
			{
				if(objGO.GetComponent<M>() == null)
				{
					objGO.AddComponent<M>();
				}
				return objGO.GetComponent<M>();
			}
		}

		return null;
	}

	static T Create()
	{
		string name = (typeof(T)).ToString();
		go = new GameObject();
		go.AddComponent<T>();
		DontDestroyOnLoad(go);

		T instan = go.GetComponent<T>();
		instan.Init();

		return instan;
	}
}
