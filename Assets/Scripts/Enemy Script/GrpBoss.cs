using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrpBoss : Boss
{
    //attack
    public GameObject bulletPrefab; // 총알 프리팹
    public float bulletSpeed = 13f; // 총알 속도
    public float fireRate = 1.0f;

    Vector3 firePoint = new Vector3(0, 2.0f, 0);

    //p1
    public GameObject[] p1Object = new GameObject[3];
    public float rotationSpeed = 500f; // 회전 속도
    public float fallDelay = 2f; // 회전 후 낙하하는 시간
    public float fallStartY = 10f;
    public Vector3[] spawnPositions = new Vector3[3]
    {
        new Vector3(-2f, 3f, 0),  // 왼쪽 위
        new Vector3(0f, 4.5f, 0),   // 정 중앙 위
        new Vector3(2f, 3f, 0)    // 오른쪽 위
    };

    public Vector3[] fallStartPositions = new Vector3[3]
    {
        new Vector3(-7f, 10f, 0),  // 왼쪽에서 낙하
        new Vector3(-5f, 10f, 0),   // 중앙에서 낙하
        new Vector3(-3f, 10f, 0)    // 오른쪽에서 낙하
    };
    private List<GameObject> spawnedObjects = new List<GameObject>();


    //p3
    public GameObject p3Object;
    public Vector3 beamPos = new Vector3(-8f, 0, 0);
    private GameObject newObj;

    protected override void Start()
    {
        base.Start();
        Debug.Log("그래픽 보스");
    }
   

    public override void Attack() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isStop", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);

        StartCoroutine(ShootBullets(3));
    }


    IEnumerator ShootBullets(int shotCount)
    {
        for (int i = 0; i < shotCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate); // ✅ 발사 간격 유지
        }

        // ✅ 3발 발사 후 다시 Wandering 또는 Following 상태로 복귀
        animator.SetBool("isAttack", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;
    }

    void Shoot()
    {
        if (bulletPrefab == null || player == null) return; // 플레이어가 없으면 실행 X

        // ✅ 총알 생성 (firePoint에서 발사)
        GameObject bullet = Instantiate(bulletPrefab, transform.position + firePoint, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            // ✅ 플레이어 방향으로 총알 발사
            Vector2 direction = (player.position - transform.position).normalized; // 방향 벡터 계산
            bulletRb.velocity = direction * bulletSpeed; // 방향 적용
        }
    }



    public override void P1() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isStop", false);
        animator.SetBool("isP1", true);

        
        StartCoroutine(SpawnRotateAndFallObjects());
    }

    IEnumerator SpawnRotateAndFallObjects()
    {
        for (int i = 0; i < p1Object.Length; i++)
        {
            if (p1Object[i] != null)
            {
                // 보스를 기준으로 지정된 위치에 오브젝트 생성
                Vector3 spawnPosition = transform.position + spawnPositions[i];
                GameObject newObj = Instantiate(p1Object[i], spawnPosition, Quaternion.identity);

                spawnedObjects.Add(newObj);
                StartCoroutine(RotateObject(newObj));
            }
        }

        // 일정 시간 대기 후 낙하 시작
        yield return new WaitForSeconds(fallDelay);

        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i] != null)
            {
                StartCoroutine(FallObject(spawnedObjects[i], fallStartPositions[i]));
            }
        }
    }
    IEnumerator RotateObject(GameObject obj)
    {
        float rotationTime = fallDelay;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            obj.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FallObject(GameObject obj, Vector3 fallStartPos)
    {
        float fallSpeed = 10f;
        float scaleMultiplier = 1.5f;

        obj.transform.position = transform.position + fallStartPos;
        obj.transform.localScale *= scaleMultiplier;

        while (obj.transform.position.y > transform.position.y - 4f) // 땅까지 떨어질 때까지
        {
            obj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(obj); // 바닥에 닿으면 삭제

        spawnedObjects.Clear();
        animator.SetBool("isP1", false);
        
        isWandering = true;
        isFollowing = true;
        isStop = true;
    }


    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        // 피회복
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isStop", false);
        animator.SetBool("isP2", true);

        StartCoroutine(GetHP());
    }

    IEnumerator GetHP() {
        float healAmount = 5f; // 초당 회복량
        float healDuration = 5f; // 회복 지속 시간
        float elapsedTime = 0f;

        while (elapsedTime < healDuration) {
            // 부모 클래스(Boss)의 hp 증가
            hp += healAmount * Time.deltaTime;

            if(hp > 100) {
                hp = 100;
            }
            
            // hpSlider UI 업데이트
            if (hpSlider != null) {
                hpSlider.value = hp;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회복 종료 후 애니메이션 리셋
        animator.SetBool("isP2", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;
    }



    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isStop", false);

        StartCoroutine(Delaying(0.95f));
        StartCoroutine(ShootBeam());
    }

    IEnumerator Delaying(float sec) {
        yield return new WaitForSeconds(sec);

        Vector3 spawnPosition = transform.position + beamPos;
        Quaternion rotation = Quaternion.Euler(0, 0, 7);

        newObj = Instantiate(p3Object, spawnPosition, rotation);
    }

    IEnumerator ShootBeam() {
        Invoke("DestroyBeam", 2f);
        
        yield return new WaitForSeconds(2.4f);

        animator.SetBool("isP3", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;
    }

    void DestroyBeam()
    {
        Destroy(newObj);
    }
}
