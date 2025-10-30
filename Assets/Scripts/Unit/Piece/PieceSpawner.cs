using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] GameObject _warriorPrefeb;
    [SerializeField] GameObject _wizardPrefeb;
    [SerializeField] Material _TranslucentMaterial;

    private WaitForNextFrameUnit _delay;
    public void CreateWarrior(Vector3 position,int level=1)
    {
        GameObject piece = Instantiate(_warriorPrefeb, position, Quaternion.identity);
        piece.GetComponent<Piece>().InifalPiece();
      
    }

    public void CreateWizard(Vector3 position, int level = 1)
    {
        GameObject piece = Instantiate(_warriorPrefeb, position, Quaternion.identity);
        piece.GetComponent<Piece>().InifalPiece();
    }
    /*
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
    */
}
