using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FieldMatrix 
{
	List<BaseBlock> blocks = new List<BaseBlock>();

	public void AddBlock(BaseBlock block)
	{
		blocks.Add(block);
	}

	public void Remove(BaseBlock block)
	{
		if(blocks.Contains(block))
		{
			blocks.Remove(block);
		}
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
