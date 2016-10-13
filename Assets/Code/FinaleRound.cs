using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FinaleRound : GameRound
{
    private FinaleQuestion[] _questions;
    private TeamData[] _teams;

    private int _currentTeamIndex;
    private int _currentQuestionIndex;
    private int _currentCorrectAnswersCount;

    private List<int> _currentQuestionTeamsPlayedIndeces;

    private Action _onWaitingForNextQuestion = () => { };
    private Action _onWaitingForTimerStart = () => { };
    private Action _onWaitingForNextTeam = () => { };

    private Action<float> _timer;
    private FinaleViewController _view;

    public event Action OnWaitingForNextQuestion
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

    public event Action OnWaitingForTimerStart
    {
        add
        {
            _onWaitingForTimerStart -= value;
            _onWaitingForTimerStart += value;
        }
        remove
        {
            _onWaitingForTimerStart -= value;
        }
    }

    public event Action OnWaitingForNextTeam
    {
        add
        {
            _onWaitingForNextTeam -= value;
            _onWaitingForNextTeam += value;
        }
        remove
        {
            _onWaitingForNextTeam -= value;
        }
    }

    private TeamData CurrentTeam
    {
        get { return _teams[_currentTeamIndex]; }
    }

    private FinaleQuestion CurrentQuestion
    {
        get { return _questions[_currentQuestionIndex]; }
    }

    public override string SceneName
    {
        get
        {
            return "Finale";
        }
    }

    public override void Start(TeamData[] teams, Question[] questions)
    {
        _teams = teams;
        _questions = questions as FinaleQuestion[];

        _timer = UpdateCurrentTeamTime;

        _currentQuestionIndex = -1;
        _currentTeamIndex = -1;

        _currentQuestionTeamsPlayedIndeces = new List<int>();

        _view = GameObject.FindObjectOfType<FinaleViewController>();
        _view.SetController(this);
        _view.SetTeamData(_teams);
        _view.SetAnswers("Press Next Question to start the round.", new int[0], new string[0]);

        _onWaitingForNextQuestion();
    }

    private void UpdateCurrentTeamTime(float timeDelta)
    {
        CurrentTeam.Time -= timeDelta;

        if(CurrentTeam.Time <= 0)
        {
            CurrentTeam.Time = 0;
            StopTimer();

            _onWaitingForNextTeam();
        }
    }

    public void StartTimer()
    {
        _view.SetActiveTeam(_currentTeamIndex, true);
        TimeManager.Instance.AddTimer(_timer);
    }

    public void StopTimer()
    {
        _view.SetActiveTeam(_currentTeamIndex, false);
        TimeManager.Instance.RemoveTimer(_timer);
    }

    public void CorrectAnswer(int answerIndex)
    {
        int timeReward = CurrentQuestion.Answers[answerIndex].TimeReward;

        for(int i = 0; i < _teams.Length; i++)
        {
            if (i != _currentTeamIndex && _teams[i].Time > 0)
            {
                _teams[i].Time -= timeReward;

                if (_teams[i].Time <= 0)
                {
                    _teams[i].Time = 0;
                    StopTimer();

                    _onWaitingForTimerStart();
                }
            }
        }

        // show answer
        _view.ShowAnswer(answerIndex, true);

        ++_currentCorrectAnswersCount;

        if (_currentCorrectAnswersCount >= CurrentQuestion.Answers.Length)
        {
            EndQuestion();
        }
    }

    public void PlayerPassed()
    {
        CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);

        _currentTeamIndex = GetNextTeamIndex(_currentQuestionTeamsPlayedIndeces, _currentTeamIndex);

        if (_currentTeamIndex == -1)
        {
            EndQuestion();
        }
        else
        {
            _view.SetActiveTeam(_currentTeamIndex, true);
        }
    }

    public void ShowAnswer(int answerIndex)
    {
        // show answer
        _view.ShowAnswer(answerIndex, false);
    }

    public void NextTeam()
    {
        _currentTeamIndex = GetNextTeamIndex(_currentQuestionTeamsPlayedIndeces, _currentTeamIndex);

        if (_currentTeamIndex == -1)
        {
            EndQuestion();
        }
        else
        {
            _view.SetActiveTeam(_currentTeamIndex, false);

            _onWaitingForTimerStart();
        }
    }

    private void EndQuestion()
    {
        StopTimer();

        _onWaitingForNextQuestion();
    }

    public void NextQuestion()
    {
        _currentCorrectAnswersCount = 0;
        _currentQuestionTeamsPlayedIndeces.Clear()
        _currentTeamIndex = -1;

        _currentTeamIndex = GetNextTeamIndex(_currentQuestionTeamsPlayedIndeces, _currentTeamIndex);

        if (_currentTeamIndex != -1)
        {
            ++_currentQuestionIndex;

            if (_currentQuestionIndex < _questions.Length)
            {
                // Set question
                _view.SetAnswers(CurrentQuestion.Question, CurrentQuestion.GetTimeRewards(), CurrentQuestion.GetAnswers());

                _view.SetActiveTeam(_currentTeamIndex, false);

                _onWaitingForTimerStart();
            }
            else
            {
                GameManager.NextRound();
            }
        }
        else
        {
            GameManager.NextRound();
        }
    }

    public void EndRound()
    {
        GameManager.NextRound();
    }

    private int GetNextTeamIndex(List<int> teamsPlayed, int currentTeamIndex)
    {
        if (currentTeamIndex != -1)
        {
            teamsPlayed.Add(currentTeamIndex);
        }

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
            if (_teams[teamIndex].Time > 0 && (nextTeamIndex == -1 || _teams[teamIndex].Time < _teams[nextTeamIndex].Time))
            {
                nextTeamIndex = teamIndex;
            }
        }
        return nextTeamIndex;
    }
}
