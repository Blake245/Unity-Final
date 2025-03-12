using System.Collections;
using UnityEngine;

/// <summary>
/// Central game management class that handles core game systems, player spawning, scoring, and scene transitions.
/// Implemented as a Singleton to provide global access with only one instance.
/// </summary>
public class GameManager : Singleton<GameManager>
{
	// Scene configuration and game state variables
	[SerializeField] SceneData sceneData;          // Contains scene-specific data like spawn points and cameras
	[SerializeField] IntData scoreData;            // Tracks the current game score
	[SerializeField] IntData highScoreData;        // Tracks the highest score achieved
	[SerializeField] BoolData pauseData;           // Tracks whether the game is paused

	[Header("Events")]
	// Event references for game-wide communication
	[SerializeField] IntEvent onScoreEvent;        // Event raised when score changes
	[SerializeField] Event onPauseEvent;           // Event raised to toggle pause state
	[SerializeField] Event onPlayerSpawnEvent;     // Event raised to spawn the player
	[SerializeField] GameObjectEvent onPlayerDeathEvent;     // Event raised when player dies
	[SerializeField] Event onGameStartEvent;       // Event raised when game starts
	[SerializeField] Event onSceneReadyEvent;      // Event raised when scene is fully loaded and ready

	/// <summary>
	/// Initialize the GameManager by subscribing to events and loading saved data.
	/// </summary>
	private void Start()
	{
		// Subscribe to all relevant events
		onScoreEvent?.Subscribe(OnAddScore);
		onSceneReadyEvent?.Subscribe(OnSceneReady);
		onPauseEvent?.Subscribe(OnPause);

		onGameStartEvent?.Subscribe(OnGameStart);
		onPlayerDeathEvent?.Subscribe(OnPlayerDeath);
		onPlayerSpawnEvent?.Subscribe(OnPlayerSpawn);

		// Load the high score from player preferences
		highScoreData.Value = PlayerPrefs.GetInt("highscore", 0);
	}

	/// <summary>
	/// Called when a scene has finished loading and is ready to be initialized.
	/// </summary>
	public void OnSceneReady()
	{
		// Handle different initialization depending on scene type
		switch (sceneData.sceneType)
		{
			case SceneDescriptor.SceneType.Menu:
				// Handle menu initialization
				break;
			case SceneDescriptor.SceneType.Level:
				StartLevel();
				break;
		}
	}

	/// <summary>
	/// Begins the gameplay sequence for a level.
	/// </summary>
	private void StartLevel()
	{
		// Hide and lock the cursor during gameplay
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		// Trigger player spawn event to start the level
		onPlayerSpawnEvent?.RaiseEvent();
	}

	/// <summary>
	/// Called when a new game is started.
	/// Resets the player's score to zero.
	/// </summary>
	public void OnGameStart()
	{
		scoreData.Value = 0;
	}

	/// <summary>
	/// Spawns the player in the level and configures camera settings.
	/// </summary>
	public void OnPlayerSpawn()
	{
		// Instantiate the player at the spawn point
		GameObject go = Instantiate(sceneData.playerPrefab, sceneData.startPlayerSpawn.position, sceneData.startPlayerSpawn.rotation);
		GameObject head = go.transform.Find("Head").gameObject;
		GameObject arms = go.transform.Find("ArmsModel").gameObject;

		
		// Configure Cinemachine camera to follow and look at the player
		//sceneData.playerCamera.Follow = sceneData.playerPrefab.transform.GetChild(2);
		sceneData.playerCamera.Follow = head.transform;
		sceneData.playerCamera.LookAt = head.transform;

        // Set up player's view reference to the camera
        if (go.TryGetComponent(out PlayerController controller))
		{
			controller.View = sceneData.playerCamera.transform;
            arms.transform.SetParent(sceneData.playerCamera.transform);
			
			arms.transform.localPosition = new Vector3(0, -2.6f, 0.2f);
			arms.transform.localRotation = Quaternion.identity;
        }
	}

	/// <summary>
	/// Called when the player dies.
	/// Currently empty - implement respawn logic or game over state here.
	/// </summary>
	public void OnPlayerDeath(GameObject go)
	{
		StartCoroutine(PlayerDeathSeqeunce(go));
		// TODO: Implement player death handling, such as respawn or game over
	}

	/// <summary>
	/// Adds points to the player's score and updates high score if needed.
	/// </summary>
	/// <param name="points">The number of points to add to the score</param>
	public void OnAddScore(int points)
	{
		// Add points to current score
		scoreData.Value += points;

		// Check if we've achieved a new high score
		if (scoreData.Value >= highScoreData.Value)
		{
			highScoreData.Value = scoreData.Value;
			PlayerPrefs.SetInt("highscore", highScoreData.Value);
		}
	}

	/// <summary>
	/// Toggles the game's pause state, affecting time scale and cursor visibility.
	/// </summary>
	public void OnPause()
	{
		// Set time scale based on pause state (0 = paused, 1 = normal)
		Time.timeScale = pauseData.Value ? 0 : 1;

		// Show cursor when paused, hide when playing
		Cursor.visible = pauseData.Value;

		// Free cursor when paused, lock when playing
		Cursor.lockState = (pauseData.Value) ? CursorLockMode.None : CursorLockMode.Locked;
	}

	private IEnumerator PlayerDeathSeqeunce(GameObject go)
	{
		yield return new WaitForSeconds(2);
		Destroy(go);
		Destroy(sceneData.playerCamera.transform.Find("ArmsModel").gameObject);
		StartLevel();
	}
}