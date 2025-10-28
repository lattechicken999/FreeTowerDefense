using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] GameObject _warriorPrefeb;
    [SerializeField] GameObject _wizardPrefeb;

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

}
