using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] GameObject _warriorPrefeb;
    [SerializeField] GameObject _wizardPrefeb;

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

}
