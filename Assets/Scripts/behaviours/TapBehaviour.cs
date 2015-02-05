using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening;
using System.Collections;

//Simple match 3 strategy
public class TapBehaviour : BaseBehaviour
{
	void OnEnable()
	{
		Game.Instanse.UserEventManager.AddListener<BaseBlock>(UserEvent.Click, Check);
	}

	void OnDisable()
	{
		if(Game.Instanse.UserEventManager.HasListener<BaseBlock>(UserEvent.Click, Check))
		{
			Game.Instanse.UserEventManager.RemoveListener<BaseBlock>(UserEvent.Click, Check);
		}
	}

	//Check neightbours using A*
	public void Check(BaseBlock block)
	{
		if(block.type == BlockType.Tap)
		{
			List<BaseBlock> neighbours = LevelMain.Instanse.FieldController.Neighbours(block);

			if(neighbours.Count < 3)
			{
				StartCoroutine(ShowMiss(neighbours));
			}
			else
			{
				StartCoroutine(Destroy(neighbours, block.direction));
			}
		}
	}

	//If count of neighbours less than 3
	IEnumerator ShowMiss(List<BaseBlock> blocks)
	{
		Tween tween = null;
		for (int i = 0; i < blocks.Count; i++) 
		{
			tk2dSprite sprite = blocks[i].gameObject.GetComponent<tk2dSprite>();
			Color baseColor = sprite.color;
			tween = DOTween.To(() => sprite.color, (c) => sprite.color = c, (baseColor + Color.grey) / 2, 1.0f);
		}

		yield return tween.WaitForCompletion ();
		blocks.ForEach(p => p.gameObject.GetComponent<tk2dSprite>().color = Color.white);
	}

	//Destroy blocks
	IEnumerator Destroy(List<BaseBlock> blocks, Direction dir)
	{
		Tween tween = null;
		for (int i = 0; i < blocks.Count; i++) 
		{
			tk2dSprite sprite = blocks[i].gameObject.GetComponent<tk2dSprite>();
			Color baseColor = sprite.color;
			Color tranparentBase = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
			tween = DOTween.To(() => sprite.color, (c) => sprite.color = c,  tranparentBase, 1.0f);
		}
		
		yield return tween.WaitForCompletion ();
		while(blocks.Count >= 0)
		{
			Game.Instanse.BlockEventManager.Broadcast<BaseBlock>(BlockEvent.Ban, blocks[0]);
			MonoBehaviour.Destroy(blocks[0].gameObject);
			LevelMain.Instanse.Field.Remove(blocks[0]);
			blocks.RemoveAt(0);
		}

		OnExecuted();
	}
}


