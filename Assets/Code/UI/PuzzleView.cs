using UnityEngine;
using System.Collections;

public class PuzzleView : MonoBehaviour
{
    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    [SerializeField]
    private PuzzleWordView[] _puzzleWordViews;

    [SerializeField]
    private AnswerView[] _answerViews;

    [SerializeField]
    private Color[] _answerColors;

    public virtual void SetTeamData(TeamData[] teams)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            _teamDataDisplays[i].Data = teams[i];
        }
    }

    public virtual void SetActiveTeam(int index, bool counting)
    {
        for (int i = 0; i < _teamDataDisplays.Length; i++)
        {
            _teamDataDisplays[i].SetState(i == index, counting);
        }
    }

    public virtual void SetPuzzleWords(string[] puzzleWords)
    {
        for (int i = 0; i < puzzleWords.Length && i < _puzzleWordViews.Length; i++)
        {
            _puzzleWordViews[i].SetWord(puzzleWords[i]);
        }
    }

    public virtual void SetAnswers(int[] answerScores, string[] answers)
    {
        for (int i = 0; i < answers.Length && i < _answerViews.Length; i++)
        {
            _answerViews[i].SetAnswer(answerScores[i], answers[i], _answerColors[i]);
        }
    }

    public virtual void ShowAnswer(int answerIndex, int[] puzzleWordIndeces, bool showScore)
    {
        _answerViews[answerIndex].ShowAnswer(showScore);

        Color answerColor = _answerColors[answerIndex];

        for (int i = 0; i < puzzleWordIndeces.Length; i++)
        {
            _puzzleWordViews[puzzleWordIndeces[i]].SetColor(answerColor);
        }
    }
}
