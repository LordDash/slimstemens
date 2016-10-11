using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GalleryView : MonoBehaviour
{
    private static readonly int RevealHash = Animator.StringToHash("Reveal");
    private static readonly int HiddenHash = Animator.StringToHash("Hidden");

    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    [SerializeField]
    private Animator _questionAnimator;

    [SerializeField]
    private Image _questionImage;

    [SerializeField]
    private Image _questionOriginalImage;

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

    public virtual void ShowQuestion(int currentQuestionIndex, Sprite questionImage, Sprite questionOriginalImage, string answer, bool reveal = false)
    {
        _questionAnimator.SetBool(RevealHash, false);

        _questionImage.overrideSprite = questionImage;
        _questionOriginalImage.overrideSprite = questionOriginalImage;

        if (questionOriginalImage != null && reveal)
        {
            StartCoroutine(WaitHalfSecondAndReveal());
        }
        
        _questionAnimator.SetBool(HiddenHash, false);
    }

    private IEnumerator WaitHalfSecondAndReveal()
    {
        yield return new WaitForSeconds(.5f);
        _questionAnimator.SetBool(RevealHash, true);
    }

    public virtual void ClearQuestion()
    {
        _questionImage.sprite = null;
        _questionImage.overrideSprite = null;

        _questionOriginalImage.sprite = null;
        _questionOriginalImage.overrideSprite = null;

        _questionAnimator.SetBool(RevealHash, false);
        _questionAnimator.SetBool(HiddenHash, true);
    }
}
