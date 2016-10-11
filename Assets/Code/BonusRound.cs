using UnityEngine;
using System.Collections;
using System;

public class BonusRound : GameRound
{
    private TeamData[] _teams;
    private BonusViewController _view;

    public override string SceneName
    {
        get
        {
            return "Bonus";
        }
    }

    public override void Start(TeamData[] teams, Question[] questions)
    {
        _teams = teams;
        _view = GameObject.FindObjectOfType<BonusViewController>();

        _view.SetController(this);
        _view.SetTeamData(_teams);
    }

    public void AddTime(int[] bonusTime)
    {
        for (int i = 0; i < _teams.Length && i < bonusTime.Length; i++)
        {
            _teams[i].Time += bonusTime[i];
        }
    }

    public void NextRound()
    {
        GameManager.NextRound();
    }
}
