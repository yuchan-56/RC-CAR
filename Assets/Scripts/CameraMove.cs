using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.05f;
    public Vector3 offset = new Vector3(0, 2.7f, -2); // 2.7f = ground��ġ�� ���� ī�޶� ����

    public float minX = -3.8f;
    public float maxX = 1.9f;

    private Vector3 velocity = Vector3.zero;
    private float FixedY;
    private Vector3 targetPosition;
    bool cameraMoving = false; // ĳ���Ͱ� �����̵��Ҷ��� ī�޶��̵� ����

    private void Start()
    {
        FixedY = -0.534f; //Stage1 Ground ����
        targetPosition = transform.position; // �ʱ�ȭ
    }
    void LateUpdate()
    {
        if (cameraMoving)
        {
            Managers.Game.currentState = GameManager.GameState.CameraMoving; // ī�޶� �̵����̹Ƿ� ĳ���� �̵��Ұ�

            Vector3 followPosition = player.position + offset;
            followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);
            followPosition.y = FixedY;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * 20f);

            // ��ǥ ��ġ�� �����ϸ� ������ ����
            if (Vector3.Distance(transform.position, followPosition) < 0.01f)
            {
                cameraMoving = false;
                Managers.Game.currentState = GameManager.GameState.Battle; // ī�޶� �̵����̳������Ƿ� �̵�����.
            }
        }
        else
        {
            // �÷��̾� ���󰡱�
            Vector3 followPosition = player.position + offset;
            followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);
            followPosition.y = FixedY;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref velocity, smoothSpeed * Time.deltaTime);
        }
    }

    public void CameraUpdate(float x) // x�� �����̵��� ĳ������ x��
    {
        // ���ο� ��ǥ ��ġ ����
        minX += 30; // BackGround������ �Ÿ�
        maxX += 30;

        // targetPosition = new Vector3(x+offset.x, FixedY, transform.position.z); // �����̵��� ĳ������ġ�� ī�޶� ����,offset�� ī�޶� ������ ĳ���Ϳ��Ը� �پ������ʰ� �ڿ������� �̵��ϱ�����
        cameraMoving = true;

        FindObjectOfType<PlayerMove>().ButtonUp();
    }
}
