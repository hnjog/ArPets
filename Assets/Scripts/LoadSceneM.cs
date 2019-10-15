using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneM : MonoBehaviour
{
	//[SerializeField] Animator owyoLogo = null;

	AsyncOperation doing;

	private void Start()
	{
		StartCoroutine(SceneLoad());
	}

	IEnumerator SceneLoad()
	{
		doing = SceneManager.LoadSceneAsync(1);
		doing.allowSceneActivation = false;
		yield return new WaitForSecondsRealtime(1.1f);

		while (!doing.isDone)
		{

			if (doing.progress >= 0.9f)
			{
				doing.allowSceneActivation = true;
				yield return null;
			}
		}
	}
}
