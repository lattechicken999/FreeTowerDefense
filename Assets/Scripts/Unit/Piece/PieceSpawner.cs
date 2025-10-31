using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceSpawner : Singleton<PieceSpawner>
{
    [SerializeField] GameObject _warriorPrefeb;
    [SerializeField] GameObject _wizardPrefeb;
   

    public bool CreateWarrior(Vector3 position,int level=1)
    {
        if(GoldManager.Instance.IsPossibleUnitBuy(UnitEnum.Warrior))
        {
            GameObject piece = Instantiate(_warriorPrefeb, position, Quaternion.identity);
            //piece.GetComponent<Piece>().InifalPiece();
            return true;
        }
        return false;
    }

    public bool CreateWizard(Vector3 position, int level = 1)
    {
        if (GoldManager.Instance.IsPossibleUnitBuy(UnitEnum.Wizard))
        {
            GameObject piece = Instantiate(_wizardPrefeb, position, Quaternion.identity);
            //piece.GetComponent<Piece>().InifalPiece();
            return true;
        }
        return false;
    }
   
}
