using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWayPoint : MonoBehaviour
{
    private void Start()
    {
        SetMonsterManagerWayPointInfo();
    }
    private void OnDestroy()
    {
        UnSetMonsterManagerWayPointInfo();
    }
    private void SetMonsterManagerWayPointInfo()
    {
        MonsterManager.Instance.SetWayPointInfo(this);
    }
    private void UnSetMonsterManagerWayPointInfo()
    {
        MonsterManager.Instance.UnSetWayPointInfo();
    }
}
