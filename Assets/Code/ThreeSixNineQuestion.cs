using System;

[Serializable]
public class ThreeSixNineQuestion : Question
{
    public string Question;
    public string Answer;
    public int TimeReward;

    public override string ToString()
    {
        return string.Format("{0} [A: {1}] [{2}]", Question, Answer, TimeReward);
    }
}