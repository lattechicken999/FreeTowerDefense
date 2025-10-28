using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] GameObject _warriorPrefeb;
    [SerializeField] GameObject _wizardPrefeb;
    [SerializeField] Material _TranslucentMaterial;

    private WaitForNextFrameUnit _delay;
    public bool CreateWarrior(int level=1)
    {
        //prefeb�� ������ ���·� ����� ���콺 ���󰡰� �����
        //Ŭ���� �ش� �ڸ��� ��ġ ������ �ڸ���� �⹰ ���� (������ ���ְ� Piece�� InitalPiece ȣ��)
        // �ش� �ڸ��� ��ġ�� �Ұ����ϴٸ� false ����
        //��ġ�� �����Ͽ� ���������� ������ �Ǿ��ٸ� true ����
        return true;
    }

    public bool CreateWizard(int level =1)
    {
        //prefeb�� ������ ���·� ����� ���콺 ���󰡰� �����
        //Ŭ���� �ش� �ڸ��� ��ġ ������ �ڸ���� �⹰ ���� (������ ���ְ� Piece�� InitalPiece ȣ��)
        // �ش� �ڸ��� ��ġ�� �Ұ����ϴٸ� false ����
        //��ġ�� �����Ͽ� ���������� ������ �Ǿ��ٸ� true ����
        return true;
    }

    private IEnumerator SetupPieceStandby(GameObject prefeb)
    {
        var newPiece = Instantiate(prefeb);

        while(true)
        {
            //���̷� ������ �� ������
            //Ŭ�� �� �� ���� �ݺ�, ���콺�� ������Ʈ�� ����ٴ�.
            yield return _delay;
        }
    }
}
