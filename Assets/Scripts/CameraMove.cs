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

    private void Start()
    {
        FixedY = -0.534f; //Stage1 Ground 기준
    }
    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = FixedY;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed * Time.deltaTime);
    }

    public void CameraUpdate()
    {
        minX += 30; // BackGround끼리의 거리
        maxX += 30;
    }
}
