using UnityEngine;
using System.Collections;

//Base class for all blocks
public class BaseBlock : MonoBehaviour 
{
	[HideInInspector]
	public int row,column;

	#region Editor

	public Direction direction;
	public Animator animator;
	public tk2dSprite sprite;
	public BlockType type;

	#endregion
}
