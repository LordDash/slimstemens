using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeSixFiveRound : GameRound
{
    private static readonly ThreeSixNineQuestion[] Questions = new ThreeSixNineQuestion[]
        {
            new ThreeSixNineQuestion() { Question = "Vraag 1", Answer = "Antwoord 1", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 2", Answer = "Antwoord 2", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 3", Answer = "Antwoord 3", TimeReward = 10 },
            new ThreeSixNineQuestion() { Question = "Vraag 4", Answer = "Antwoord 4", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 5", Answer = "Antwoord 5", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 6", Answer = "Antwoord 6", TimeReward = 10 },
            new ThreeSixNineQuestion() { Question = "Vraag 7", Answer = "Antwoord 7", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 8", Answer = "Antwoord 8", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 9", Answer = "Antwoord 9", TimeReward = 10 },
            new ThreeSixNineQuestion() { Question = "Vraag 10", Answer = "Antwoord 10", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 11", Answer = "Antwoord 11", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 12", Answer = "Antwoord 12", TimeReward = 10 },
            new ThreeSixNineQuestion() { Question = "Vraag 13", Answer = "Antwoord 13", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 14", Answer = "Antwoord 14", TimeReward = 0 },
            new ThreeSixNineQuestion() { Question = "Vraag 15", Answer = "Antwoord 15", TimeReward = 10 },
        };

    private TeamData[] _teams;
    private int _currentQuestionIndex;
    private int _currentTeamIndex;

    private TeamData CurrentTeam
    {
        get { return _teams[_currentTeamIndex]; }
    }

    private ThreeSixNineQuestion CurrentQuestion
    {
        get { return Questions[_currentQuestionIndex]; }
    }

    #region implemented abstract members of GameRound

    public override void Start(TeamData[] teams)
    {
        this._teams = teams;
        this._currentQuestionIndex = -1;
        this._currentTeamIndex = 0;
    }

    public override string SceneName
    {
        get
        {
            return "ThreeSixNine";
        }
    }

    #endregion

    public void NextQuestion()
    {
        ++_currentQuestionIndex;

        if(_currentQuestionIndex < Questions.Length)
        {
            // Show question
        }
        else
        {
            GameManager.NextRound();
        }
    }

    public void AnsweredCorrect()
    {
        // Show answer

        CurrentTeam.Time += CurrentQuestion.TimeReward;
    }

    public void AnsweredWrong()
    {
        _currentTeamIndex = (_currentTeamIndex + 1) % _teams.Length;

        // Show next team is active
    }
}

public class ThreeSixNineQuestion
{
    public string Question;
    public string Answer;
    public int TimeReward;
}