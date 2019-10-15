using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 음식 함수
// 종종 중력에 따라 먹이가 제대로 나가지 않는 경향이 있음

public class Food : MonoBehaviour
{
	Rigidbody f_rigid = null;               // 리지드바디

	GameObject tf = null;                   // 던지는 곳

	float degree = 45f;                     // 던지는 각도

	[SerializeField] AIStateController aiState = null;         // aistatecontroller
	[SerializeField] UInput uu = null;
	[SerializeField] Transform veTF = null;
	GameObject obm = null;

	[SerializeField] GameObject foodParticle = null;
	
	// 작동 될 때
	private void OnEnable()
	{
		if (f_rigid == null)
		{
			transform.SetParent(null);                              // 부모를 없애어, 플레이어의 시각으로부터 자유로워진다.
			f_rigid = GetComponent<Rigidbody>();
			tf = GameObject.Find("LookChecker");
			obm = GameObject.Find("ObjectManager");
			aiState = GameObject.Find("BelugaAxis").GetComponent<AIStateController>();
			veTF = GameObject.Find("BelugaAxis").GetComponent<Transform>();
			uu = GameObject.Find("Main Camera").GetComponent<UInput>();
			foodParticle = GameObject.Find("Eating");
		}

		uu.fakeFish.SetActive(false);
		if (AI.success + AI.fail < 9 && UInput.uIState == UInput.UIState.Feed && Vector3.Distance(tf.transform.position, veTF.position) < 0.4f) // 벨루가위치와 던짐 목표 위치를 비교하여 일정 거리 이하일 경우만
		{
			aiState.StartCoroutine(aiState.FeedFeed());
		}
		f_rigid.velocity = Vector3.zero;
		Vector3 foodVelo = Throwing(obm.transform.position, tf.transform.position + Vector3.up, degree);
		Throw(foodVelo);

		StartCoroutine("DestroyFood");

	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "Pet")
		{
			StopCoroutine("DestroyFood");
			foodParticle.SetActive(true);
			SoundManager.s_Instance.Sound_EffectFeed();
			ObjectManager.instance.F_Recovery(gameObject);
			AI.success++;

			if (AI.success + AI.fail < 10)
			{
				uu.fakeFish.SetActive(true);

			}
		}
	}

	// x초 뒤에 사라짐(유저가 먹이를 벨루가에게 못 준 경우)
	IEnumerator DestroyFood()
	{
		yield return new WaitForSeconds(2.5f);
		AI.fail++;
		if (AI.success + AI.fail < 10) uu.fakeFish.SetActive(true);
		ObjectManager.instance.F_Recovery(gameObject);
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
