using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRound : GameRound
{
    private class PuzzleQuestion
    {
        public PuzzleAnswer[] Answers;
    }

    private class PuzzleAnswer
    {
        public string[] Words;
        public string Answer;
        public int TimeReward;
    }

    private static readonly PuzzleQuestion[] Questions = new PuzzleQuestion[]
        {
            new PuzzleQuestion() { Answers =  new[] 
                    {
                        new PuzzleAnswer() { Answer = "Antwoord 1", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 1",
                                    "Woord 2",
                                    "Woord 3",
                                    "Woord 4",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 2", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 5",
                                    "Woord 6",
                                    "Woord 7",
                                    "Woord 8",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 3", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 9",
                                    "Woord 10",
                                    "Woord 11",
                                    "Woord 12",
                                } },
                    }},
            new PuzzleQuestion() { Answers =  new[] 
                    {
                        new PuzzleAnswer() { Answer = "Antwoord 1", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 1",
                                    "Woord 2",
                                    "Woord 3",
                                    "Woord 4",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 2", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 5",
                                    "Woord 6",
                                    "Woord 7",
                                    "Woord 8",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 3", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 9",
                                    "Woord 10",
                                    "Woord 11",
                                    "Woord 12",
                                } },
                    }},
            new PuzzleQuestion() { Answers =  new[] 
                    {
                        new PuzzleAnswer() { Answer = "Antwoord 1", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 1",
                                    "Woord 2",
                                    "Woord 3",
                                    "Woord 4",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 2", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 5",
                                    "Woord 6",
                                    "Woord 7",
                                    "Woord 8",
                                } },
                        new PuzzleAnswer() { Answer = "Antwoord 3", TimeReward = 30, Words = new [] 
                                {
                                    "Woord 9",
                                    "Woord 10",
                                    "Woord 11",
                                    "Woord 12",
                                } },
                    }},
        };

    #region implemented abstract members of GameRound
    public override void Start(TeamData[] teams)
    {
        _currentQuestionIndex = 0;
        _teams = teams;
        _roundTeamsPlayedIndeces = new List<int>(_teams.Length);
        _currentQuestionTeamsPlayedIndeces = new List<int>(_teams.Length);

        _timer = UpdateCurrentTeamTime;
    }
    public override string SceneName
    {
        get
        {
            return "Puzzle";
        }
    }
    #endregion

    private TeamData[] _teams;
    private int _currentQuestionIndex;

    private int _currentCorrectAnswersCount;
    private int _currentTeamIndex;
    private List<int> _currentQuestionTeamsPlayedIndeces;

    private int _currentQuestionTeamIndex;
    private List<int> _roundTeamsPlayedIndeces;

    private Action<float> _timer;

    private TeamData CurrentTeam
    {
        get{ return _teams[_currentTeamIndex]; }
    }

    private PuzzleQuestion CurrentQuestion
    {
        get{ return Questions[_currentQuestionIndex]; }
    }

    public void NextQuestion()
    {
        _currentCorrectAnswersCount = 0;
        _currentQuestionTeamsPlayedIndeces.Clear();

        _currentQuestionTeamIndex = GetNextTeamIndex(_roundTeamsPlayedIndeces, _currentQuestionTeamIndex);

        if (_currentTeamIndex != -1)
        {
            ++_currentQuestionIndex;

            // Show video/question

            // Start timer
        }
        else
        {
            GameManager.NextRound();
        }
    }

    public void StartTimer()
    {
        TimeManager.Instance.AddTimer(_timer);
    }

    private void UpdateCurrentTeamTime(float timeDelta)
    {
        CurrentTeam.Time -= timeDelta;
    }

    private void EndQuestion()
    {
        TimeManager.Instance.RemoveTimer(_timer);

        // Show result
    }

    public void CorrectAnswer(int answerIndex)
    {
        // Show answer

        CurrentTeam.Time += CurrentQuestion.Answers[answerIndex].TimeReward;

        if (++_currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
        {
            CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);
            EndQuestion();
        }
    }

    private int GetNextTeamIndex(List<int> teamsPlayed, int currentTeamIndex)
    {
        teamsPlayed.Add(currentTeamIndex);

        if (teamsPlayed.Count == _teams.Length)
        {
            return -1;
        }

        int nextTeamIndex = -1;

        for (int teamIndex = 0; teamIndex < _teams.Length; teamIndex++)
        {
            if (teamsPlayed.Contains(teamIndex))
            {
                continue;
            }
            if (nextTeamIndex == -1 || _teams[teamIndex].Time < _teams[nextTeamIndex].Time)
            {
                nextTeamIndex = teamIndex;
            }
        }
        return nextTeamIndex;
    }

    public void TeamPassed()
    {
        CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);

        _currentTeamIndex = GetNextTeamIndex(_currentQuestionTeamsPlayedIndeces, _currentTeamIndex);

        if (_currentTeamIndex == -1)
        {
            EndQuestion();
        }
    }
}