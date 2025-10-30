using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stage/Stage Monster Data",fileName ="StageMonsterData")]
public class StageDataSO : ScriptableObject
{
    //유니티에서 제공하는 ScriptableObject를 이용해서 Inspector에서 Stage정보를 설정할 수 있도록 설정
    public int stageNumber;
    public int spawnDelay;
    public List<GoldManager.MonsterNameEnum> monsterInfoList;
}
