using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI의 이미지는 일정 시간마다 작게함.
// 특정 시간 내에 유저의 입력이 있을 경우
// 박스 콜라이더를 켜 주어 리바운드를 가능하게 한다.
// 타이머는 Uinput 쪽에서 하는게 더 나은 듯...??
// UI는 애니메이션으로 줄어들게 하는 게 더 낫지 않을까?


public class RhythmCheck : MonoBehaviour
{

	//[SerializeField] BoxCollider playerCollider = null; // 플레이어의 박스 콜라이더

	[Header("이미지 및 크기 관련")]
	[SerializeField] Image big = null;
	[SerializeField] GameObject count = null;
	[SerializeField] UInput uuu = null;
	[SerializeField] Ball ball;

	//[SerializeField] AIStateController aistate = null;

	bool rightTiming = false;
	bool doubleNot = false;

	// 일정 크기 사이면 타이밍 체크 성공

	private void Start()
	{
		ball = GameObject.Find("ObjectManager").transform.Find("BeachBall(Clone)").GetComponent<Ball>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			CheckRhythm();
		}
	}

	public void CheckRhythm()
	{
		//if (input.once == false)
		//{
		//    input.once = true;

		Debug.Log(big.rectTransform.sizeDelta.x);
		if (big.rectTransform.sizeDelta.x <= 65 && big.rectTransform.sizeDelta.x >= 40)
		{
			rightTiming = true;
			uuu.TimingOff();
			Result();
		}
		//else if(big.rectTransform.sizeDelta.x <= 31)
		//{
		//    count.SetActive(false);
		//    AI.fail++;                              //공을 올바르게 치지 못한 경우.
		//}
		//}
	}

	public void Result()
	{
		if (rightTiming)
		{
			Debug.Log("on");
			count.SetActive(true);
			// 빠르게 두번 터치시 나타나는 문제를 방지하기 위함
			if (!doubleNot)
			{
				count.GetComponent<Text>().text = "" + (AI.success + 1);
				//playerCollider.enabled = true;
				ball.Rebound();
			}
			StartCoroutine(PutOut());               //1초 후에 다시 꺼준다.
		}
	}

	public IEnumerator PutOut()
	{
		doubleNot = true;
		yield return new WaitForSeconds(1f);
		//input.once = false;                         // 다시 사용할 수 있게 해준다.
		count.SetActive(false);
		//playerCollider.enabled = false;
		doubleNot = false;
	}
}
