using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerView : MonoBehaviour
{
    private static readonly int VisibleHash = Animator.StringToHash("Visible");

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Text _blurredAnswerText;

    [SerializeField]
    private Text _answerText;

    [SerializeField]
    private Text _scoreText;

    public void SetAnswer(int score, string answer, Color color = default(Color))
    {
        _blurredAnswerText.text = answer;
        _answerText.text = answer;

        _answerText.color = color;

        _scoreText.text = score.ToString();

        _animator.SetBool(VisibleHash, false);
    }

    public void ShowAnswer(bool showScore, int score = -1)
    {
        if (showScore == false)
        {
            _scoreText.text = "";
        }
        else if (score != -1)
        {
            _scoreText.text = score.ToString();
        }

        _animator.SetBool(VisibleHash, true);
    }
}
