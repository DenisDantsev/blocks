using System;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using DG.Tweening;
using System.Linq;

//Controll sloght of blocks
public class SloughingController : MonoBehaviour
{ 
	public float tweenSpeed = 0.5f;
	private BaseBlock catchedBlock;
	private bool sloughtNeedProcess;

	void OnEnable()
	{
		Game.Instanse.UserEventManager.AddListener<BaseBlock>(UserEvent.FingerDown, OnTap);
		Game.Instanse.LevelEventManager.AddListener(LevelEvents.AllStepsProcessed, OnAllStepsProccesed);

	}

	void OnDisable()
	{
		if(Game.Instanse.UserEventManager.HasListener<BaseBlock>(UserEvent.FingerDown, OnTap))
		{
			Game.Instanse.UserEventManager.RemoveListener<BaseBlock>(UserEvent.FingerDown, OnTap);
		}
		if(Game.Instanse.LevelEventManager.HasListener(LevelEvents.AllStepsProcessed, OnAllStepsProccesed))
		{
			Game.Instanse.LevelEventManager.RemoveListener(LevelEvents.AllStepsProcessed, OnAllStepsProccesed);
		}
	}

	private void OnTap(BaseBlock block)
	{
		if(!sloughtNeedProcess && block.type == BlockType.Tap)
		{
			catchedBlock = block;
			sloughtNeedProcess = true;
		}

	}

	private void OnAllStepsProccesed()
	{
		if(!sloughtNeedProcess)
		{
			return;
		}

		ProcessSlought(catchedBlock.direction);
	}

	private void ProcessSlought(Direction direction)
	{
		List<Vector2> emptyEntries = LevelMain.Instanse.Field.EmptyIndexes();
		List<Vector2> buffer = emptyEntries.Clone<Vector2>();

		List<BaseBlock> targets = new List<BaseBlock>();
		List<BaseBlock> newBlocks = new List<BaseBlock>();

		//Гавнокод
		while(buffer.Count > 0)
		{
			Vector3 startBlockPosition = Vector3.zero;
			Func<BaseBlock, bool> linePredicat;
			Func<BaseBlock, bool> directionPredicat;
			List<Vector2> extrasBuffer = new List<Vector2>();
			Vector3 firstEnd, secondEnd;
			bool hasEdge;

			BaseFieldController fieldController = LevelMain.Instanse.FieldController;
			LevelModel levelModel = LevelMain.Instanse.Level;

			if(direction == Direction.Left || direction == Direction.Right)
			{
				float row = buffer[0].x;
				float demensionValue =  LevelMain.Instanse.FieldController.GetDemensionValue((int)row, true);
				linePredicat = (p) => p.row == row;
				extrasBuffer = buffer.Where(p => p.x == row).ToList();

				int minIndexEmpty, maxIndexEmpty;
				minIndexEmpty = (int)extrasBuffer.Min(p => p.x);
				maxIndexEmpty = (int)extrasBuffer.Max(p => p.x);

				if(direction == Direction.Right)
				{
					startBlockPosition = new Vector3(-2000, demensionValue);
					directionPredicat = (p) => p.column < buffer[0].y;
					hasEdge = minIndexEmpty != 0;
					firstEnd = fieldController.GetPositionToDemension((int)row, 0);
					secondEnd = fieldController.GetPositionToDemension((int)row, maxIndexEmpty);
				}
				else
				{
					startBlockPosition = new Vector3(2000, demensionValue);
					directionPredicat = p => p.column > buffer[0].y;
					hasEdge = maxIndexEmpty != levelModel.columns - 1;
					firstEnd = fieldController.GetPositionToDemension((int)row, levelModel.columns - 1);
					secondEnd = fieldController.GetPositionToDemension((int)row, minIndexEmpty);
				}
			}
			else 
			{
				int column = (int)buffer[0].y;
				linePredicat = (p) => p.column == column;
				float demensionValue = LevelMain.Instanse.FieldController.GetDemensionValue(column, false);
				
				int minIndexEmpty, maxIndexEmpty;
				minIndexEmpty = (int)extrasBuffer.Min(p => p.y);
				maxIndexEmpty = (int)extrasBuffer.Max(p => p.y);

				extrasBuffer = buffer.Where(p => p.y == column).ToList();

				if(direction == Direction.Top)
				{
					startBlockPosition = new Vector3(demensionValue, -2000);
					directionPredicat = p => p.row > buffer[0].y;
					hasEdge = maxIndexEmpty != levelModel.rows - 1;
					firstEnd = fieldController.GetPositionToDemension(levelModel.rows - 1, column);
					secondEnd = fieldController.GetPositionToDemension(maxIndexEmpty, column);
				}
				else
				{
					startBlockPosition =  new Vector3(demensionValue, 2000);
					directionPredicat = p => p.row < buffer[0].y;
					hasEdge = maxIndexEmpty != 0;
					firstEnd = fieldController.GetPositionToDemension(0, column);
					secondEnd = fieldController.GetPositionToDemension(minIndexEmpty, column);
				}
			}

			targets.Concat(LevelMain.Instanse.Field.blocks.Where(p => linePredicat(p) && directionPredicat(p)));

			//Cоздаем новые блоки
			for (int i = 0; i < extrasBuffer.Count; i++) {
				BaseBlock newBlock = Game.Instanse.factory.GetNewRandomBlock();
				Vector3 nextPosition = LevelMain.Instanse.FieldController.GetNexPosition(direction, startBlockPosition);
				startBlockPosition = nextPosition;
				newBlock.transform.localPosition = nextPosition;
				newBlocks.Add (newBlock);
			}

			//Удаляем из буффера
			while(extrasBuffer.Count != 0)
			{ 
				int extraIndex = buffer.IndexOf(extrasBuffer[0]);
				buffer.RemoveAt(extraIndex);
				extrasBuffer.RemoveAt(0);
			}
		}

	}

	IEnumerator AnimateSlought(bool hasEdge, Direction dir, Vector3 firstEnd, Vector3 secondEnd, List<BaseBlock> newBlocks, List<BaseBlock> existBlocks)
	{
		Vector2 blockDemensions = LevelMain.Instanse.FieldController.BlockDemensions;
		if(dir == Direction.Left || dir == Direction.Right)
		{
			newBlocks = newBlocks.OrderBy(p => p.transform.position.x).ToList();
		}
		else
		{
			newBlocks = newBlocks.OrderBy(p => p.transform.position.y).ToList();
		}

		if(dir == Direction.Right)
		{
			if(!hasEdge)
			{
				for (int i = 0; i < newBlocks.Count; i++) {
					int leftToEnd = newBlocks.Count - i - 1;
					float finishX = secondEnd.x - leftToEnd * blockDemensions.x;
					newBlocks[i].transform.DOMoveX(finishX, tweenSpeed);
				}
			}
			else
			{
				Tween lastAnimation = null;
				for (int i = 0; i < newBlocks.Count; i++) {
					int leftToEnd = newBlocks.Count - i - 1;
					float finishX = firstEnd.x - leftToEnd * blockDemensions.x;
					lastAnimation = newBlocks[i].transform.DOMoveX(finishX, tweenSpeed/2);
				}
				yield return lastAnimation.WaitForCompletion();
				var wholeBlock = newBlocks.Concat(existBlocks).OrderBy(p => p.transform.position.x).ToList();
				for (int i = 0; i < wholeBlock.Count; i++) {
					int leftToEnd = newBlocks.Count - i - 1;
					float finishX = secondEnd.x - leftToEnd * blockDemensions.x;
					wholeBlock[i].transform.DOMoveX(finishX, tweenSpeed / 2);
				}
			}
		}
		else if(dir == Direction.Left)
		{
			//TODO
		}
		else if(dir == Direction.Right)
		{
			//TODO
		}
		else if(dir == Direction.Bottom)
		{
			//TODO
		}
	}

}


