using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeManager : MonoSingleton<TimeManager> 
{
    private List<Action<float>> _timerActions = new List<Action<float>>();

    public void AddTimer(Action<float> timer)
    {
        _timerActions.Add(timer);
    }

    public void RemoveTimer(Action<float> timer)
    {
        _timerActions.Remove(timer);
    }

    private void Update()
    {
        _timerActions.RemoveAll(t => t == null || t.Target == null);

        for (int i = 0; i < _timerActions.Count; i++)
        {
            _timerActions[i].Invoke(Time.deltaTime);
        }
    }
}
