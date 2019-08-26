using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 자이로 카메라 문제점
// 부모를 만들어 회전할 때 z축 이동을 제어해봤음.
// 카메라를 자연스럽게 이동시키는 쪽으로 변환시킴.(실제로는 카메라가 움직이게 됨.)


public class CameraRotate : MonoBehaviour
{
    // 부모로 지정할 것
    GameObject camParent;

    //public Text debug;

    Quaternion qC, qT;          //부모와 자신의 쿼터니언 값

    void Start()
    {
        
        camParent = new GameObject("CamParent");
        camParent.transform.position = this.transform.position;
        transform.parent = camParent.transform;
        Input.gyro.enabled = true;
    }

    Vector3 tess;
    void Update()
    {

        camParent.transform.Translate(new Vector3( -Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.x, 0) * Time.deltaTime * 5);
        tess = transform.position;
        //갑작스러운 z값 이동이 됐을 경우 초기화 시킴
        if (Mathf.Abs(camParent.transform.position.z) >= tess.z + 50 || Mathf.Abs(camParent.transform.position.z) <= tess.z - 50)
        {
            reRo2(tess);
        }

        //debug.text = Input.gyro.rotationRateUnbiased.x + "\n" + Input.gyro.rotationRateUnbiased.y + "\n" + transform.eulerAngles + "\n" + transform.position;
    }
    void reRo2(Vector3 tee)
    {
        camParent.transform.position =new Vector3(0, tee.y, 0);
        transform.position = new Vector3(tee.x, 0, 0);
    }
    //void reRo()
    //{
    //    camParent.transform.rotation = Quaternion.Euler(0, qC.y, 0);
    //    transform.rotation = Quaternion.Euler(qT.x, 0, 0);
    //}
}


