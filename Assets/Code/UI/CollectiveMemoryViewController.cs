using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectiveMemoryViewController : CollectiveMemoryView
{
	[SerializeField]
	private CanvasGroup _answerButtonsCanvas;
	
	[SerializeField]
	private Button[] _answerButtons;

	[SerializeField]
	private Button _nextQuestionButton;
	
	[SerializeField]
	private Button _playVideoButton;
	
	[SerializeField]
	private Button _startTimerButton;
	
	[SerializeField]
	private Button _playerPassedButton;
	
    [SerializeField]
    private CollectiveMemoryView _playerView;
    
    private CollectiveMemoryRound _controller;
    
    public void SetController(CollectiveMemoryRound controller)
    {
    	_controller = controller;
    	
    	_controller.OnWaitingForNextQuestion += SetStateWaitingForNextQuestion;    	
    	_controller.OnWaitingForTimerStart += SetStateWaitingForStartTimer;

		_nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
		_playVideoButton.onClick.AddListener(_playerView.PlayVideo);
		_startTimerButton.onClick.AddListener(_controller.StartTimer);
		_startTimerButton.onClick.AddListener(_playerView.StopVideo);
		_startTimerButton.onClick.AddListener(SetStateToWaitingForAnswer);
		_playerPassedButton.onClick.AddListener(_controller.PlayerPassed);
		
		for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = false;
        }
    }
    
    private void SetStateWaitingForNextQuestion()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.ShowAnswer(index); });
        }

		_answerButtonsCanvas.interactable = false;
		_playVideoButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = true;
    }
    
    private void SetStateWaitingForStartTimer()
    {
    	_answerButtonsCanvas.interactable = false;
    	_playVideoButton.interactable = true;
        _startTimerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateToWaitingForAnswer()
    {
    	for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
        }
    	
    	_answerButtonsCanvas.interactable = true;
    	_playVideoButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _nextQuestionButton.interactable = false;
    }

    public override void SetTeamData(TeamData[] teams)
    {
        base.SetTeamData(teams);
        
        _playerView.SetTeamData(teams);
    }
    
    public override void SetActiveTeam(int index, bool counting)
    {
        base.SetActiveTeam(index, counting);
        
        _playerView.SetActiveTeam(index, counting);
    }
    
    public override void SetQuestion(string[] answers, MovieTexture video)
    {
    	base.SetQuestion(answers, video);
        
        _playerView.SetQuestion(answers, video);
        
        for(int i = 0; i < answers.Length; i++)
        {
        	base.ShowAnswer(i, -1, false);
        	
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
			_answerButtons[i].interactable = true;
        }        
    }
    
    public override void ShowAnswer(int answerIndex, int score, bool showScore = true)
    {
		base.ShowAnswer(answerIndex, score, showScore);
        
        _playerView.ShowAnswer(answerIndex, score, showScore);
    }
    
    public override void PlayVideo()
    {
        _playerView.PlayVideo();
    }
    
    public override void StopVideo()
    {
        _playerView.StopVideo();
    }
}