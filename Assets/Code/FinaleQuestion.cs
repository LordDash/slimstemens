using System;

[Serializable]
public class FinaleQuestion : Question
{
    public string Question;
    public FinaleAnswer[] Answers;

    public int[] GetTimeRewards()
    {
        int[] timeRewards = new int[Answers.Length];

        for (int i = 0; i < Answers.Length; i++)
        {
            timeRewards[i] = Answers[i].TimeReward;
        }

        return timeRewards;
    }

    public string[] GetAnswers()
    {
        string[] answers = new string[Answers.Length];

        for (int i = 0; i < Answers.Length; i++)
        {
            answers[i] = Answers[i].Answer;
        }

        return answers;
    }
}