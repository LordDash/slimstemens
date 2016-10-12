using System;

[Serializable]
public class CollectiveMemoryQuestion : Question
{
	public string QuestionFileName;
	public string[] Answers;
	public int[] TimeRewards;
	
	private int _currentTimeRewardIndex = -1;
	
	public int GetNextTimeReward()
	{
		return TimeRewards[++_currentTimeRewardIndex];
	}
}