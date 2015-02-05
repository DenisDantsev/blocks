using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions  {

	public static List<T> Clone<T>(this List<T> member) 
	{
		List<T> result = new List<T>();
		for (int i = 0; i < member.Count; i++) {
			result.Add (member[i]);
				}
		return result;
	}
}
