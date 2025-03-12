using TMPro;
using UnityEngine;

public class ScenePause : MonoBehaviour
{
	[SerializeField] GameObject pauseUI;
	[SerializeField] StringEvent onSceneLoadEvent;
	[SerializeField] StringData mainMenuSceneNameData;

	[SerializeField] Event onPauseEvent;
	[SerializeField] BoolData pauseData;

	private void Start()
	{
		onPauseEvent?.Subscribe(OnPause);
	}

	public void OnPause()
	{
		pauseUI.SetActive(pauseData);
	}

	public void OnResumeButton()
	{
		pauseData.Value = false;
		onPauseEvent.RaiseEvent();
	}

	public void OnQuitButton()
	{
		pauseData.Value = false;
		onPauseEvent.RaiseEvent();

		onSceneLoadEvent.RaiseEvent(mainMenuSceneNameData);
	}
}
