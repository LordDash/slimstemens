﻿using UnityEngine;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _startButton;

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
        GameManager.Start(new[] { Round.ThreeSixNine }, new[] { new TeamData() { Name = "Stijn" }, new TeamData() { Name = "Dave" }, new TeamData() { Name = "Kim" } });
    }
}
