using System;

public abstract class GameRound
{
    public abstract string SceneName
    {
        get;
    }
    public abstract void Start(TeamData[] teams);
}