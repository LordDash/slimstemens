using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorRound : GameRound
{
    //private static readonly OpenDoorQuestion[] Questions = new OpenDoorQuestion[]
    //    {
    //        new OpenDoorQuestion() { Question = "Vraag 1", QuestionFileName = "OpenDoor01", Answers =  new[] 
    //                {
    //                    new OpenDoorAnswer() { Answer = "Antwoord 1", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 2", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 3", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 4", TimeReward = 20 },
    //                }},
    //        new OpenDoorQuestion() { Question = "Vraag 2", QuestionFileName = "OpenDoor02", Answers =  new[] 
    //                {
    //                    new OpenDoorAnswer() { Answer = "Antwoord 1", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 2", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 3", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 4", TimeReward = 20 },
    //                }},
    //        new OpenDoorQuestion() { Question = "Vraag 3", QuestionFileName = "OpenDoor03", Answers =  new[] 
    //                {
    //                    new OpenDoorAnswer() { Answer = "Antwoord 1", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 2", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 3", TimeReward = 20 },
    //                    new OpenDoorAnswer() { Answer = "Antwoord 4", TimeReward = 20 },
    //                }},
    //    };

    #region implemented abstract members of GameRound

    public override void Start(TeamData[] teams, Question[] questions)
    {
        _questions = questions as OpenDoorQuestion[];
        _currentQuestionIndex = -1;
        _currentQuestionTeamIndex = -1;
        _currentTeamIndex = -1;
        _teams = teams;
        _roundTeamsPlayedIndeces = new List<int>(_teams.Length);
        _currentQuestionTeamsPlayedIndeces = new List<int>(_teams.Length);

        _timer = UpdateCurrentTeamTime;

        _view = GameObject.FindObjectOfType<OpenDoorViewController>();
        _view.SetController(this);
        _view.SetTeamData(_teams);
        _view.SetAnswers("Press the Next Question button to start the round.", new int[0], new string[0]);
        _view.SetVideos(GetQuestionVideos(), GetQuestionFirstFrames());
        _view.ClearAnswers();

        _onWaitingForNextQuestion();
    }

    private Sprite[] GetQuestionFirstFrames()
    {
        Sprite[] firstFrames = new Sprite[_questions.Length];

        for (int i = 0; i < _questions.Length; i++)
        {
            firstFrames[i] = FileLoader.Load<Sprite>(_questions[i].QuestionFirstFrameFilePath);
        }

        return firstFrames;
    }

    private MovieTexture[] GetQuestionVideos()
    {
        MovieTexture[] videos = new MovieTexture[_questions.Length];

        for (int i = 0; i < _questions.Length; i++)
        {
            videos[i] = FileLoader.Load<MovieTexture>(_questions[i].QuestionFilePath);
        }

        return videos;
    }

    public override string SceneName
    {
        get
        {
            return "OpenDoor";
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
    private OpenDoorQuestion[] _questions;

    private Action _onWaitingForNextQuestion = () => { };
    private Action _onWaitingForStartTimer = () => { };
    private Action _onWaitingForEndRound = () => { };
    private OpenDoorViewController _view;

    private TeamData CurrentTeam
    {
        get{ return _teams[_currentTeamIndex]; }
    }

    private OpenDoorQuestion CurrentQuestion
    {
        get{ return _questions[_currentQuestionIndex]; }
    }

    public event Action OnWaitingForEndRound
    {
        add
        {
            _onWaitingForEndRound -= value;
            _onWaitingForEndRound += value;
        }
        remove
        {
            _onWaitingForEndRound -= value;
        }
    }

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

    public event Action OnWaitingForStartTimer
    {
        add
        {
            _onWaitingForStartTimer -= value;
            _onWaitingForStartTimer += value;
        }
        remove
        {
            _onWaitingForStartTimer -= value;
        }
    }

    public void NextQuestion()
    {
        _currentCorrectAnswersCount = 0;
        _currentQuestionTeamsPlayedIndeces.Clear();

        _currentQuestionTeamIndex = GetNextTeamIndex(_roundTeamsPlayedIndeces, _currentQuestionTeamIndex);
        _currentTeamIndex = _currentQuestionTeamIndex;

        _view.SetActiveTeam(_currentTeamIndex, false);

        if (_currentQuestionTeamIndex != -1)
        {
            _view.ClearAnswers();
            _view.SetVideoUsed(_currentQuestionIndex);
        }
        else
        {
            GameManager.NextRound();
        }
    }

    public void StartTimer()
    {
        _view.SetActiveTeam(_currentTeamIndex, true);
        TimeManager.Instance.AddTimer(_timer);
    }

    private void StopTimer()
    {
        _view.SetActiveTeam(_currentTeamIndex, false);
        TimeManager.Instance.RemoveTimer(_timer);
    }

    private void UpdateCurrentTeamTime(float timeDelta)
    {
        CurrentTeam.Time -= timeDelta;
    }

    private void EndQuestion()
    {
        StopTimer();

        _onWaitingForNextQuestion();
    }

    public void CorrectAnswer(int answerIndex)
    {
        // Show answer
        _view.ShowAnswer(answerIndex, true);

        CurrentTeam.Time += CurrentQuestion.Answers[answerIndex].TimeReward;

        if (++_currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
        {
            CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);
            EndQuestion();
        }
    }

    public void ShowAnswer(int answerIndex)
    {
        _view.ShowAnswer(answerIndex, false);
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

        StopTimer();
        _onWaitingForStartTimer();

        if (_currentTeamIndex == -1)
        {
            EndQuestion();
        }
    }

    public void NextQuestion(int questionIndex)
    {
        _currentQuestionIndex = questionIndex;

        _view.SetAnswers(CurrentQuestion.Question, CurrentQuestion.GetTimeRewards(), CurrentQuestion.GetAnswers());
        _view.ShowVideo(questionIndex);
    }
}