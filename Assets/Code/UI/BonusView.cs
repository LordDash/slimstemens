using UnityEngine;
using System.Collections;

public class BonusView : MonoBehaviour
{
    [SerializeField]
    private TeamDataDisplay[] _teamDataDisplays;

    public virtual void SetTeamData(TeamData[] teams)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            _teamDataDisplays[i].Data = teams[i];
        }
    }
}
