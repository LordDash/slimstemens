using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ThreeSixNineViewController : ThreeSixNineView
{
    [SerializeField]
    private Button _nextQuestionButton;

    [SerializeField]
    private Button _correctAnswerButton;

    [SerializeField]
    private Button _wrongAnswerButton;

    [SerializeField]
    private ThreeSixNinePlayerView _playerView;

    private ThreeSixNineRound _controller;

    public override void SetTeamData(TeamData[] teams)
    {
        base.SetTeamData(teams);

        if (_playerView != null)
        {
            _playerView.SetTeamData(teams);
        }
    }

    public override void SetQuestion(int index, string question, string answer)
    {
        base.SetQuestion(index, question, answer);

        if (_playerView != null)
        {
            _playerView.SetQuestion(index, question, "");
        }
    }

    public override void SetAnswer(string answer)
    {
        base.SetAnswer(answer);

        if (_playerView != null)
        {
            _playerView.SetAnswer(answer);
        }
    }

    public override void SetActiveTeam(int index)
    {
        base.SetActiveTeam(index);

        if (_playerView != null)
        {
            _playerView.SetActiveTeam(index);
        }
    }

    public void SetController(ThreeSixNineRound controller)
    {
        _controller = controller;

        _controller.OnWaitingForNextQuestionPrompt += SetStateToWaitingForNextQuestion;

        _nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
        _nextQuestionButton.onClick.AddListener(SetStateToWaitingForAnswer);
        _correctAnswerButton.onClick.AddListener(_controller.AnsweredCorrect);
        _wrongAnswerButton.onClick.AddListener(_controller.AnsweredWrong);
    }

    private void SetStateToWaitingForAnswer()
    {
        _nextQuestionButton.interactable = false;
        _correctAnswerButton.interactable = true;
        _wrongAnswerButton.interactable = true;
    }

    private void SetStateToWaitingForNextQuestion()
    {
        _nextQuestionButton.interactable = true;
        _correctAnswerButton.interactable = false;
        _wrongAnswerButton.interactable = false;
    }

    private void OnDestroy()
    {
        if (_controller != null)
        {
            if (_nextQuestionButton != null)
            {
                _nextQuestionButton.onClick.RemoveListener(_controller.NextQuestion);
            }
            if (_correctAnswerButton != null)
            {
                _correctAnswerButton.onClick.RemoveListener(_controller.AnsweredCorrect);
            }
            if (_wrongAnswerButton != null)
            {
                _wrongAnswerButton.onClick.RemoveListener(_controller.AnsweredWrong);
            }

            _controller.OnWaitingForNextQuestionPrompt -= SetStateToWaitingForNextQuestion;

            _controller = null;
        }
    }
}