using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.05f;
    public Vector3 offset = new Vector3(0, -1f, -2); // 2.7f = ground��ġ�� ���� ī�޶� ����
    public Transform limitLeft;
    public Transform limitRight;


    private Vector3 velocity = Vector3.zero;
    public float FixedY =-1;
    private void Start()
    {
     
        FixedY = -1f; //Stage1 Ground ����
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

    public void CameraGoUp()
    {     
        FindObjectOfType<PlayerMove>().ButtonUp();
    }
}

