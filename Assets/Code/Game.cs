using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Game
{
    public List<Round> GameRounds;
    public List<string> SerializedRoundQuestions;

    private List<object> _roundQuestionsContainers;
    private Dictionary<int, Question[]> _roundQuestions;

    public Game()
    {
        GameRounds = new List<Round>();
        SerializedRoundQuestions = new List<string>();
        _roundQuestionsContainers = new List<object>();
        _roundQuestions = new Dictionary<int, Question[]>();
    }

    public void RemoveRound(int roundIndex)
    {
        GameRounds.RemoveAt(roundIndex);
        SerializedRoundQuestions.RemoveAt(roundIndex);
    }

    public void AddRound<T>(Round round, QuestionContainer<T> questions) where T : Question
    {
        GameRounds.Add(round);

        if (questions != null)
        {
            SerializedRoundQuestions.Add(JsonUtility.ToJson(questions));
        }
        else
        {
            SerializedRoundQuestions.Add(null);
        }
    }

    public Round GetRound(int roundIndex)
    {
        if (GameRounds.Count <= roundIndex)
        {
            return default(Round);
        }

        return GameRounds[roundIndex];
    }

    public T[] GetQuestionsForRound<T>(int roundIndex) where T : Question
    {
        if (SerializedRoundQuestions.Count <= roundIndex)
        {
            return null;
        }

        if (_roundQuestionsContainers == null)
        {
            _roundQuestionsContainers = new List<object>();
        }

        Question[] questions;

        if (_roundQuestions.TryGetValue(roundIndex, out questions))
        {
            return questions as T[];
        }

        string serializedQuestions = SerializedRoundQuestions[roundIndex];

        if (serializedQuestions == null)
        {
            return null;
        }

        QuestionContainer<T> questionContainer = JsonUtility.FromJson<QuestionContainer<T>>(serializedQuestions);

        while (_roundQuestionsContainers.Count <= roundIndex)
        {
            _roundQuestionsContainers.Add(null);
        }

        _roundQuestionsContainers[roundIndex] = questionContainer;
        _roundQuestions[roundIndex] = questionContainer.Questions;

        return questionContainer.Questions;
    }

    public string Save()
    {
        for (int i = 0; i < SerializedRoundQuestions.Count; i++)
        {
            if (SerializedRoundQuestions[i] != null && _roundQuestionsContainers[i] != null)
            {
                SerializedRoundQuestions[i] = JsonUtility.ToJson(_roundQuestionsContainers[i]);
            }
        }

        return JsonUtility.ToJson(this);
    }
}