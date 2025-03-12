using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene loading with loading screen and fade transitions.
/// Implemented as a singleton for global access.
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
	[Header("UI References")]
	[SerializeField] private GameObject loadingUI;			// UI panel shown during loading
	[SerializeField] private Slider loadingMeterUI;			// Progress bar for load progress
	[SerializeField] private ScreenFade screenFade;			// Screen fade transition effect

	[Header("Loading Settings")]
	[SerializeField] private float minimumLoadTime = 1;		// Minimum time to show loading screen
	[SerializeField] private float loadingBarSpeed = 2;		// Speed of loading bar interpolation
	[SerializeField] private bool smoothLoadingBar = true;  // Whether to smooth loading bar progress

	// Events for scene loading state changes
	[SerializeField] StringEvent onSceneLoadEvent;			// Triggered to request scene load
	[SerializeField] Event onSceneLoadStartEvent;			// Triggered when loading begins
	[SerializeField] Event onSceneLoadCompleteEvent;		// Triggered when loading finishes

	private bool isLoading = false;    // Tracks if a scene is currently loading
	public bool IsLoading => isLoading;

	/// <summary>
	/// Subscribe to load event when enabled
	/// </summary>
	private void Start()
	{
		onSceneLoadEvent?.Subscribe(Load);
	}

	/// <summary>
	/// Begin loading the specified scene if not already loading
	/// </summary>
	/// <param name="sceneName">Name of scene to load</param>
	public void Load(string sceneName)
	{
		// Prevent multiple simultaneous loads
		if (isLoading)
		{
			Debug.LogWarning("Scene load already in progress!");
			return;
		}

		// Validate scene exists
		if (!Application.CanStreamedLevelBeLoaded(sceneName))
		{
			Debug.LogError($"Scene {sceneName} does not exist in build settings!");
			return;
		}

		StartCoroutine(LoadSceneRoutine(sceneName));
	}

	/// <summary>
	/// Coroutine that handles the scene loading process
	/// </summary>
	/// <param name="sceneName">Name of scene to load</param>
	private IEnumerator LoadSceneRoutine(string sceneName)
	{
		isLoading = true;
		float loadStartTime = Time.time;

		// Reset time scale in case it was modified
		Time.timeScale = 1;

		onSceneLoadStartEvent?.RaiseEvent();

		// Fade out current scene
		screenFade.FadeOut();
		yield return new WaitUntil(() => screenFade.isDone);

		// Start async scene loading
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		asyncOperation.allowSceneActivation = false;  // Prevent auto-activation

		// Show and initialize loading UI
		loadingUI.SetActive(true);
		loadingMeterUI.value = 0;

		float currentProgress = 0f;

		// Update loading progress until complete
		while (!asyncOperation.isDone)
		{
			// Convert progress to 0-1 range (async op goes to 0.9)
			float targetProgress = (asyncOperation.progress / 0.9f);

			// Update progress bar
			if (smoothLoadingBar)
			{
				// Smoothly interpolate for visual polish
				currentProgress = Mathf.MoveTowards(currentProgress, targetProgress,
					Time.deltaTime * loadingBarSpeed);
			}
			else
			{
				currentProgress = targetProgress;
			}

			loadingMeterUI.value = currentProgress;

			// Continue until loading complete and minimum time elapsed
			if (asyncOperation.progress >= 0.9f &&
				Time.time - loadStartTime >= minimumLoadTime)
			{
				break;
			}

			yield return null;
		}

		// Ensure loading bar looks complete
		loadingMeterUI.value = 1f;

		// Clean up loading UI
		loadingUI.SetActive(false);

		// Allow scene to activate and wait for completion
		asyncOperation.allowSceneActivation = true;
		yield return new WaitUntil(() => asyncOperation.isDone);

		onSceneLoadCompleteEvent?.RaiseEvent();

		// Complete loading process
		isLoading = false;

		// Fade in new scene
		screenFade.FadeIn();
		yield return new WaitUntil(() => screenFade.isDone);
	}
}