using UnityEngine;
using System.Collections;
using System;

public class DoneRound : GameRound
{
    private DoneViewController _view;

    public override string SceneName
    {
        get
        {
            return "Done";
        }
    }

    public override void Start(TeamData[] teams, Question[] questions)
    {
        _view = GameObject.FindObjectOfType<DoneViewController>();
        _view.SetController(this);
        _view.SetTeamData(teams);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
