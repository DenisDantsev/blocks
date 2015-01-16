using UnityEngine;
using System.Collections;

public static class ScreenHelper
{
	public static Rect WorldScreenRect()
	{
		Camera camera = Camera.main;

		Vector3 leftBottom = camera.ViewportToWorldPoint(Vector3.zero);
		Vector3 leftTop = camera.ViewportToWorldPoint(new Vector3(0, 1));
		Vector3 rightBottom = camera.ViewportToWorldPoint(new Vector3(1, 0));
		Vector3 rightTop = camera.ViewportToWorldPoint(new Vector3(1,1));

		Rect rect =  new Rect();
		rect.xMin = leftBottom.x;
		rect.xMax = rightTop.x;
		rect.yMin = leftBottom.y;
		rect.yMax = rightTop.y;

		return rect;
	}

	//World height and width
	public static Vector3 WorldDemensions()
	{
		Rect rect = ScreenHelper.WorldScreenRect;
		return new Vector3(rect.width, rect.height);
	}

	public static Vector3 WorldCenter()
	{
		Rect rect = ScreenHelper.WorldScreenRect;
		Vector3 Demensions = ScreenHelper.WorldDemensions;
		return new Vector3(rect.xMin + Demensions.x / 2, rect.yMin + Demensions.y / 2);
	}
}
