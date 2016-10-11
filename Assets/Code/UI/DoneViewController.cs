using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DoneViewController : DoneView
{
    [SerializeField]
    private Button _quitButton;

    [SerializeField]
    private DoneView _playerView;

    private DoneRound _controller;

    public override void SetTeamData(TeamData[] teams)
    {
        base.SetTeamData(teams);

        _playerView.SetTeamData(teams);
    }

    public void SetController(DoneRound controller)
    {
        _controller = controller;
        _quitButton.onClick.AddListener(_controller.QuitGame);
    }
}
