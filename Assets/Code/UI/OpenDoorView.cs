using UnityEngine;

public class OpenDoorView : MonoBehaviour
{
    private static readonly int VideoHash = Animator.StringToHash("Video");
    private static readonly int UsedHash = Animator.StringToHash("Used");

    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    [SerializeField]
    private AnswerView[] _answerViews;

    [SerializeField]
    private Animator _videosAnimator;

    [SerializeField]
    private VideoPlayer[] _videoPlayers;

    [SerializeField]
    private Animator[] _videoPlayersAnimators;

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

    public virtual void SetAnswers(string question, int[] answerScores, string[] answers)
    {
        for (int i = 0; i < answers.Length && i < _answerViews.Length; i++)
        {
            _answerViews[i].SetAnswer(answerScores[i], answers[i]);
        }
    }

    public virtual void ClearAnswers()
    {
        _videosAnimator.SetInteger(VideoHash, -1);

        for (int i = 0; i < _answerViews.Length && i < _answerViews.Length; i++)
        {
            _answerViews[i].SetAnswer(-1, "");
        }
    }

    public virtual void ShowAnswer(int answerIndex, bool showScore)
    {
        _answerViews[answerIndex].ShowAnswer(showScore);
    }

    public virtual void SetVideos(MovieTexture[] videos, Sprite[] firstFrames)
    {
        for (int i = 0; i < _videoPlayers.Length && i < _videoPlayers.Length; i++)
        {
            _videoPlayers[i].SetMovie(videos[i], firstFrames[i]);
        }
    }

    public virtual void SetVideoUsed(int questionIndex)
    {
        if (questionIndex != -1)
        {
            _videoPlayersAnimators[questionIndex].SetBool(UsedHash, true);
        }
    }

    public virtual void ShowVideo(int questionIndex)
    {
        _videosAnimator.SetInteger(VideoHash, questionIndex);
        _videoPlayers[questionIndex].Play();
    }
}
