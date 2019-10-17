using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneM : MonoBehaviour
{
	//[SerializeField] Animator owyoLogo = null;

	float timer = 0f;

	AsyncOperation doing;

	private void Start()
	{
		StartCoroutine(SceneLoad());
	}

	IEnumerator SceneLoad()
	{
		doing = SceneManager.LoadSceneAsync(1);
		doing.allowSceneActivation = false;
		//yield return new WaitForSecondsRealtime(6f);

		while (!doing.isDone)
		{
			yield return null;
			timer += Time.deltaTime;
			Debug.Log(timer);
			if (timer >= 3f)
			{
				doing.allowSceneActivation = true;
				
			}
			
		}
	}
}
