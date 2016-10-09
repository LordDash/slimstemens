using UnityEngine;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _startButton;

    [SerializeField]
    private TextAsset gameFile;

    private void Awake()
    {
#if !UNITY_EDITOR
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
#endif

        _startButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        Game game = JsonUtility.FromJson<Game>(gameFile.text);

        GameManager.Start(game, new[] { new TeamData() { Name = "Stijn", Time = 60 }, new TeamData() { Name = "Dave", Time = 60 }, new TeamData() { Name = "Kim", Time = 60 } });
    }
}
