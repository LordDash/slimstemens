using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleWordView : MonoBehaviour
{
    [SerializeField]
    private Text _wordText;

    public void SetWord(string word)
    {
        _wordText.text = word;
        _wordText.color = Color.white;
    }

    public void SetColor(Color color)
    {
        _wordText.color = color;
    }
}
