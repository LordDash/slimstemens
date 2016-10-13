using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinaleViewController : FinaleView
{
    [SerializeField]
    private Text _question;

    [SerializeField]
    private CanvasGroup _answerButtonsCanvas;

	[SerializeField]
	private Button[] _answerButtons;

	[SerializeField]
	private Button _nextQuestionButton;
	
	[SerializeField]
	private Button _startTimerButton;

    [SerializeField]
    private Button _nextPlayerButton;

    [SerializeField]
	private Button _playerPassedButton;

    [SerializeField]
    private Button _endRoundButton;

    [SerializeField]
    private FinaleView _playerView;
    
    private FinaleRound _controller;
    
    public void SetController(FinaleRound controller)
    {
    	_controller = controller;
    	
    	_controller.OnWaitingForNextQuestion += SetStateWaitingForNextQuestion;    	
    	_controller.OnWaitingForTimerStart += SetStateWaitingForStartTimer;
        _controller.OnWaitingForNextTeam += SetStateWaitingForNextTeam;

        _nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
		_nextPlayerButton.onClick.AddListener(_controller.NextTeam);
		_startTimerButton.onClick.AddListener(_controller.StartTimer);
		_startTimerButton.onClick.AddListener(SetStateToWaitingForAnswer);
		_playerPassedButton.onClick.AddListener(_controller.PlayerPassed);
        _endRoundButton.onClick.AddListener(_controller.EndRound);


        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = false;
        }
    }

    private void SetStateWaitingForNextTeam()
    {
        _answerButtonsCanvas.interactable = false;
        _nextPlayerButton.interactable = true;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = false;
        _endRoundButton.interactable = true;
    }

    private void SetStateWaitingForNextQuestion()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.ShowAnswer(index); });
        }

        _answerButtonsCanvas.interactable = true;
        _nextPlayerButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = true;
        _endRoundButton.interactable = false;
    }
    
    private void SetStateWaitingForStartTimer()
    {
        _answerButtonsCanvas.interactable = false;
        _nextPlayerButton.interactable = false;
        _startTimerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = false;
        _endRoundButton.interactable = true;
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
        _nextPlayerButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _nextQuestionButton.interactable = false;
        _endRoundButton.interactable = false;
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
    
    public override void SetAnswers(string question, int[] scores, string[] answers)
    {
    	base.SetAnswers(question, scores, answers);

        _question.text = question;


        _playerView.SetAnswers(question, scores, answers);
        
        for(int i = 0; i < answers.Length; i++)
        {
        	base.ShowAnswer(i, false);
        	
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
			_answerButtons[i].interactable = true;
        }        
    }
    
    public override void ShowAnswer(int answerIndex, bool showScore = true)
    {
		base.ShowAnswer(answerIndex, showScore);
        
        _playerView.ShowAnswer(answerIndex, showScore);
    }
}