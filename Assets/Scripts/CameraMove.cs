using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.05f;
    public Vector3 offset = new Vector3(0, -1f, -2); // 2.7f = ground위치에 따른 카메라 조절

    public float minX = -3f;
    public float maxX = 1.9f;

    public Transform limitLeft;
    public Transform limitRight;


    private Vector3 velocity = Vector3.zero;
    public float FixedY =-1;
    private Vector3 targetPosition;
    private void Start()
    {
        FixedY = -1f; //Stage1 Ground 기준
        targetPosition = transform.position; // 초기화
    }
    void LateUpdate()
    {

        Vector3 followPosition = player.position + offset;
        followPosition.y = FixedY;

        // 부드럽게 이동
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * Time.deltaTime);

        // 카메라 왼쪽 끝 좌표 구하기
        Vector3 viewPortLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));

        // 왼쪽 제한 처리
        if (viewPortLeft.x < limitLeft.position.x)
        {
            float diff = limitLeft.position.x - viewPortLeft.x;
            transform.position += new Vector3(diff, 0, 0);
        }

        // 카메라 오른쪽 끝 좌표 구하기
        Vector3 viewPortRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // 오른쪽 제한 처리
        if (viewPortRight.x > limitRight.position.x)
        {
            float diff = viewPortRight.x - limitRight.position.x;
            transform.position -= new Vector3(diff, 0, 0);
        }
    }

    public void CameraUpdate(float x) // x는 순간이동한 캐릭터의 x축
    {
        // 새로운 목표 위치 설정
        minX += 30; // BackGround끼리의 거리
        maxX += 30;

        // targetPosition = new Vector3(x+offset.x, FixedY, transform.position.z); // 순간이동한 캐릭터위치로 카메라 무빙,offset은 카메라 무빙이 캐릭터에게만 붙어있지않고 자연스럽게 이동하기위함
        FindObjectOfType<PlayerMove>().ButtonUp();
    }
    
    public void CameraGoUp()
    {     
        FindObjectOfType<PlayerMove>().ButtonUp();
    }
    public void CameraGoNext()
    {
        Vector3 followPosition = player.position + offset;
        followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);
        followPosition.y = FixedY;
        transform.position = followPosition;
    }

}

