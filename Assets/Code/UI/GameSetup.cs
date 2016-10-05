using UnityEngine;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _startButton;

	private void Awake ()
    {
        _startButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        GameManager.Start(new[] { Round.ThreeSixNine }, new[] { new TeamData() { Name = "Stijn" }, new TeamData() { Name = "Dave" }, new TeamData() { Name = "Kim" } });
    }
}
