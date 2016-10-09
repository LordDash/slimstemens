using System;
using System.Collections.Generic;

[Serializable]
public class PuzzleQuestion : Question
{
    public PuzzleAnswer[] Answers;

    private int[][] _shuffledWordIndeces = null;
    private string[] _shuffledWords = null;

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

    public string[] GetWords()
    {
        if (_shuffledWords == null)
        {
            _shuffledWords = new string[Answers.Length * Answers[0].Words.Length];
            _shuffledWordIndeces = new int[Answers.Length][];

            List<int> unusedIndeces = new List<int>(_shuffledWords.Length);

            for (int i = 0; i < _shuffledWords.Length; i++)
            {
                unusedIndeces.Add(i);
            }

            for (int answerIndex = 0; answerIndex < Answers.Length; answerIndex++)
            {
                PuzzleAnswer answer = Answers[answerIndex];

                _shuffledWordIndeces[answerIndex] = new int[answer.Words.Length];

                for (int wordIndex = 0; wordIndex < answer.Words.Length; wordIndex++)
                {
                    int index = unusedIndeces[UnityEngine.Random.Range(0, unusedIndeces.Count)];
                    _shuffledWords[index] = answer.Words[wordIndex];
                    _shuffledWordIndeces[answerIndex][wordIndex] = index;
                    unusedIndeces.Remove(index);
                }
            }
        }

        return _shuffledWords;
    }

    public int[] GetAnswerWordIndeces(int answerIndex)
    {
        return _shuffledWordIndeces[answerIndex];
    }
}