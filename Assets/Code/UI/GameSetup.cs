using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private InputField[] _teamNameInputs;

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

        TeamData[] teams = new TeamData[_teamNameInputs.Length];

        for (int i = 0; i < _teamNameInputs.Length; i++)
        {
            teams[i] = new TeamData() { Name = _teamNameInputs[i].text, Time = 60 };
        }

        GameManager.Start(game, teams);
    }
}
