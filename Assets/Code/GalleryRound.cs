using System;
using System.Collections.Generic;
using UnityEngine;

public class GalleryRound : GameRound
{
    //private static readonly GalleryQuestion[] Questions = new GalleryQuestion[]
    //    {
    //        new GalleryQuestion() { Answers =  new[] 
    //                {
    //                    new GalleryAnswer() { ImageFileName = "Image01", Answer = "Antwoord 1", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image02", Answer = "Antwoord 2", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image03", Answer = "Antwoord 3", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image04", Answer = "Antwoord 4", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image05", Answer = "Antwoord 5", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image06", Answer = "Antwoord 6", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image07", Answer = "Antwoord 7", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image08", Answer = "Antwoord 8", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image09", Answer = "Antwoord 9", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image10", Answer = "Antwoord 10", TimeReward = 15 },
    //                }},
    //        new GalleryQuestion() { Answers =  new[] 
    //                {
    //                    new GalleryAnswer() { ImageFileName = "Image01", Answer = "Antwoord 1", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image02", Answer = "Antwoord 2", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image03", Answer = "Antwoord 3", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image04", Answer = "Antwoord 4", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image05", Answer = "Antwoord 5", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image06", Answer = "Antwoord 6", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image07", Answer = "Antwoord 7", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image08", Answer = "Antwoord 8", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image09", Answer = "Antwoord 9", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image10", Answer = "Antwoord 10", TimeReward = 15 },
    //                }},
    //        new GalleryQuestion() { Answers =  new[] 
    //                {
    //                    new GalleryAnswer() { ImageFileName = "Image01", Answer = "Antwoord 1", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image02", Answer = "Antwoord 2", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image03", Answer = "Antwoord 3", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image04", Answer = "Antwoord 4", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image05", Answer = "Antwoord 5", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image06", Answer = "Antwoord 6", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image07", Answer = "Antwoord 7", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image08", Answer = "Antwoord 8", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image09", Answer = "Antwoord 9", TimeReward = 15 },
    //                    new GalleryAnswer() { ImageFileName = "Image10", Answer = "Antwoord 10", TimeReward = 15 },
    //                }},
    //    };

    #region implemented abstract members of GameRound
    public override void Start(TeamData[] teams, Question[] questions)
    {
        _questions = questions as GalleryQuestion[];
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
            return "Gallery";
        }
    }
    #endregion

    private TeamData[] _teams;
    private int _currentQuestionIndex;

    private int _currentAnswerIndex;

    private int _currentCorrectAnswersCount;
    private int _currentTeamIndex;
    private List<int> _currentQuestionTeamsPlayedIndeces;

    private int _currentQuestionTeamIndex;
    private List<int> _roundTeamsPlayedIndeces;

    private Action<float> _timer;
    private GalleryQuestion[] _questions;

    private TeamData CurrentTeam
    {
        get{ return _teams[_currentTeamIndex]; }
    }

    private GalleryQuestion CurrentQuestion
    {
        get{ return _questions[_currentQuestionIndex]; }
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

    public void CorrectAnswer()
    {
        // Show answer

        CurrentTeam.Time += CurrentQuestion.Answers[_currentAnswerIndex].TimeReward;

        ++_currentCorrectAnswersCount;

        if (++_currentAnswerIndex >= CurrentQuestion.Answers.Length && _currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
        {
            CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);
            EndQuestion();
        }

        if (_currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
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