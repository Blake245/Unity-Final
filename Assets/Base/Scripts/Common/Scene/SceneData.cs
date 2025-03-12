using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Scriptable Object that stores all essential data related to a game level.
/// This is used to configure level-specific settings like player spawning and camera setup.
/// </summary>
[CreateAssetMenu(fileName = "SceneData", menuName = "Data/SceneData")]
public class SceneData : ScriptableObjectBase
{
	[Tooltip("The type of scene this data represents (e.g., MainMenu, Level, Cutscene)")]
	public SceneDescriptor.SceneType sceneType;

	/// <summary>
	/// The player prefab to instantiate when the scene starts.
	/// This is the character that the user will control in the level.
	/// </summary>
	[Tooltip("The player character prefab that will be instantiated in this scene")]
	public GameObject playerPrefab;

	/// <summary>
	/// The initial spawn point for the player in this scene.
	/// Defines where the player will appear when the level begins or after respawning.
	/// </summary>
	[Tooltip("The transform position where the player will spawn when the level starts")]
	public Transform startPlayerSpawn;

	/// <summary>
	/// Cinemachine camera that will follow the player.
	/// Handles smooth camera movement and transitions while tracking the player.
	/// </summary>
	[Tooltip("The Cinemachine camera that will be assigned to follow the player")]
	public CinemachineCamera playerCamera;

    /// <summary>
    /// The location of the camera for set locations
    /// </summary>
    [Tooltip("The location of the camera for set locations")]
	public Transform cameraTransform;

	/// <summary>
	/// Background music that will play during this level.
	/// Sets the audio atmosphere for the level environment.
	/// </summary>
	[Tooltip("The music track that will play while this scene is active")]
	public AudioClip backgroundMusic;
}