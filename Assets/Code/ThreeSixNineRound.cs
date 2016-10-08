using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ThreeSixNineRound : GameRound
{
    private static readonly ThreeSixNineQuestion[] Questions = new ThreeSixNineQuestion[]
        {
            new ThreeSixNineQuestion() { Question = "Wie antwoorde op de vragenlijst dat hij/zij geboren is in het jaar 198?", Answer = "Jeroen", TimeReward = 0 },
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

    private Action _onWaitingForNextQuestion = () => { };

    public event Action OnWaitingForNextQuestionPrompt
    {
        add
        {
            _onWaitingForNextQuestion -= value;
            _onWaitingForNextQuestion += value;
        }
        remove
        {
            _onWaitingForNextQuestion -= value;
        }
    }

    private TeamData[] _teams;
    private int _currentQuestionIndex;
    private int _currentTeamIndex;
    private List<int> _currentQuestionTeamsPlayedIndeces = new List<int>();

    private ThreeSixNineViewController _view;

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
        Debug.LogFormat("[ThreeSixNine] Start round");
        _teams = teams;
        _currentQuestionIndex = -1;
        _currentTeamIndex = 0;

        _view = GameObject.FindObjectOfType<ThreeSixNineViewController>();
        _view.SetController(this);
        _view.SetTeamData(_teams);
        _view.SetActiveTeam(_currentTeamIndex);
        _view.SetQuestion(-1, "", "Press Next Question to start the game.");

        _onWaitingForNextQuestion();
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
            Debug.LogFormat("[ThreeSixNine] Next question\n{0}", CurrentQuestion.ToString());
            _view.SetQuestion(_currentQuestionIndex, CurrentQuestion.Question, CurrentQuestion.Answer);
        }
        else
        {
            Debug.LogFormat("[ThreeSixNine] Next round");
            GameManager.NextRound();
        }
    }

    public void AnsweredCorrect()
    {
        _view.SetAnswer(CurrentQuestion.Answer);

        CurrentTeam.Time += CurrentQuestion.TimeReward;

        _onWaitingForNextQuestion();

        _currentQuestionTeamsPlayedIndeces.Clear();

        Debug.LogFormat("[ThreeSixNine] Answer correct, added {0} seconds to {1}", CurrentQuestion.TimeReward, CurrentTeam.Name);
    }

    public void AnsweredWrong()
    {
        _currentQuestionTeamsPlayedIndeces.Add(_currentTeamIndex);

        _currentTeamIndex = (_currentTeamIndex + 1) % _teams.Length;

        _view.SetActiveTeam(_currentTeamIndex);

        if (_currentQuestionTeamsPlayedIndeces.Contains(_currentTeamIndex))
        {
            _view.SetAnswer(CurrentQuestion.Answer);

            _onWaitingForNextQuestion();

            _currentQuestionTeamsPlayedIndeces.Clear();
        }

        Debug.LogFormat("[ThreeSixNine] Answer wrong, {0}'s turn", CurrentTeam.Name);
    }
}

public class ThreeSixNineQuestion
{
    public string Question;
    public string Answer;
    public int TimeReward;

    public override string ToString()
    {
        return string.Format("{0} [A: {1}] [{2}]", Question, Answer, TimeReward);
    }
}