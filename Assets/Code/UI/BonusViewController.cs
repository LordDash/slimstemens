using System;
using UnityEngine;
using UnityEngine.UI;

public class BonusViewController : BonusView
{
    [SerializeField]
    private BonusView _playerView;

    [SerializeField]
    private InputField[] _bonusScoreField;

    [SerializeField]
    private Button _addTimeButton;

    [SerializeField]
    private Button _nextRoundButton;

    [SerializeField]
    private Text _error;

    private BonusRound _controller;

    public void SetController(BonusRound controller)
    {
        _controller = controller;

        _nextRoundButton.onClick.AddListener(_controller.NextRound);
        _nextRoundButton.interactable = false;
        _addTimeButton.onClick.AddListener(AddTime);
        _addTimeButton.interactable = true;
    }

    private void AddTime()
    {
        try
        {
            int[] bonusTime = new int[_bonusScoreField.Length];

            for (int i = 0; i < bonusTime.Length; i++)
            {
                bonusTime[i] = int.Parse(_bonusScoreField[i].text);
            }

            _addTimeButton.interactable = false;
            _nextRoundButton.interactable = true;

            _controller.AddTime(bonusTime);
        }
        catch (FormatException)
        {
            _error.text = "Please make sure all fields contain a number.";
        }
    }

    public override void SetTeamData(TeamData[] teams)
    {
        base.SetTeamData(teams);

        _playerView.SetTeamData(teams);
    }
}