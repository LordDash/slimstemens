using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TeamDataDisplay : MonoBehaviour
{
    private static readonly int ActiveHash = Animator.StringToHash("Active");
    private static readonly int CountingHash = Animator.StringToHash("CountDown");

    [SerializeField]
    private Text _nameField;

    [SerializeField]
    private Text _timeField;

    [SerializeField]
    private Animator _animator;

    private TeamData _data;

    public TeamData Data
    {
        set
        {
            _data = value;

            if (_data != null)
            {
                _nameField.text = _data.Name;
            }
        }
    }

    private void Update()
    {
        if (_data != null)
        {
            _timeField.text = Mathf.CeilToInt(_data.Time).ToString();
        }
    }

    public void SetState(bool active, bool counting = true)
    {
        _animator.SetBool(CountingHash, counting);
        _animator.SetBool(ActiveHash, active);
    }

    private void OnDestroy()
    {
        _data = null;
    }
}
