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
        //prefeb을 반투면 상태로 만들어 마우스 따라가게 만들기
        //클릭시 해당 자리에 설치 가능한 자리라면 기물 생성 (반투명 없애고 Piece의 InitalPiece 호출)
        // 해당 자리에 설치가 불가능하다면 false 리턴
        //설치가 가능하여 정상적으로 마무리 되었다면 true 리턴
        return true;
    }

    public bool CreateWizard(int level =1)
    {
        //prefeb을 반투면 상태로 만들어 마우스 따라가게 만들기
        //클릭시 해당 자리에 설치 가능한 자리라면 기물 생성 (반투명 없애고 Piece의 InitalPiece 호출)
        // 해당 자리에 설치가 불가능하다면 false 리턴
        //설치가 가능하여 정상적으로 마무리 되었다면 true 리턴
        return true;
    }

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
}
