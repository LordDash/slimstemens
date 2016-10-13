using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoorViewController : OpenDoorView
{
    [SerializeField]
    private Text _question;

    [SerializeField]
    private CanvasGroup _questionCanvas;

    [SerializeField]
    private Button[] _questionButtons;

	[SerializeField]
    private CanvasGroup _answerButtonsCanvas;

    [SerializeField]
    private Button[] _answerButtons;

    [SerializeField]
    private Button _startTimerButton;

    [SerializeField]
    private Button _playerPassedButton;

    [SerializeField]
    private Button _nextQuestionButton;

    [SerializeField]
    private OpenDoorView _playerView;

    private OpenDoorRound _controller;

    public void SetController(OpenDoorRound controller)
    {
        _controller = controller;

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to show answer {0}", index); _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
            _answerButtons[i].interactable = false;
        }

        _startTimerButton.onClick.AddListener(_controller.StartTimer);
        _startTimerButton.onClick.AddListener(SetStateToWaitingForAnswer);
        _playerPassedButton.onClick.AddListener(_controller.TeamPassed);
        _nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
        _nextQuestionButton.onClick.AddListener(SetStateToWaitingForQuestionPicked);

        for (int i = 0; i < _questionButtons.Length; i++)
        {
            int index = i;
            _questionButtons[i].onClick.RemoveAllListeners();
            _questionButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to set question {0}", index); _questionButtons[index].interactable = false; _controller.NextQuestion(index); });
            _questionButtons[i].onClick.AddListener(SetStateWaitingForStartTimer);
            _questionButtons[i].interactable = true;
        }
        _questionCanvas.interactable = false;

        _controller.OnWaitingForNextQuestionPrompt += SetStateToWaitingForNextQuestion;
        _controller.OnWaitingForStartTimer += SetStateWaitingForStartTimer;
    }

    private void SetStateToWaitingForQuestionPicked()
    { 
    	_answerButtonsCanvas.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _questionCanvas.interactable = true;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateWaitingForStartTimer()
    { 
    	_answerButtonsCanvas.interactable = false;
        _startTimerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _questionCanvas.interactable = false;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateToWaitingForAnswer()
    { 
    	_answerButtonsCanvas.interactable = true;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _questionCanvas.interactable = false;
        _nextQuestionButton.interactable = false;

        StopCurrentVideo();
    }

    private void SetStateToWaitingForNextQuestion()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to show answer {0}", index); _answerButtons[index].interactable = false; _controller.ShowAnswer(index); });
        }
		_answerButtonsCanvas.interactable = true;

        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextQuestionButton.interactable = true;
    }

    public override void SetTeamData(TeamData[] teams)
    {
        base.SetTeamData(teams);

        _playerView.SetTeamData(teams);
    }

    public override void SetActiveTeam(int index, bool counting)
    {
        Debug.LogFormat("Next team to play: {0}", index);

        base.SetActiveTeam(index, counting);

        _playerView.SetActiveTeam(index, counting);
    }

    public override void SetAnswers(string question, int[] answerScores, string[] answers)
    {
        base.SetAnswers(question, answerScores, answers);

        for (int i = 0; i < answers.Length; i++)
        {
            base.ShowAnswer(i, false);

            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to show answer {0}", index); _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
            _answerButtons[i].interactable = true;
        
        
        _answerButtonsCanvas.interactable = false;}

        _question.text = question;

        _playerView.SetAnswers(question, answerScores, answers);
    }

    public override void ClearAnswers()
    {
        base.ClearAnswers();

        _question.text = "";

        _playerView.ClearAnswers();
    }

    public override void ShowAnswer(int answerIndex, bool showScore)
    {
        base.ShowAnswer(answerIndex, showScore);

        _playerView.ShowAnswer(answerIndex, showScore);
    }

    public override void SetVideos(MovieTexture[] videos, Sprite[] firstFrames)
    {
        base.SetVideos(videos, firstFrames);

        _playerView.SetVideos(videos, firstFrames);
    }

    public override void ShowVideo(int questionIndex)
    {
        _playerView.ShowVideo(questionIndex);
    }

    public override void SetVideoUsed(int questionIndex)
    {
        base.SetVideoUsed(questionIndex);

        _playerView.SetVideoUsed(questionIndex);
    }

    public override void StopCurrentVideo()
    {
        _playerView.StopCurrentVideo();
    }
}
