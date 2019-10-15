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
											//GameObject player = null;               // 플레이어
	GameObject veluga = null;               // 벨루가

	[SerializeField] AIStateController aiState = null;         // aistatecontroller

	Vector3 BallVelo;                       // 방향
	float degree = 60f;                     // 던지는 각도

	[SerializeField] UInput input = null;
	Transform caTF = null;
	Transform catchTF = null;

	[SerializeField] GameObject ballHit = null;

	// 작동 될 때
	private void OnEnable()
	{
		if (b_rigid == null)
		{
			transform.SetParent(null);                              // 부모를 없애어, 플레이어의 시각으로부터 자유로워진다.
			b_rigid = GetComponent<Rigidbody>();
			tf = GameObject.Find("LookChecker");
			tf.transform.SetParent(null);                           // 부모를 없애어, 플레이어가 어디를 보고 치던 벨루가에게 날아가도록 한다.
																	//player = GameObject.Find("Camera Container");
			veluga = GameObject.Find("BelugaAxis");
			aiState = GameObject.Find("BelugaAxis").GetComponent<AIStateController>();
			input = GameObject.Find("Main Camera").GetComponent<UInput>();
			caTF = GameObject.Find("Main Camera").GetComponent<Transform>();
			catchTF = GameObject.Find("CatchPoint").GetComponent<Transform>();
			ballHit = GameObject.Find("Ball");
		}

		// off 한 후의 힘 초기화  + new Vector3(0,4, veluga.transform.position.z * 2.5f) 
		b_rigid.velocity = Vector3.zero;
		BallVelo = Throwing(transform.position, veluga.transform.position + Vector3.up * 0.5f, degree);
		Throw(BallVelo);

		if (UInput.uIState == UInput.UIState.Ball)
		{
			catchTF.SetParent(null);
			aiState.StartCoroutine(aiState.BallBall());
		}

	}

	// 받아치는 함수, 타이밍 등의 조건 추가 예정임.(타이밍에 맞게 치지 못한 경우, fail 카운트가 늘어나 실패로 규정이 됨)
	public void Rebound()
	{
		AI.success++;
		b_rigid.velocity = Vector3.zero;
		BallVelo = Throwing(transform.position, veluga.transform.position, degree);
		Throw(BallVelo);

		aiState.StartCoroutine(aiState.BallBall());

	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "Pet")
		{
			input.TimingOn();
			ballHit.SetActive(true);
			SoundManager.s_Instance.Sound_EffectBall();
			b_rigid.velocity = Vector3.zero;
			BallVelo = Throwing(transform.position, catchTF.position, degree);                 // 벨루가가 플레이어에게 볼을 던짐  + new Vector3(0, 0, veluga.transform.position.z * 2.5f)veluga.
			Throw(BallVelo);
			StartCoroutine("DestroyBall");

		}
		else if (other.gameObject.name == "Main Camera")
		{
			Debug.Log(other.gameObject.name);
			other.collider.enabled = false;
			input.TimingOff();

			StopCoroutine("DestroyBall");
			Rebound();
		}
	}

	// 유저가 볼을 타이밍에 못맞추고 놓친 경우 사라짐.
	IEnumerator DestroyBall()
	{
		yield return new WaitForSeconds(1.35f);
		input.TimingOff();                    // 타이밍 메뉴 먼저 꺼줌
		yield return new WaitForSeconds(0.6f);
		AI.fail++;
		tf.transform.SetParent(caTF);
		catchTF.SetParent(caTF);
		catchTF.localPosition = new Vector3(0, 0, 0.5f);
		ObjectManager.instance.B_Recovery(gameObject);
	}

	// 던지는 용도 (현재 속도 절반으로, 거리는 두배로) * 0.5f
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
		// vector3.up 부분에 약하게 랜덤값을 곱해주면 더 좋아보일 지도?
		Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

		return finalVelocity;
	}


}
