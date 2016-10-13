using UnityEngine;
using System.Collections;

public class FinaleView : MonoBehaviour
{	
    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;
    
    [SerializeField]
    private AnswerView[] _answers;

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
    
    public virtual void SetAnswers(string question, int[] scores, string[] answers)
    {
    	for(int i = 0; i < _answers.Length && i < answers.Length; i++)
    	{
    		_answers[i].SetAnswer(scores[i], answers[i]);
    	}
    }
    
    public virtual void ShowAnswer(int answerIndex, bool showScore = true)
    {
    	_answers[answerIndex].ShowAnswer(showScore);
    }
}