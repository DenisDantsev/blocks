using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Interaction with user and draw graphic representation. Controll current state of a field
public class BaseFieldController : MonoBehaviour
{
	float topMargin = 100;
	float bottomMargin = 100;
	float leftMargin = -385;

	//Create graphics representation of a field

	public Vector2 BlockDemensions
	{
		get
		{
			Vector3 demensions = ScreenHelper.WorldDemensions();
			float widthOne = (demensions.x - leftMargin * 2) / LevelMain.Instanse.Level.columns;
			float heightOne = (demensions.y - bottomMargin - topMargin) / LevelMain.Instanse.Level.rows;

			return new Vector2(widthOne, heightOne);
		}
	}
	public Vector3 GetPositionToDemension(int row, int column)
	{
		Vector3 result = new Vector3();
		result.x = GetDemensionValue(column, false);
		result.y = GetDemensionValue(row, true);

		return result;
	}

	public float GetDemensionValue(int demension, bool isRow)
	{
		Vector2 demensions = BlockDemensions;
		if(isRow)
		{
			return bottomMargin + BlockDemensions.y * demension;
		}
		else
		{
			return leftMargin + BlockDemensions.x * demension;
		}
	}
	public List<BaseBlock> Neighbours(BaseBlock block)
	{
			List<BaseBlock> neighbours = new List<BaseBlock>();
			Stack<BaseBlock> stack = new Stack<BaseBlock>();
			stack.Push(block);
			while(stack.Count != 0)
			{
				BaseBlock pushedBlock = stack.Pop();
				neighbours.Add(pushedBlock);
				for (int row = pushedBlock.row - 1; row <= pushedBlock.row + 1; row++) 
				{
					for (int col = pushedBlock.column -1; col <= pushedBlock.column + 1; col++) 
					{
						BaseBlock newBlock = null;
						if(LevelMain.Instanse.Field.GetBlockByPosition(row, col, out newBlock))
						{
							stack.Push(newBlock);
						}
					}
				}
			}
		return neighbours;
	}

	public virtual void CreateField()
	{
		List<BlockDomainModel> blocks = LevelMain.Instanse.Level.Blocks;
		float widthOne, heightOne;
		Vector3 demensions = BlockDemensions;

		widthOne = demensions.x;
		heightOne = demensions.y;

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

	public Vector3 GetNexPosition(Direction dir, Vector3 current)
	{
		Vector3 demensions = BlockDemensions;
		Vector3 newPos = Vector3.zero;
		switch (dir)
		{
			case Direction.Left:
				newPos = new Vector3(current.x - demensions.x, current.y);
			break;
		case Direction.Bottom:
			newPos = new Vector3(current.x, current.y - demensions.y);
			break;
		case Direction.Top:
			newPos = new Vector3(current.x, current.y + demensions.y);
			break;
		case Direction.Right:
			newPos = new Vector3(current.x + demensions.x, current.y);
		break;
		}
		return newPos;
	}

	public virtual void OnFingerUp(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.FingerUp, target);
	}

	public virtual void OnFingerDown(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.FingerDown, target);
	}

	public virtual void OnClick(tk2dUIItem item)
	{
		BaseBlock target = item.gameObject.GetComponent<BaseBlock>();
		Game.Instanse.UserEventManager.Broadcast<BaseBlock>(UserEvent.Click, target);
	}
}
