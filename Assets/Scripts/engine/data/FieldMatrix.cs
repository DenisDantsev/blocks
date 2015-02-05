using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FieldMatrix 
{
	public List<BaseBlock> blocks = new List<BaseBlock>();

	public void AddBlock(BaseBlock block)
	{
		blocks.Add(block);
	}

	public List<BaseBlock> GetBlocksByPredicat(Func<bool, BaseBlock> predicat)
	{
		return blocks.Where(p => predicat(p)).ToList();
	}

	public void Remove(BaseBlock block)
	{
		if(blocks.Contains(block))
		{
			blocks.Remove(block);
		}
	}

	public List<Vector2> EmptyIndexes()
	{
		List<Vector2> result = new List<Vector2>();
		int columns = LevelMain.Instanse.Level.columns;
		int row = LevelMain.Instanse.Level.rows;

		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < row; j++) {
				BaseBlock outBlock = new BaseBlock();
				if(!GetBlockByPosition(j, i, out outBlock))
				{
					result.Add(new Vector2(i,j));
				}
			}
				}
		return result;
	}

	public bool GetBlockByPosition(int row, int column, out BaseBlock block)
	{
		if(blocks.Count == 0)
		{
			block = null;
			return false;
		}
		else
		{
			block =  blocks.Find(p => p.column == column && p.row == row);
			return block != null;
		}
	}

	public List<BlockDomainModel> ToDomainsModel()
	{
		return null;
	}
}
