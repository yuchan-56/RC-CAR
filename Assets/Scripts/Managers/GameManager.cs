using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    public float damage = 50;
    public int gage = 0;
    public bool GetHit = false;

    

    //게임 상태를 나눠서 상태에 따라 스크립트들이 돌아가게 함
    public enum GameState
    {
        CameraMoving,
        Battle,
        Store,
        Bless,

    }
    public GameState currentState;
    //플레이어 죽을 때 실행시킬 함수
    public void PlayerDied()
    {
       
    }
    //인게임 데이터 초기화 
    public void GameStart()
    {
        currentState = GameState.Battle;
    }

    public void GoJump()
    {
        
    }

    public void UltimateDamageUp()
    {
        damage = 1000;
    }

}
