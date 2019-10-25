using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벨루가의 이동에 관한 함수
// 곡선 이동, 이동할 범위의 제한
// slerp와 시점 변환(transform.LookAt(transform.position + forward);)
// 최대 이동거리 지정 및 너무 갑작스러운 각도의 조정.
// 기본 상태일 때만 이동이 되어야 한다
// 호출 - UI 메뉴에서 선택, 중앙으로 오게함
// 멀리가게 하는 것 추가 MoveState 중 하나 새로 생성하여 만든다. 
// 해당 상태일 경우, farPoint로 이동하게 하는 형식으로?(그렇다면 어떤 조건에서 가게 만들까)

public class AIMoveController : AI
{
	[Header("bool 변수")]
	bool isMoving = false;                          // 업데이트로 움직이게 하는 용도의 bool 변수
	bool isReturning = false;                       // 리턴용 변수
	bool isFarAway = false;                         // 멀리가는 변수

	[Header("속도 관련")]
	float moSpeed = 1.5f;                           // 이동 스피드
	float roSpeed = 0.01f;                          // 회전 스피드

	[Header("위치 조정용")]
	[SerializeField] Transform cameraPos = null;    // 카메라 위치
	[SerializeField] Transform farPoint = null;     // 멀리 가는 곳
	float distance = 0;                             // 거리

	[Header("쿼터니언 값 조정")]
	int qx = 0;                                     // x 쿼터니언               
	int qy = 0;                                     // y 쿼터니언

	Quaternion lc1, lc2;                            // 각도 조정용

	[SerializeField] GameObject pointer = null;
	[SerializeField] GameObject lookCheckerTF2 = null;

	void Start()
	{
		StartCoroutine(MoveAI());
		StartCoroutine(RandRo());
	}

	void Update()
	{
		if (veluga_State == Chara_State.state_Idle) // 벨루가가 기본 상태일때만 작동하도록.
		{

			//이동 상태 및 호출되지 않았을 경우
			if (isMoving && !isStillMove)
			{
				VelugaLook();                       // 벨루가 방향
				FreeMove();                         // 벨루가 움직임
			}
			// 기본 상태에서 움직임 명령 받은 경우
			else if (isStillMove)
			{
				Returning();                        // 카메라로 귀환함.
			}
			// 대기 중 수평을 맞추는 용도
			else
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, qy, 0), 0.01f);
		}
		// 다른 상태가 되었을 때, 카메라의 안에 들어오게 하고, 카메라를 바라보게 한다.
		else if (veluga_State == Chara_State.state_Eat || veluga_State == Chara_State.state_Ball)
		{
			pointer.SetActive(false);
			if (isStillMove)
			{
				Returning();

				// 카메라를 바라보게 되면
				if (Quaternion.Angle(transform.rotation, lc2) < 10)
				{
					isStillMove = false;
				}

			}

		}
	}

	// 중앙으로 돌아옴
	// 일반 상태에서 벗어남 - 벨루가를 중앙에 강제위치 시키는 용도
	void Returning()
	{
		if (Vector3.Distance(transform.position, lookCheckerTF2.transform.position) > 0.1f)
		{
			lc1 = Quaternion.LookRotation(lookCheckerTF2.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, lc1, 0.3f);
			transform.Translate(Vector3.forward * Time.deltaTime * moSpeed * 2);
		}
		else
		{
			lc2 = Quaternion.LookRotation(cameraPos.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, lc2, 0.05f);
		}
	}

	// 벨루가가 2초마다 행동을 결정하게 함.
	IEnumerator MoveAI()
	{
		//무한반복
		while (true)
		{

			Timer2Second();
			yield return new WaitForSeconds(2);         // 2초 대기
		}
	}
	// 2초마다 행동을 결정해줄 함수
	public void Timer2Second()
	{

		// 대기 상태, 60%의 확률
		if (veluga_MoveState == Chara_Move.idle && Random.value <= 0.6)
		{
			isMoving = true;
			if (Random.value <= 0.2)
			{
				isFarAway = true;
			}
		}
		// 이동 상태, 25%의 확률, 귀환 중에는 사용하지 않음.
		else if (veluga_MoveState == Chara_Move.move && Random.value <= 0.25 && !isReturning)
		{
			isMoving = false;
			Wating();
		}

	}

	// 자유 이동 상태
	public void FreeMove()
	{
		veluga_Ani.SetInteger("MoveState", 1);

		transform.Translate(Vector3.forward * Time.deltaTime * moSpeed);                               // 정면 * 거리 * 일정시간 나눠주기

		veluga_MoveState = Chara_Move.move;
	}

	// 대기상태
	public void Wating()
	{
		veluga_Ani.SetInteger("MoveState", 0);

		//지속 실행 방지... 인데 이게 효율적인지는 의문
		if (transform.position != transform.position)
		{
			transform.Translate(transform.position);
		}

		veluga_MoveState = Chara_Move.idle;
	}

	// 움직일때 1초마다 랜덤한 시점처리
	IEnumerator RandRo()
	{
		while (true)
		{
			if (isMoving) RandomRotate();
			yield return new WaitForSeconds(1);         // 1초 대기
		}
	}

	// 랜덤 방향 지정
	public void RandomRotate()
	{
		qx = Random.Range(-31, 31);
		qy = Random.Range(-121, 121);
	}

	// 시점 변환 메소드
	public void VelugaLook()
	{
		// if문을 이용하여 ro 값을 조정하면 회전을 부드럽게 할수 있지 않을까
		roSpeed = Random.Range(0.01f, 0.05f);

		// 임의의 쿼터니언 값을 가지게 함.
		Quaternion qa = Quaternion.Euler(qx, qy, 0);

		distance = Vector3.Distance(transform.position, lookCheckerTF2.transform.position);



		//멀리가기 true시, 먼곳의 방향으로 나아감.
		if (isFarAway)
		{
			Quaternion far = Quaternion.LookRotation(farPoint.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, far, roSpeed);
			if (Vector3.Distance(transform.position, farPoint.position) <= 0.5f)
			{
				isFarAway = false;
			}
		}
		else
		{
			// 거리 체크
			if (distance >= 3) isReturning = true;
			else if (distance <= 1) isReturning = false;


			// 거리가 일정거리 이상이면 시점을 카메라 중앙 지점으로 돌리게 한다.
			if (!isReturning)
				transform.rotation = Quaternion.Slerp(transform.rotation, qa, roSpeed);
			else
			{
				Quaternion qb = Quaternion.LookRotation(lookCheckerTF2.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, qb, roSpeed);
			}
		}
	}


}
