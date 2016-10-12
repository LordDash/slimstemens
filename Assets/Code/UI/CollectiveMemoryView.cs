using UnityEngine;
using System.Collections;

public class CollectiveMemoryView : MonoBehaviour
{
	private readonly static int VisibleHash = Animator.StringToHash("Visible");
	
    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;
    
    [SerializeField]
    private AnswerView[] _answers;
    
    [SerializeField]
    private VideoPlayer _videoPlayer;
    
    [SerializeField]
    private Animator _videoAnimator;

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
    
    public virtual void SetQuestion(string[] answers, MovieTexture video)
    {
    	for(int i = 0; i < _answers.Length && i < answers.Length; i++)
    	{
    		_answers[i].SetAnswer(-1, answers[i]);
    	}
    	
    	_videoPlayer.SetMovie(video, null);
    	_videoAnimator.SetBool(VisibleHash, false);
    }
    
    public virtual void ShowAnswer(int answerIndex, int score, bool showScore = true)
    {
    	_answers[answerIndex].ShowAnswer(showScore, score);
    }
    
    public virtual void PlayVideo()
    {
    	_videoAnimator.SetBool(VisibleHash, true);
    	_videoPlayer.Play();
    }
    
    public virtual void StopVideo()
    {
    	_videoAnimator.SetBool(VisibleHash, false);
    	_videoPlayer.Stop();
    }
}