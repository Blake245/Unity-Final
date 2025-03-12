using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Handles level-specific initialization such as player spawning and camera setup.
/// This component should be placed in each scene that requires these settings.
/// </summary>
public class SceneDescriptor : MonoBehaviour
{
	/// <summary>
	/// Defines the type of scene, which determines how it will be initialized.
	/// </summary>
	public enum SceneType
	{
		Menu,		// User interface scene like main menu, options, etc.
		Level,		// Gameplay scene that requires player, camera setup, etc.
		Cinematic	// 		
	}

	/// <summary>
	/// The type of scene this descriptor represents.
	/// </summary>
	[SerializeField] SceneType sceneType;

	/// <summary>
	/// The player prefab to instantiate when the scene starts.
	/// Only used for scenes of type Level.
	/// </summary>
	[SerializeField] GameObject playerPrefab;

	/// <summary>
	/// The initial spawn point for the player in this scene.
	/// Defines where the player will appear when the level begins.
	/// </summary>
	[SerializeField] Transform startPlayerSpawn;

	/// <summary>
	/// Cinemachine camera that will follow the player.
	/// Handles smooth camera movement and transitions.
	/// </summary>
	[SerializeField] CinemachineCamera playerCamera;

    /// <summary>
    /// The location of the camera for set locations
    /// </summary>
    [SerializeField] Transform cameraTransform;

    /// <summary>
    /// Event channel that notifies when a scene has finished loading.
    /// Used to trigger scene initialization.
    /// </summary>
    [SerializeField] Event onSceneLoadCompleteEvent;


	/// <summary>
	/// Event channel that notifies when the scene is ready to start.
	/// Used to trigger game manager scene start.
	/// </summary>
	[SerializeField] Event onSceneReadyEvent;

	/// <summary>
	/// ScriptableObject data container that will be populated with this scene's settings.
	/// Provides a way for other systems to access this scene's configuration.
	/// </summary>
	[SerializeField] SceneData sceneData;

	bool loaded = false;

	/// <summary>
	/// Subscribe to scene loaded event when this component is enabled.
	/// </summary>
	private void OnEnable()
	{
		onSceneLoadCompleteEvent?.Subscribe(OnSceneLoaded);
	}

	/// <summary>
	/// Unsubscribe from scene loaded event when this component is disabled.
	/// Prevents memory leaks and invalid callbacks.
	/// </summary>
	private void OnDisable()
	{
		onSceneLoadCompleteEvent?.Unsubscribe(OnSceneLoaded);
	}

	private void Update()
	{
#if UNITY_EDITOR
		// Check if this is the first scene that started with the Editor play button
		if (!loaded && Time.timeSinceLevelLoad < 0.1f && Time.frameCount < 2)
		{
			OnSceneLoaded();
		}
#endif
	}

	/// <summary>
	/// Called when a scene is loaded via the scene loading system.
	/// Updates the shared data container with this scene's specific settings.
	/// </summary>
	/// <param name="sceneName">Name of the loaded scene</param>
	void OnSceneLoaded()
	{
		loaded = true;

		// Transfer scene-specific settings to the shared data container
		sceneData.sceneType = sceneType;
		sceneData.playerPrefab = playerPrefab;
		sceneData.startPlayerSpawn = startPlayerSpawn;
		sceneData.playerCamera = playerCamera;
		sceneData.cameraTransform = cameraTransform;

		onSceneReadyEvent?.RaiseEvent();
	}
}