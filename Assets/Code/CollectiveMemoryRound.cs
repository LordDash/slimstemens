using UnityEngine;
using System.Collections;
using System;

public class CollectiveMemoryRound : GameRound
{
	private TeamData[] _teams;
	private CollectiveMemoryQuestion[] _questions;
	
	private int _currentQuestionIndex;
	private int _currentCorrectAnswersCount;
	
	private int _currentTeamIndex;
	private List<int> _currentQuestionTeamsPlayedIndeces;
	
	private int _currentQuestionTeamIndex;
	private List<int> _roundTeamsPlayedIndeces;
	
	private MovieTexture[] _loadedQuestionVideos;
	
	private Action _onWaitingForTimerStart = () => {};
	private Action _onWaitingForNextQuestion = () => {};
	
	private TeamData CurrentTeam
    {
        get{ return _teams[_currentTeamIndex]; }
    }

    private CollectiveMemoryQuestion CurrentQuestion
    {
        get{ return _questions[_currentQuestionIndex]; }
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
	
    public override string SceneName
    {
        get
        {
            return "CollectiveMemory";
        }
    }

    public override void Start(TeamData[] teams, Question[] questions)
    {
        _teams = teams;
        _questions = questions as CollectiveMemoryQuestion[];
        
        LoadQuestionVideos();
        
        _currentQuestionTeamsPlayedIndeces = new List<int>();
        _roundTeamsPlayedIndeces = new List<int>();
        
        _currentQuestionTeamIndex = -1;
        _currentTeamIndex = -1;
        
        _view = GameObject.FindObjectOfType<CollectiveMemoryViewController>();
        _view.SetController(this);
        _view.SetTeamData(_teams);
        
        _onWaitingForNextQuestion();
    }
    
    private void LoadQuestionVideos()
    {
    	_loadedQuestionVideos = new MovieTexture[_questions.Length];
    	
    	for(int i = 0; i < _loadedQuestionVideos.Length; i++)
    	{
    		_loadedQuestionVideos[i] = FileLoader.Load<MovieTexture>(_questions[i].QuestionFileName);
    	}
    }
    
	public void NextQuestion()
	{
		_currentCorrectAnswersCount = 0;
        _currentQuestionTeamsPlayedIndeces.Clear();

        _currentQuestionTeamIndex = GetNextTeamIndex(_roundTeamsPlayedIndeces, _currentQuestionTeamIndex);

        if (_currentQuestionTeamIndex != -1)
        {
            _currentTeamIndex = _currentQuestionTeamIndex;
            ++_currentQuestionIndex;

			// Set question
			_view.SetQuestion(CurrentQuestion.Answers, _loadedQuestionVideos[_currentQuestionIndex]);
			
			_view.SetActiveTeam(_currentTeamIndex, false);
			
			_onWaitingForTimerStart();
        }
        else
        {
            GameManager.NextRound();
        }
	}
	
	private void EndQuestion()
    {
        StopTimer();
        
		_onWaitingForNextQuestion();
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
        	_onWaitingForTimerStart();
            StopTimer();
        }
    }
    
    public void CorrectAnswer(int answerIndex)
    {
    	int timeReward = CurrentQuestion.GetNextTimeReward();
    	
    	CurrentTeam.Time += timeReward;
    	
    	// show answer
    	_view.ShowAnswer(answerIndex, timeReward, true);
    	
    	++_currentCorrectAnswersCount;
    	
    	if(_currentCorrectAnswersCount >= CurrentQuestion.Answers.Length)
    	{
    		EndQuestion();
    	}
    }
    
    public void ShowAnswer(int answerIndex)
    {
    	// show answer
    	_view.ShowAnswer(answerIndex, timeReward, false);
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
