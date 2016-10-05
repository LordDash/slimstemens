using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ThreeSixNineViewController : MonoBehaviour
{
    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    [SerializeField]
    private Text _questionField;

    [SerializeField]
    private Text _answerField;

    [SerializeField]
    private Image[] _activeQuestionImages;

    [SerializeField]
    private Button _nextQuestionButton;

    [SerializeField]
    private Button _correctAnswerButton;

    [SerializeField]
    private Button _wrongAnswerButton;

    [SerializeField]
    private ThreeSixNinePlayerView _playerView;

    private ThreeSixNineRound _controller;

    public void SetTeamData(TeamData[] teams)
    {
        if (_playerView != null)
        {
            _playerView.SetTeamData(teams);
        }

        for (int i = 0; i < teams.Length; i++)
        {
            _teamDataDisplays[i].Data = teams[i];
        }
    }

    public void SetQuestion(int index, string question, string answer)
    {
        if (_playerView != null)
        {
            _playerView.SetQuestion(index, question, "");
        }

        _questionField.text = question;
        _answerField.text = answer;

        for (int i = 0; i < _activeQuestionImages.Length; i++)
        {
            _activeQuestionImages[i].gameObject.SetActive(i == index);
        }
    }

    public void SetAnswer(string answer)
    {
        if (_playerView != null)
        {
            _playerView.SetAnswer(answer);
        }

        _answerField.text = answer;
    }

    public void SetActiveTeam(int index)
    {
        if (_playerView != null)
        {
            _playerView.SetActiveTeam(index);
        }

        for (int i = 0; i < _teamDataDisplays.Length; i++)
        {
            _teamDataDisplays[i].SetCurrentActive(i == index);
        }
    }

    public void SetController(ThreeSixNineRound controller)
    {
        _controller = controller;

        _nextQuestionButton.onClick.AddListener(_controller.NextQuestion);
        _correctAnswerButton.onClick.AddListener(_controller.AnsweredCorrect);
        _wrongAnswerButton.onClick.AddListener(_controller.AnsweredWrong);
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
        }

        _controller = null;
    }
}