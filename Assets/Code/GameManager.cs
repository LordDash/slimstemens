using UnityEngine;
using System.Collections;

public static class GameManager 
{
    private static Round[] _rounds;
    private static TeamData[] _teams;

    private static int _currentRoundIndex = -1;
    static GameRound _currentGameRound;

    private static Round CurrentRound
    {
        get { return _rounds[_currentRoundIndex]; }
    }

    public static void Start(Round[] rounds, TeamData[] teams)
    {
        _rounds = rounds;
        _teams = teams;

        NextRound();
    }

    public static void NextRound()
    {
        ++_currentRoundIndex;

        switch (CurrentRound)
        {
            case Round.ThreeSixNine:
                _currentGameRound = new ThreeSixNineRound();
                break;
            case Round.OpenDoor:
                _currentGameRound = new OpenDoorRound();
                break;
            case Round.Puzzle:
                _currentGameRound = new PuzzleRound();
                break;
        }

        SceneManager.Instance.Load(_currentGameRound.SceneName, () => _currentGameRound.Start(_teams));
    }
}

public class TeamData
{
    public string Name;
    public float Time;
}

public enum Round
{
    ThreeSixNine,
    OpenDoor,
    Puzzle,
    Framed,
    Gallery,
    CollectiveMemory,
    Final,
    Bonus,
    Done,
    Result
}