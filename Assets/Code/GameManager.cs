using UnityEngine;
using System.Collections;

public static class GameManager 
{
    private static TeamData[] _teams;

    private static int _currentRoundIndex = -1;
    private static Game _game;

    private static Round CurrentRound
    {
        get { return _game.GetRound(_currentRoundIndex); }
    }

    private static T[] GetCurrentRoundQuestions<T>() where T : Question
    {
        return _game.GetQuestionsForRound<T>(_currentRoundIndex);
    }

    public static void Start(Game game, TeamData[] teams)
    {
        _game = game;
        _teams = teams;

        NextRound();
    }

    public static void NextRound()
    {
        ++_currentRoundIndex;

        Question[] questions = null;
        GameRound currentGameRound = null;

        switch (CurrentRound)
        {
            case Round.ThreeSixNine:
                currentGameRound = new ThreeSixNineRound();
                questions = GetCurrentRoundQuestions<ThreeSixNineQuestion>();
                break;
            case Round.OpenDoor:
                currentGameRound = new OpenDoorRound();
                questions = GetCurrentRoundQuestions<OpenDoorQuestion>();
                break;
            case Round.Puzzle:
                currentGameRound = new PuzzleRound();
                questions = GetCurrentRoundQuestions<PuzzleQuestion>();
                break;
            case Round.Gallery:
                currentGameRound = new GalleryRound();
                questions = GetCurrentRoundQuestions<GalleryQuestion>();
                break;
            case Round.CollectiveMemory:
                currentGameRound = new CollectiveMemoryRound();
                questions = GetCurrentRoundQuestions<CollectiveMemoryQuestion>();
                break;
            case Round.Finale:
                currentGameRound = new FinaleRound();
                questions = GetCurrentRoundQuestions<FinaleQuestion>();
                break;
            case Round.Bonus:
                currentGameRound = new BonusRound();
                break;
            case Round.Done:
                currentGameRound = new DoneRound();
                break;
        }

        SceneManager.Instance.Load(currentGameRound.SceneName, () => currentGameRound.Start(_teams, questions));
    }
}

public class TeamData
{
    public string Name;
    public float Time;
}

public enum Round
{
    ThreeSixNine = 0,
    OpenDoor = 1,
    Puzzle = 2,
    Gallery = 4,
    CollectiveMemory = 5,
    Finale = 6,
    Bonus = 7,
    Done = 8
}