using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Interaction with user and draw graphic representation. Controll current state of a field
public class BaseFieldController : MonoBehaviour
{
	float topMargin = 200,
	bottomMargin = 200,
	leftMargin = 200;

	//Create graphics representation of a field
	public virtual void CreateField()
	{
		List<BlockDomainModel> blocks = LevelMain.Instanse.Level.Blocks;
		Vector3 demensions = ScreenHelper.WorldDemensions;

		float widthOne = (demensions.x - leftMargin * 2) / LevelMain.Instanse.Level.columns;
		float heightOne = (demensions.y - bottomMargin - topMargin) / LevelMain.Instanse.Level.rows;
		foreach (var item in blocks) 
		{
			BlockType blockType = (BlockType)Enum.Parse(typeof(BlockType), item.type);
			Direction direction = (Direction)Enum.Parse(typeof(Direction), item.direction);
			Vector3 blockSize = new Vector3(widthOne, heightOne);
			var block = Game.Instanse.factory.GetBlock(blockType, blockSize, direction);
			tk2dUIItem blockUIItem = block.gameObject.GetComponent<tk2dUIItem>();

			blockUIItem.OnClickUIItem += OnClick;
			blockUIItem.OnDownUIItem += OnFingerDown;
			blockUIItem.OnUpUIItem += OnFingerUp;

			LevelMain.Instanse.Field.AddBlock(block);

			float xPos = leftMargin + widthOne * block.column;
			float yPos = bottomMargin + heightOne * block.row;

			block.gameObject.transform.localPosition = new Vector3(xPos, yPos);
		}
	}

	public virtual void OnFingerUp(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.FingerUp);
	}

	public virtual void OnFingerDown(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.FingerDown);
	}

	public virtual void OnClick(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.Click);
	}
}
