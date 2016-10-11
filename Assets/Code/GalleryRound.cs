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
        _currentQuestionIndex = -1;
        _currentTeamIndex = -1;
        _currentQuestionTeamIndex = -1;
        _teams = teams;
        _roundTeamsPlayedIndeces = new List<int>(_teams.Length);
        _currentQuestionTeamsPlayedIndeces = new List<int>(_teams.Length);

        _timer = UpdateCurrentTeamTime;

        _view = GameObject.FindObjectOfType<GalleryViewController>();
        _view.SetController(this);
        _view.SetTeamData(_teams);
        _view.ClearQuestion();

        _onWaitingForNextQuestion();
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

    private Action _onWaitingForNextPlayer = () => { };
    private Action _onWaitingForAnswersViewed = () => { };
    private Action _onWaitingForNextQuestion = () => { };

    private GalleryViewController _view;
    private Sprite[] _loadedQuestionSprites;
    private Sprite[] _loadedQuestionOriginalSprites;

    private TeamData CurrentTeam
    {
        get{ return _teams[_currentTeamIndex]; }
    }

    private GalleryQuestion CurrentQuestion
    {
        get{ return _questions[_currentQuestionIndex]; }
    }

    public event Action OnWaitingForAnswersViewed
    {
        add
        {
            _onWaitingForAnswersViewed -= value;
            _onWaitingForAnswersViewed += value;
        }
        remove
        {
            _onWaitingForAnswersViewed -= value;
        }
    }

    public event Action OnWaitingForNextPlayer
    {
        add
        {
            _onWaitingForNextPlayer -= value;
            _onWaitingForNextPlayer += value;
        }
        remove
        {
            _onWaitingForNextPlayer -= value;
        }
    }

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

    public void NextQuestion()
    {
        _currentCorrectAnswersCount = 0;
        _currentAnswerIndex = 0;
        _currentQuestionTeamsPlayedIndeces.Clear();

        _currentQuestionTeamIndex = GetNextTeamIndex(_roundTeamsPlayedIndeces, _currentQuestionTeamIndex);

        if (_currentQuestionTeamIndex != -1)
        {
            _currentTeamIndex = _currentQuestionTeamIndex;
            ++_currentQuestionIndex;

            LoadQuestionSprites();

            // Show video/question
            _view.ShowQuestion(_currentAnswerIndex, _loadedQuestionSprites[_currentAnswerIndex], _loadedQuestionOriginalSprites[_currentAnswerIndex], CurrentQuestion.Answers[_currentAnswerIndex].Answer);

            // Start timer
            StartTimer();
        }
        else
        {
            GameManager.NextRound();
        }
    }

    private void LoadQuestionSprites()
    {
        _loadedQuestionSprites = new Sprite[CurrentQuestion.Answers.Length];
        _loadedQuestionOriginalSprites = new Sprite[CurrentQuestion.Answers.Length];

        for (int i = 0; i < CurrentQuestion.Answers.Length; i++)
        {
            _loadedQuestionSprites[i] = FileLoader.Load<Sprite>(CurrentQuestion.Answers[i].ImageFileName);

            if (string.IsNullOrEmpty(CurrentQuestion.Answers[i].ImageOriginalFileName) == false)
            {
                _loadedQuestionOriginalSprites[i] = FileLoader.Load<Sprite>(CurrentQuestion.Answers[i].ImageOriginalFileName);
            }
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

        _currentAnswerIndex = -1;
        _onWaitingForAnswersViewed();
    }

    public void CorrectAnswer(int answerIndex)
    {
        CurrentTeam.Time += CurrentQuestion.Answers[answerIndex].TimeReward;
        ++_currentCorrectAnswersCount;

        if (_currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
        {
            CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);
            EndQuestion();
        }
    }

    public void CorrectAnswer()
    {
        CurrentTeam.Time += CurrentQuestion.Answers[_currentAnswerIndex].TimeReward;

        ++_currentCorrectAnswersCount;
        ++_currentAnswerIndex;

        if (_currentCorrectAnswersCount == CurrentQuestion.Answers.Length)
        {
            _view.ClearQuestion();
            CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);
            EndQuestion();
        }
        else if (_currentAnswerIndex >= CurrentQuestion.Answers.Length)
        {
            // Hide picture
            _view.ClearQuestion();

            NextTeam();
        }
        else
        {
            // Show picture
            _view.ShowQuestion(_currentAnswerIndex, _loadedQuestionSprites[_currentAnswerIndex], _loadedQuestionOriginalSprites[_currentAnswerIndex], CurrentQuestion.Answers[_currentAnswerIndex].Answer);
        }
    }

    public void Pass()
    {
        ++_currentAnswerIndex;

        if (_currentAnswerIndex >= CurrentQuestion.Answers.Length)
        {
            // Hide picture
            _view.ClearQuestion();

            NextTeam();
        }
        else
        {
            // Show picture
            _view.ShowQuestion(_currentAnswerIndex, _loadedQuestionSprites[_currentAnswerIndex], _loadedQuestionOriginalSprites[_currentAnswerIndex], CurrentQuestion.Answers[_currentAnswerIndex].Answer);
        }
    }

    public void NextTeam()
    {
        CurrentTeam.Time = Mathf.CeilToInt(CurrentTeam.Time);

        _currentTeamIndex = GetNextTeamIndex(_currentQuestionTeamsPlayedIndeces, _currentTeamIndex);

        if (_currentTeamIndex == -1)
        {
            EndQuestion();
        }
        else
        {
            _onWaitingForNextPlayer();
            StopTimer();
        }
    }

    public void ShowNextPicture()
    {
        ++_currentAnswerIndex;

        if (_currentAnswerIndex < CurrentQuestion.Answers.Length)
        {
            // show picture
            _view.ShowQuestion(_currentAnswerIndex, _loadedQuestionSprites[_currentAnswerIndex], _loadedQuestionOriginalSprites[_currentAnswerIndex], CurrentQuestion.Answers[_currentAnswerIndex].Answer, true);
        }
        else
        {
            _view.ClearQuestion();
            _onWaitingForNextQuestion();
        }
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
}