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
   
}
