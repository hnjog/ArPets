using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공 함수
// 유저가 던지고 받아칠 때 보는 방향으로 날아간다. (또한 공 파괴 가능 시간이 초기화 됨)
// 벨루가를 만나면 유저를 향해서 공을 날림
// 타이밍에 맞추어?? 친다.
// 벨루가가 공에 맞게 되면, 유저 쪽으로 팅겨나오면서 코루틴이 멈추며,
// 유저가 공을 다시 받아치면 코루틴을 새로 시작한다.(단, 유저가 공을 못받는 경우가 필요함)

public class Ball : MonoBehaviour
{
    Rigidbody b_rigid = null;               // 리지드바디

    GameObject tf = null;                   // 던지는 곳
    GameObject player = null;               // 플레이어
    GameObject veluga = null;               // 벨루가

    IEnumerator ballCorutine;

    Vector3 BallVelo;                       // 방향
    float degree = 30f;                     // 공 던지는 각도

    // 작동 될 때
    private void OnEnable()
    {
        if (b_rigid == null)
        {
            b_rigid = GetComponent<Rigidbody>();
            tf = GameObject.Find("LookChecker");
            player = GameObject.Find("Camera Container");
            veluga = GameObject.Find("BelugaAxis");
        }

        // off 한 후의 힘 초기화
        b_rigid.velocity = Vector3.zero;
        BallVelo = Throwing(transform.position, tf.transform.position, degree);
        Throw(BallVelo);

        ballCorutine = DestroyBall();
        StartCoroutine(ballCorutine);
    }

    // 받아치는 함수, 타이밍 등의 조건 추가 예정임.(타이밍에 맞게 치지 못한 경우, fail 카운트가 늘어나 실패로 규정이 됨)
    public void Rebound()
    {
        b_rigid.velocity = Vector3.zero;
        BallVelo = Throwing(transform.position, tf.transform.position, degree);
        Throw(BallVelo);
        AI.success += 1;

        // 코루틴 초기화
        
        StartCoroutine(ballCorutine);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Pet")
        {
            b_rigid.velocity = Vector3.zero;
            BallVelo = Throwing(veluga.transform.position, player.transform.position, degree);                 // 벨루가가 플레이어에게 볼을 던짐
            Throw(BallVelo);
            StopCoroutine(ballCorutine);
        }
    }

    // x초 뒤에 사라짐(유저가 먹이를 벨루가에게 못 준 경우)
    IEnumerator DestroyBall()
    {
        if (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(5f);
            ObjectManager.instance.B_Recovery(gameObject);
            AI.fail += 1;
        }
        else
        {
            yield return null;
        }
    }

    // 던지는 용도
    void Throw(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }

    // 던지는 velocity 값 구하기 - 긁어온 코드(기준, 목표, 날리는 각도)
    Vector3 Throwing(Vector3 transformPos, Vector3 targetPos, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 vTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 vPosition = new Vector3(transformPos.x, 0, transformPos.z);

        float distance = Vector3.Distance(vTarget, vPosition);
        float yOffset = transformPos.y - targetPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, vTarget - vPosition) * (targetPos.x > transformPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
