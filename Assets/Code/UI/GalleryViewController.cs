using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GalleryViewController : GalleryView
{
    [SerializeField]
    private Text _answer;

    [SerializeField]
    private Text[] _answers;


    [SerializeField]
    private Button _answeredCorrectButton;

    [SerializeField]
    private Button _playerPassedButton;

    [SerializeField]
    private Button _nextPictureButton;

    [SerializeField]
    private Button[] _answerButtons;

    [SerializeField]
    private CanvasGroup _answerButtonsCanvas;

    [SerializeField]
    private Button _startTimerButton;

    [SerializeField]
    private Button _nextPlayerButton;

    [SerializeField]
    private Button _nextGalleryButton;

    [SerializeField]
    private GalleryView _playerView;

    private GalleryRound _controller;

    public void SetController(GalleryRound controller)
    {
        _controller = controller;

        _controller.OnWaitingForNextPlayer += SetStateWaitingForNextPlayer;
        _controller.OnWaitingForNextQuestion += SetStateToWaitingForNextQuestion;
        _controller.OnWaitingForAnswersViewed += SetStateToViewAnswers;

        _playerPassedButton.onClick.AddListener(_controller.Pass);
        _nextPictureButton.onClick.AddListener(_controller.ShowNextPicture);
        for(int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[index].onClick.AddListener(() => { _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
        }
        _nextGalleryButton.onClick.AddListener(_controller.NextQuestion);
        _nextGalleryButton.onClick.AddListener(SetStateToWaitingForAnswer);
        _startTimerButton.onClick.AddListener(_controller.StartTimer);
        _startTimerButton.onClick.AddListener(SetStateWaitingForAnswerOfSecondThirdPlayer);
        _nextPlayerButton.onClick.AddListener(_controller.NextTeam);
    }

    private void SetStateToViewAnswers()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = false;
        }

        _answerButtonsCanvas.interactable = false;
        _answeredCorrectButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextGalleryButton.interactable = false;
        _nextPictureButton.interactable = true;
        _nextPlayerButton.interactable = false;
    }

    private void SetStateWaitingForAnswerOfSecondThirdPlayer()
    {
        _answerButtonsCanvas.interactable = true;
        _answeredCorrectButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextGalleryButton.interactable = false;
        _nextPictureButton.interactable = false;
        _nextPlayerButton.interactable = true;
    }

    private void SetStateWaitingForNextPlayer()
    {
        _answerButtonsCanvas.interactable = false;
        _answeredCorrectButton.interactable = false;
        _startTimerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _nextGalleryButton.interactable = false;
        _nextPictureButton.interactable = false;
        _nextPlayerButton.interactable = false;
    }

    private void SetStateToWaitingForAnswer()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = true;
        }

        _answerButtonsCanvas.interactable = false;
        _answeredCorrectButton.interactable = true;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _nextGalleryButton.interactable = false;
        _nextPictureButton.interactable = false;
        _nextPlayerButton.interactable = false;
    }

    private void SetStateToWaitingForNextQuestion()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = false;
        }

        _answerButtonsCanvas.interactable = false;
        _answeredCorrectButton.interactable = false;
        _startTimerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextGalleryButton.interactable = true;
        _nextPictureButton.interactable = false;
        _nextPlayerButton.interactable = false;
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

    public override void ShowQuestion(int currentQuestionIndex, Sprite questionImage, Sprite questionOriginalImage, string answer, bool reveal = false)
    {
        base.ShowQuestion(currentQuestionIndex, questionImage, questionOriginalImage, answer, reveal);

        _answer.text = answer;
        _answers[currentQuestionIndex].text = answer;

        _answeredCorrectButton.onClick.RemoveAllListeners();
        _answeredCorrectButton.onClick.AddListener(
            () =>
            {
                _controller.CorrectAnswer();
                _answerButtons[currentQuestionIndex].interactable = false;
            });

        _playerView.ShowQuestion(currentQuestionIndex, questionImage, questionOriginalImage, answer, reveal);
    }

    public override void ClearQuestion()
    {
        base.ClearQuestion();

        _answer.text = "";

        _playerView.ClearQuestion();
    }
}
