using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.05f;
    public Vector3 offset = new Vector3(0, 2.7f, -2); // 2.7f = ground위치에 따른 카메라 조절

    public float minX = -3.8f;
    public float maxX = 1.9f;

    private Vector3 velocity = Vector3.zero;
    private float FixedY;
    private Vector3 targetPosition;
    bool cameraMoving = false; // 캐릭터가 순간이동할때의 카메라이동 관리

    private void Start()
    {
        FixedY = -0.534f; //Stage1 Ground 기준
        targetPosition = transform.position; // 초기화
    }
    void LateUpdate()
    {
        if (cameraMoving)
        {
            Managers.Game.currentState = GameManager.GameState.CameraMoving; // 카메라가 이동중이므로 캐릭터 이동불가

            Vector3 followPosition = player.position + offset;
            followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);
            followPosition.y = FixedY;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * 20f);

            // 목표 위치에 도달하면 움직임 멈춤
            if (Vector3.Distance(transform.position, followPosition) < 0.01f)
            {
                cameraMoving = false;
                Managers.Game.currentState = GameManager.GameState.Battle; // 카메라가 이동중이끝났으므로 이동가능.
            }
        }
        else
        {
            // 플레이어 따라가기
            Vector3 followPosition = player.position + offset;
            followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);
            followPosition.y = FixedY;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * Time.deltaTime);
        }
    }

    public void CameraUpdate(float x) // x는 순간이동한 캐릭터의 x축
    {
        // 새로운 목표 위치 설정
        minX += 30; // BackGround끼리의 거리
        maxX += 30;

        // targetPosition = new Vector3(x+offset.x, FixedY, transform.position.z); // 순간이동한 캐릭터위치로 카메라 무빙,offset은 카메라 무빙이 캐릭터에게만 붙어있지않고 자연스럽게 이동하기위함
        cameraMoving = true;

        FindObjectOfType<PlayerMove>().ButtonUp();
    }
}
