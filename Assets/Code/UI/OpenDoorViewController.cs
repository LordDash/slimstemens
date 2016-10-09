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
    private Button[] _answerButtons;

    [SerializeField]
    private Button _nextPlayerButton;

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

        _nextPlayerButton.onClick.AddListener(_controller.StartTimer);
        _nextPlayerButton.onClick.AddListener(SetStateToWaitingForAnswer);
        _playerPassedButton.onClick.AddListener(_controller.TeamPassed);
        _nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
        _nextQuestionButton.onClick.AddListener(SetStateToWaitingForQuestionPicked);

        for (int i = 0; i < _questionButtons.Length; i++)
        {
            int index = i;
            _questionButtons[i].onClick.RemoveAllListeners();
            _questionButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to set question {0}", index); _questionButtons[index].interactable = false; _controller.NextQuestion(index); });
            _questionButtons[i].onClick.AddListener(SetStateToWaitingForAnswer);
            _questionButtons[i].interactable = true;
        }
        _questionCanvas.interactable = false;

        _controller.OnWaitingForNextQuestionPrompt += SetStateToWaitingForNextQuestion;
        _controller.OnWaitingForNextPlayer += SetStateWaitingForNextPlayer;
    }

    private void SetStateToWaitingForQuestionPicked()
    {
        _nextPlayerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _questionCanvas.interactable = true;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateWaitingForNextPlayer()
    {
        _nextPlayerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _questionCanvas.interactable = false;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateToWaitingForAnswer()
    {
        _nextPlayerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _questionCanvas.interactable = false;
        _nextQuestionButton.interactable = false;
    }

    private void SetStateToWaitingForNextQuestion()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to show answer {0}", index); _answerButtons[index].interactable = false; _controller.ShowAnswer(index); });
        }

        _nextPlayerButton.interactable = false;
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
        base.SetActiveTeam(index, counting);

        _playerView.SetActiveTeam(index, counting);
    }

    public override void SetAnswers(string question, int[] answerScores, string[] answers)
    {
        base.SetAnswers(question, answerScores, answers);

        for (int i = 0; i < answers.Length; i++)
        {
            base.ShowAnswer(i, true);
        }

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
}
