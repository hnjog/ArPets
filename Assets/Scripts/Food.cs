using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 음식 함수
// 

public class Food : MonoBehaviour
{
    Rigidbody f_rigid = null;               // 리지드바디

    GameObject tf = null;                   // 던지는 곳

    float degree = 30f;                     // 공 던지는 각도

    [SerializeField] AIStateController aiState = null;         // aistatecontroller

    // 작동 될 때
    private void OnEnable()
    {
        if(f_rigid == null)
        {
            f_rigid = GetComponent<Rigidbody>();
            tf = GameObject.Find("LookChecker");
            aiState = GameObject.Find("BelugaAxis").GetComponent<AIStateController>();
        }

        // off 한 후의 힘 초기화
        f_rigid.velocity = Vector3.zero;
        Vector3 foodVelo = Throwing(transform.position, tf.transform.position, degree);
        Throw(foodVelo);

        StartCoroutine(DestroyFood());

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Pet")
        {
            ObjectManager.instance.F_Recovery(gameObject);
            AI.success += 1;
        }
    }

    // x초 뒤에 사라짐(유저가 먹이를 벨루가에게 못 준 경우)
    IEnumerator DestroyFood()
    {
        if (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(2f);
            ObjectManager.instance.F_Recovery(gameObject);
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
