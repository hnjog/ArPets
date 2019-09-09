using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    Rigidbody f_rigid = null;

    private void OnEnable()
    {
        if(f_rigid == null)
        {
            f_rigid = GetComponent<Rigidbody>();
        }

        // off 한 후의 힘 초기화
        f_rigid.velocity = Vector3.zero;
        // 일정 각도로 발사되게....

        
    }

    // x초 뒤에 사라짐(유저가 먹이를 벨루가에게 못 준 경우)
    IEnumerator DestroyFood()
    {
        yield return new WaitForSeconds(2f);
        ObjectManager.instance.F_Recovery(gameObject);
    }
}
