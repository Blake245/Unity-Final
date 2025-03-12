using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ensures that a specified scene is loaded into the game.
/// Useful for managing persistent scenes like UI, audio, or manager scenes.
/// </summary>
public class SceneEnforcer : MonoBehaviour
{
	// Name of the scene that should be loaded/enforced
	[SerializeField] string sceneName;

	// Determines how the scene should be loaded:
	// - Single: Unload all current scenes, load the new scene
	// - Additive: Keep current scenes, add the new scene
	[SerializeField] LoadSceneMode mode;

	[SerializeField] bool useAsyncLoading = false;

	private void OnValidate()
	{
		// Verify scene exists in build settings
		if (!string.IsNullOrEmpty(sceneName))
		{
			bool sceneExists = false;
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
				string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
				if (sceneNameFromPath == sceneName)
				{
					sceneExists = true;
					break;
				}
			}
			if (!sceneExists)
			{
				Debug.LogWarning($"Scene '{sceneName}' is not in build settings!");
			}
		}
	}

	/// <summary>
	/// Called when the GameObject this component is attached to is initialized.
	/// Checks if the required scene is loaded, and loads it if not present.
	/// </summary>
	private void Awake()
	{
		if (!isSceneLoaded(sceneName))
		{
			if (useAsyncLoading)
				StartCoroutine(LoadSceneAsync());
			else
				SceneManager.LoadScene(sceneName, mode);
		}
	}

	private IEnumerator LoadSceneAsync()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	/// <summary>
	/// Checks if a scene with the specified name is currently loaded.
	/// </summary>
	/// <param name="sceneName">The name of the scene to check for</param>
	/// <returns>True if the scene is loaded, false otherwise</returns>
	public static bool isSceneLoaded(string sceneName)
	{
		// Iterate through all currently loaded scenes
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			// Get the scene at current index
			var scene = SceneManager.GetSceneAt(i);

			// Use case-insensitive comparison
			if (string.Equals(scene.name, sceneName, System.StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;    // Scene not found in loaded scenes
	}
}