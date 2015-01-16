using UnityEngine;
using System.Collections;

//Base class for all blocks
public class BaseBlock : MonoBehaviour 
{
	public Behaviour behaviour;

	[HideInInspector]
	public int row,column;

	#region Editor

	public Direction direction;
	public tk2dSpriteAnimator animator;
	public tk2dSprite sprite;
	public BlockType type;

	#endregion
}
