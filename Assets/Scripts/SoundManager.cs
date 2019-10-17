using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 매니저를 통해 각종 사운드를 사용함.
// 해당 사운드가 필요할 때 여기서 메소드를 호출하는 형식으로 한다.

public class SoundManager : MonoBehaviour
{
	public static SoundManager s_Instance;

	[Header("오디오소스 및 클립들")]
	[SerializeField] AudioSource[] soundSources;            // 0: 벨루가, 1: 카메라(UI) 2 : bgm(자기자신)
	[SerializeField] AudioClip[] sound_Beluga;
	// 벨루가 소리들 0 : 기쁨 1 : 보통 2 : 슬픔
	[SerializeField] AudioClip[] sound_Effect;
	// UI 및 상태 효과음
	// 0 : 공 1 : 먹이 2 : 먹이 다 먹고 난 뒤
	// 3 : 공놀이 성공 4 : 공놀이 실패
	// 5 : 메뉴 선택 6 : 메뉴 이동
	// 7 : 배고픔
	[SerializeField] AudioClip[] sound_Bgm;

	private void Start()
	{
		s_Instance = this;
		Bgm();
	}

	// BGM 재생용
	public void Bgm()
	{
		soundSources[2].clip = sound_Bgm[0];
		soundSources[2].Play();
	}

	IEnumerator Bubble()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			if (AIStateController.veluga_State == AI.Chara_State.state_Idle && Random.value <= 0.5f)
			{
				soundSources[3].clip = sound_Bgm[1];
				soundSources[3].Play();
			}
		}
	}

	// 벨루가 기쁨, 보통, 슬픔
	public void Sound_BelugaHappy()
	{
		soundSources[0].volume = 0.4f;
		soundSources[0].PlayOneShot(sound_Beluga[0]);
	}
	public void Sound_BelugaNormal()
	{
		soundSources[0].volume = 0.5f;
		soundSources[0].PlayOneShot(sound_Beluga[1]);
	}
	public void Sound_BelugaSad()
	{
		soundSources[0].volume = 0.3f;
		soundSources[0].PlayOneShot(sound_Beluga[2]);
	}

	// UI 공, 먹이, 먹이 후, 공 성공, 공 실패, 메뉴 선택, 메뉴 이동, 배고픔
	public void Sound_EffectBall()
	{
		soundSources[1].volume = 0.4f;
		soundSources[1].PlayOneShot(sound_Effect[0]);
	}
	public void Sound_EffectFeed()
	{
		soundSources[1].volume = 0.2f;
		soundSources[1].PlayOneShot(sound_Effect[1]);
		soundSources[1].volume = 1f;
	}
	public void Sound_EffectFeedAfter()             //어차피 먹고 나면 행복 나올텐데 굳이?
	{
		soundSources[1].volume = 0.3f;
		soundSources[1].PlayOneShot(sound_Effect[2]);
		soundSources[1].volume = 1f;
	}
	public void Sound_EffectBallSuccess()
	{
		soundSources[1].volume = 0.3f;
		soundSources[1].PlayOneShot(sound_Effect[3]);
		soundSources[1].volume = 1f;
	}
	public void Sound_EffectBallFail()
	{
		soundSources[1].PlayOneShot(sound_Effect[4]);

	}
	public void Sound_EffectMenuMove()
	{
		soundSources[1].PlayOneShot(sound_Effect[5]);
	}
	public void Sound_EffectMenuChoose()
	{
		soundSources[1].PlayOneShot(sound_Effect[6]);
	}
	public void Sound_EffectHungry()
	{
		soundSources[1].PlayOneShot(sound_Effect[7]);
	}
}
