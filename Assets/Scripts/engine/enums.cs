
public enum AssetPriority
{
	Instantly,
	Background
}

public enum BlockType
{
	Lazer,
	Bomb,
	Teleport,
	Tap
}

public enum Direction
{
	Top,
	Left,
	Bottom,
	Right,
	None
}

public enum UserEvent
{
	Click,
	FingerDown,
	FingerUp
}

public enum LevelEvents
{
	NextStepProccesed,
	AllStepsProcessed
}

public enum LevelStates
{
	Idle,
	ProccessSteps,
}

public enum BlockEvent
{
	Ban,
	Move,
	Appear
}

public enum BlockInfoAviability
{
	Total,
	Custom
}