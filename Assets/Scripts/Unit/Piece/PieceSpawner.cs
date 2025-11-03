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
   

    public bool CreateWarrior(Transform placeTransform,int level=1)
    {
        if(GoldManager.Instance.IsPossibleUnitBuy(UnitEnum.Warrior))
        {
            GameObject piece = Instantiate(_warriorPrefeb, placeTransform.position, Quaternion.identity);
            piece.GetComponent<Piece>().SetPlaceTransform(placeTransform);
            //piece.GetComponent<Piece>().InifalPiece();
            return true;
        }
        return false;
    }

    public bool CreateWizard( Transform placeTransform, int level = 1)
    {
        if (GoldManager.Instance.IsPossibleUnitBuy(UnitEnum.Wizard))
        {
            GameObject piece = Instantiate(_wizardPrefeb, placeTransform.position, Quaternion.identity);
            piece.GetComponent<Piece>().SetPlaceTransform(placeTransform);
            //piece.GetComponent<Piece>().InifalPiece();
            return true;
        }
        return false;
    }
   
}
