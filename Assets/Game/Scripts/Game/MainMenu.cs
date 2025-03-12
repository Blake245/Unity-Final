using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] GameObject titleUI;
	[SerializeField] GameObject optionsUI;

	[SerializeField] StringEvent onSceneLoadEvent;
	[SerializeField] StringData startSceneNameData;


	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void OnStartButton()
	{
		onSceneLoadEvent.RaiseEvent(startSceneNameData);
	}

	public void OnQuitButton()
	{
		Application.Quit();
	}

	public void OnOptionsButton()
	{
		titleUI.SetActive(false);
		optionsUI.SetActive(true);
	}

	public void OnReturnToTitleButton()
	{
		titleUI.SetActive(true);
		optionsUI.SetActive(false);
	}
}
