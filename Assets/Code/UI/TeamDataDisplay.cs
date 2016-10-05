using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TeamDataDisplay : MonoBehaviour
{
    [SerializeField]
    private Text _nameField;

    [SerializeField]
    private Text _timeField;

    [SerializeField]
    private Image _activeImage;

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

    public void SetCurrentActive(bool active)
    {
        _activeImage.gameObject.SetActive(active);
    }

    private void OnDestroy()
    {
        _data = null;
    }
}
