using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stage/Stage Monster Data",fileName ="StageMonsterData")]
public class StageDataSO : ScriptableObject
{
    //����Ƽ���� �����ϴ� ScriptableObject�� �̿��ؼ� Inspector���� Stage������ ������ �� �ֵ��� ����
    public int stageNumber;
    public int spawnDelay;
    public List<GoldManager.MonsterNameEnum> monsterInfoList;
}
