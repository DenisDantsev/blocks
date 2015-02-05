using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class BlockInfo
{
	public Direction direction;
	public BaseBlock prefab;
	public List<int> levels;
	public BlockType type;
	public BlockInfoAviability aviability;
}

public class BlockFactory :  Singleton<BlockFactory>, ISingleton
{
	public List<BlockInfo> BlockInfos = new List<BlockInfo>();

	public void Init()
	{

	}

	//Create an instanse
	public BaseBlock GetBlock(BlockType blockType, Vector3 size, Direction direction = Direction.None)
	{
			BlockInfo block = BlockInfos.Find(p => p.type == blockType && p.direction == direction);
			if(block != null)
			{
				GameObject instanse = Instantiate(block.prefab.gameObject) as GameObject;
				tk2dSprite sprite = instanse.GetComponent<tk2dSprite>();
				Bounds bounds = sprite.GetBounds();
				Vector3 newScale = new Vector3(size.x / bounds.size.x, size.y / bounds.size.y);
				sprite.scale = newScale;
				return instanse.GetComponent<BaseBlock>();
			}
			return null;
	}

	//Create new one using a balance file
	public BaseBlock GetNewRandomBlock()
	{
		//TODO
		return null;
	}

	public void DestroyBlock(BaseBlock block)
	{

	}
}
