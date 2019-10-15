using UnityEngine;
using UnityEngine.UI;



// 카메라를 자연스럽게 이동시키는 쪽으로 변환시킴.(실제로는 카메라가 움직이게 됨.)
// 한바퀴 도는 등의 경우가 발생할 경우 정상적으로 보이게 될까? 일정 범위의 이동이 이루어진 경우 반대쪽으로 이동시켜주는 것이 필요함.
// 한바퀴 돌경우 약 30 정도 이동(절반은 15 이므로 15 정도 이동한 경우 카메라 rotation을 180 돌려주는 것은?)
// 그렇게 할 경우 카메라가 다시 중앙으로 이동되게 되지 않을까(중앙으로 왔다리 갔다리)
// y축의 경우 한바퀴 돌때 이상하게 조정되는 경우가 있으므로 카메라 rotation을 돌려줄때 0으로 초기화 시켜주는 방식은?

public class CameraScript : MonoBehaviour
{

	[Header("자이로 스코프 테스트용")]
	private bool gyroEnabled;
	private Gyroscope gyro;

	private GameObject cameraContainer;
	private Quaternion rot;

	[SerializeField] Transform velugaPos = null;

	[SerializeField] GameObject pointer = null;

	Quaternion fP;

	private void Start()
	{
		cameraContainer = new GameObject("Camera Container");
		cameraContainer.transform.position = transform.position;
		transform.SetParent(cameraContainer.transform);

		gyroEnabled = EnableGyro();


	}

	// 자이로 사용이 가능한 여부
	private bool EnableGyro()
	{
		if (SystemInfo.supportsGyroscope)
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
		if (gyroEnabled)
		{
			transform.localRotation = gyro.attitude * rot;
			fP = Quaternion.LookRotation(velugaPos.position - transform.position);// 
		}

		if (Quaternion.Angle(fP, transform.rotation) > 30) pointer.SetActive(true);
		else pointer.SetActive(false);
	}
}


