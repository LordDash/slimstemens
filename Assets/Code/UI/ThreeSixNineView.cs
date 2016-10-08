using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThreeSixNineView : MonoBehaviour
{
    private static readonly int ActiveHash = Animator.StringToHash("Active");

    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    [SerializeField]
    private Text _questionField;

    [SerializeField]
    private Text _answerField;

    [SerializeField]
    private Animator[] _questionAnimators;

    public virtual void SetTeamData(TeamData[] teams)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            _teamDataDisplays[i].Data = teams[i];
        }
    }

    public virtual void SetQuestion(int index, string question, string answer)
    {
        _questionField.text = question;
        _answerField.text = answer;

        for (int i = 0; i < _questionAnimators.Length; i++)
        {
            _questionAnimators[i].SetBool(ActiveHash, i == index);
        }
    }

    public virtual void SetAnswer(string answer)
    {
        _answerField.text = answer;
    }

    public virtual void SetActiveTeam(int index)
    {
        for (int i = 0; i < _teamDataDisplays.Length; i++)
        {
            _teamDataDisplays[i].SetState(i == index, false);
        }
    }
}
