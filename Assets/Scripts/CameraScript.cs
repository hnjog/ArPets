using UnityEngine;
using UnityEngine.UI;



// 카메라를 자연스럽게 이동시키는 쪽으로 변환시킴.(실제로는 카메라가 움직이게 됨.)
// 한바퀴 도는 등의 경우가 발생할 경우 정상적으로 보이게 될까? 일정 범위의 이동이 이루어진 경우 반대쪽으로 이동시켜주는 것이 필요함.
// 한바퀴 돌경우 약 30 정도 이동(절반은 15 이므로 15 정도 이동한 경우 카메라 rotation을 180 돌려주는 것은?)
// 그렇게 할 경우 카메라가 다시 중앙으로 이동되게 되지 않을까(중앙으로 왔다리 갔다리)
// y축의 경우 한바퀴 돌때 이상하게 조정되는 경우가 있으므로 카메라 rotation을 돌려줄때 0으로 초기화 시켜주는 방식은?

public class CameraScript : MonoBehaviour
{

    //// 디버깅용
    //public Text debug;
    //
    //// 카메라 위치
    //Vector3 cameraPos;
    //
    //[SerializeField]
    //Transform startPos = null;
    //
    //// cameraturning 함수가 1번만 사용되게 
    ////bool checkerX = true;                   
    ////bool checkerY = true;
    //
    //void Start()
    //{
    //    Input.gyro.enabled = true;
    //}
    //
    //
    //void Update()
    //{
    //    transform.Translate(new Vector3(-Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.x, 0) * Time.deltaTime * 3);
    //    cameraPos = transform.position;
    //
    //    // 카메라가 일정 x축 이상으로 움직일시 뒤를 돌게함(카메라 rotation을 뒤돌게 하여 다시 0으로 가까워지게 하는 방식)
    //    if (Vector3.Distance(startPos.position, cameraPos) > 15)
    //    {
    //        CameraTurning();
    //    }
    //
    //    //if (Vector3.Distance(startPos.position, cameraPos) > 15)
    //    //{
    //    //    CameraTurningX();
    //    //}
    //
    //
    //
    //    //debug.text = Input.gyro.rotationRateUnbiased.x + "\n" + Input.gyro.rotationRateUnbiased.y + "\n" + transform.eulerAngles + "\n" + transform.position;
    //}
    //
    //// 카메라 위치 초기화 - 갑작스러운 무언가에 대비하려 했으나 잠시 생각좀
    //void RePosition(Vector3 pose)
    //{
    //    transform.position = new Vector3(pose.x, pose.y, 0);
    //}
    //
    //// 카메라 x축 회전 - 카메라가 한바퀴 도는 상황들
    ////void CameraTurningX()
    ////{
    ////    transform.position = new Vector3(0, cameraPos.y, -4);                           // 현재 y측 좌표는 유지, x측 초기화 , z축 좌표는 나중에 원하는 대로
    ////    transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,0,0));   // x축 돈 것을 제외하고 0으로 초기화( zero 사용시 아래 코드와 같이 사용해 계속 뒤만 볼수 있기에)
    ////    transform.Rotate(new Vector3(180, 0, 0));                                       // x축을 180도 회전시켜 반대쪽을 보게 한다.
    ////}
    //
    //// 카메라 Y축 회전 - 뒤를 돌거나 하는 상황들
    //void CameraTurning()
    //{
    //    transform.position = new Vector3(cameraPos.x, cameraPos.y, startPos.position.z);                           // 현재 x측 좌표는 유지, y측 초기화
    //    //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 0)); // y축 돈 것을 제외하고 0으로 초기화( zero 사용시 아래 코드와 같이 사용해 계속 뒤만 볼수 있기에)
    //    transform.Rotate(new Vector3(0, 180, 0));                                       // y축을 180도 회전시켜 반대쪽을 보게 한다.
    //}

    [Header("자이로 스코프 테스트용")]
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;

    private void Awake()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();
    }

    // 자이로 사용이 가능한 여부
    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

    private void Update()
    {
        if(gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
    }
}


