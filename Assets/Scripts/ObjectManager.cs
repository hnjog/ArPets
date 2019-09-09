﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 먹이와 공의 오브젝트 풀
// 일단 카메라에 넣어줄 예정임.

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;           // 스크립트 접근용, 싱글톤

    [SerializeField]
    GameObject ob_Food = null;                      // 먹이용 프리팹
    GameObject ob_Ball = null;                      // 공 프리팹

    public Queue<GameObject> q_Food = new Queue<GameObject>();   //먹이용 큐
    //public Queue<GameObject> q_Ball = new Queue<GameObject>();   //공 큐

    private void Start()
    {
        instance = this;

        //먹이 생성 10회
        for (int i = 0; i < 10; i++)
        {
            //위치와 각도는 변경 가능
            GameObject f_object = Instantiate(ob_Food, Vector3.zero, Quaternion.identity);
            q_Food.Enqueue(f_object);
            f_object.SetActive(false);
        }

        Instantiate(ob_Ball, Vector3.zero, Quaternion.identity);
        ob_Ball.SetActive(false);
    }

    // 사용한 먹이를 풀에 반납시키는 함수
    public void F_Recovery(GameObject r_object)
    {
        q_Food.Enqueue(r_object);
        r_object.SetActive(false);
    }

    // 먹이 내보내기 함수
    public GameObject F_Expert()
    {
        GameObject e_object = q_Food.Dequeue();
        e_object.SetActive(true);
        return e_object;
    }

    // 볼 끄기
    public void B_Off(GameObject r_object)
    {
        r_object.SetActive(false);
    }

    // 볼 켜기
    public void B_On(GameObject r_object)
    {
        r_object.SetActive(true);
    }
}


