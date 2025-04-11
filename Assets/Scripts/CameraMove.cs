using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.05f;
    public Vector3 offset = new Vector3(0, -1f, -2); // 2.7f = ground��ġ�� ���� ī�޶� ����

    public float minX = -3f;
    public float maxX = 1.9f;

    public Transform limitLeft;
    public Transform limitRight;


    private Vector3 velocity = Vector3.zero;
    public float FixedY =-1;
    private Vector3 targetPosition;
    private void Start()
    {
        FixedY = -1f; //Stage1 Ground ����
        targetPosition = transform.position; // �ʱ�ȭ
    }
    void LateUpdate()
    {

        Vector3 followPosition = player.position + offset;
        followPosition.y = FixedY;

        // �ε巴�� �̵�
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * Time.deltaTime);

        // ī�޶� ���� �� ��ǥ ���ϱ�
        Vector3 viewPortLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));

        // ���� ���� ó��
        if (viewPortLeft.x < limitLeft.position.x)
        {
            float diff = limitLeft.position.x - viewPortLeft.x;
            transform.position += new Vector3(diff, 0, 0);
        }

        // ī�޶� ������ �� ��ǥ ���ϱ�
        Vector3 viewPortRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // ������ ���� ó��
        if (viewPortRight.x > limitRight.position.x)
        {
            float diff = viewPortRight.x - limitRight.position.x;
            transform.position -= new Vector3(diff, 0, 0);
        }
    }

    public void CameraUpdate(float x) // x�� �����̵��� ĳ������ x��
    {
        // ���ο� ��ǥ ��ġ ����
        minX += 30; // BackGround������ �Ÿ�
        maxX += 30;

        // targetPosition = new Vector3(x+offset.x, FixedY, transform.position.z); // �����̵��� ĳ������ġ�� ī�޶� ����,offset�� ī�޶� ������ ĳ���Ϳ��Ը� �پ������ʰ� �ڿ������� �̵��ϱ�����
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

