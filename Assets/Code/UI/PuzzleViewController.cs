using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleViewController : PuzzleView
{
    [SerializeField]
    private CanvasGroup _answerButtonsCanvas;

    [SerializeField]
    private Button[] _answerButtons;

    [SerializeField]
    private Button _nextPlayerButton;

    [SerializeField]
    private Button _playerPassedButton;

    [SerializeField]
    private Button _nextPuzzleButton;

    [SerializeField]
    private PuzzleView _playerView;

    private PuzzleRound _controller;

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

    public override void SetAnswers(int[] answerScores, string[] answers)
    {
        base.SetAnswers(answerScores, answers);

        _playerView.SetAnswers(answerScores, answers);

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int index = i;
            _answerButtons[i].onClick.RemoveAllListeners();
            _answerButtons[i].onClick.AddListener(() => { Debug.LogFormat("Trying to show answer {0}", index); _answerButtons[index].interactable = false; _controller.CorrectAnswer(index); });
            _answerButtons[i].interactable = true;

            base.ShowAnswer(i, new int[0], false);
        }
    }

    public override void SetPuzzleWords(string[] puzzleWords)
    {
        base.SetPuzzleWords(puzzleWords);

        _playerView.SetPuzzleWords(puzzleWords);
    }

    public override void ShowAnswer(int answerIndex, int[] puzzleWordIndeces, bool showScore)
    {
        base.ShowAnswer(answerIndex, puzzleWordIndeces, showScore);

        _playerView.ShowAnswer(answerIndex, puzzleWordIndeces, showScore);
    }

    public void SetController(PuzzleRound controller)
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
        _nextPuzzleButton.onClick.AddListener(_controller.NextQuestion);
        _nextPuzzleButton.onClick.AddListener(SetStateToWaitingForAnswer);

        _controller.OnWaitingForNextQuestionPrompt += SetStateToWaitingForNextQuestion;
        _controller.OnWaitingForNextPlayer += SetStateWaitingForNextPlayer;
    }

    private void SetStateWaitingForNextPlayer()
    {
        _nextPlayerButton.interactable = true;
        _playerPassedButton.interactable = false;
        _nextPuzzleButton.interactable = false;
        _answerButtonsCanvas.interactable = false;
    }

    private void SetStateToWaitingForAnswer()
    {
        _answerButtonsCanvas.interactable = true;
        _nextPlayerButton.interactable = false;
        _playerPassedButton.interactable = true;
        _nextPuzzleButton.interactable = false;
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
        _nextPlayerButton.interactable = false;
        _playerPassedButton.interactable = false;
        _nextPuzzleButton.interactable = true;
    }

    private void OnDestroy()
    {
        if (_controller != null)
        {
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                _answerButtons[i].onClick.RemoveAllListeners();
            }

            _nextPlayerButton.onClick.RemoveAllListeners();

            _controller.OnWaitingForNextQuestionPrompt -= SetStateToWaitingForNextQuestion;
            _controller.OnWaitingForNextPlayer -= SetStateWaitingForNextPlayer;

            _controller = null;
        }
    }
}
