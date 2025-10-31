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
    //[SerializeField] Material _TranslucentMaterial;

    private WaitForNextFrameUnit _delay;
    public void CreateWarrior(Vector3 position,int level=1)
    {
        GameObject piece = Instantiate(_warriorPrefeb, position, Quaternion.identity);
        //piece.GetComponent<Piece>().InifalPiece();
      
    }

    public void CreateWizard(Vector3 position, int level = 1)
    {
        GameObject piece = Instantiate(_wizardPrefeb, position, Quaternion.identity);
        //piece.GetComponent<Piece>().InifalPiece();
    }
    /*
    private IEnumerator SetupPieceStandby(GameObject prefeb)
    {
        var newPiece = Instantiate(prefeb);

        while(true)
        {
            //레이로 포지션 값 가져옴
            //클릭 할 때 까지 반복, 마우스를 오브젝트가 따라다님.
            yield return _delay;
        }
    }
    */

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as PieceSpawner;
            //DontDestroyOnLoad(gameObject);
            init();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
