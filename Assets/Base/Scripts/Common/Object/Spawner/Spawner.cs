using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Abstract base class for object spawners in Unity.
/// Handles common spawning functionality with configurable parameters.
/// Derived classes must implement the Spawn() method to define specific spawn behavior.
/// </summary>
public abstract class Spawner : MonoBehaviour
{
	// Serialized fields for inspector configuration
	[SerializeField] GameObject[] spawnPrefabs;        // Array of prefabs that can be spawned
	[SerializeField] Transform parent = null;           // Optional parent transform for spawned objects

	// Spawn timing and quantity parameters
	[SerializeField] float minSpawnTime = 1;            // Minimum time between spawns
	[SerializeField] float maxSpawnTime = 1;            // Maximum time between spawns
	[SerializeField] int maxSpawned = 1;                // Maximum number of spawned objects allowed simultaneously
	[SerializeField] protected bool onlySpawnInEmptySpace = true; // Whether to check for collisions before spawning
	[SerializeField] float checkRadius = 0.5f;          // Radius to check for collisions when onlySpawnInEmptySpace is true

	// Runtime activation settings
	[SerializeField] bool enableOnAwake = true;         // Whether spawner should be active on startup
	[SerializeField] Event onStartEvent;                // Event that will trigger the spawner to start
	[SerializeField] Event onStopEvent;                 // Event that will trigger the spawner to stop

	// Runtime state tracking
	private List<GameObject> activeSpawns = new List<GameObject>(); // Tracks currently spawned objects
	private bool active = false;                                    // Whether spawner is currently active
	private Coroutine spawnTimerCoroutine;                          // Reference to spawn timer coroutine

	protected const int MAX_SPAWN_ATTEMPTS = 5; // Maximum number of attempts to find a clear spawn position
	/// <summary>
	/// Abstract method that must be implemented by derived classes to define specific spawn behavior.
	/// </summary>
	public abstract void Spawn();

	/// <summary>
	/// Called when the script instance is enabled.
	/// Subscribes to start and stop events.
	/// </summary>
	private void OnEnable()
	{
		onStartEvent?.Subscribe(SetActive);
		onStopEvent?.Subscribe(SetInactive);
	}

	/// <summary>
	/// Called when the script instance is disabled.
	/// Unsubscribes from start and stop events to prevent memory leaks.
	/// </summary>
	private void OnDisable()
	{
		onStartEvent?.Unsubscribe(SetActive);
		onStopEvent?.Unsubscribe(SetInactive);
	}

	/// <summary>
	/// Called on the first frame after the script instance is enabled.
	/// Initializes the spawner state based on enableOnAwake setting.
	/// </summary>
	private void Start()
	{
		active = enableOnAwake;
		if (active)
		{
			SetActive();
		}
	}

	/// <summary>
	/// Coroutine that handles the timing of spawn attempts.
	/// Runs continuously while the spawner is active.
	/// </summary>
	/// <returns>Yield instruction for the coroutine system</returns>
	IEnumerator SpawnTimer()
	{
		while (true)
		{
			// Wait for a random time between min and max spawn time
			yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
			if (SpawnReady())
			{
				Spawn();
			}
		}
	}

	/// <summary>
	/// Checks if spawning conditions are met.
	/// </summary>
	/// <returns>True if spawner is active and below maximum spawn count</returns>
	bool SpawnReady()
	{
		return (active && activeSpawns.Count < maxSpawned);
	}

	/// <summary>
	/// Instantiates a prefab at the specified position and rotation.
	/// Registers the spawned object for tracking if it has a DestroyNotifier component.
	/// </summary>
	/// <param name="prefab">The prefab to instantiate</param>
	/// <param name="position">World position to spawn at</param>
	/// <param name="rotation">Rotation to apply to the spawned object</param>
	protected void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject go = Instantiate(prefab, position, rotation, parent);
		// Register for notification when object is destroyed to maintain accurate spawn count
		if (go.TryGetComponent(out DestroyNotifier notifier))
		{
			notifier.OnDestroyed += RemoveSpawn;
		}
		activeSpawns.Add(go);
	}

	/// <summary>
	/// Gets a random prefab from the spawnPrefabs array.
	/// </summary>
	/// <returns>A randomly selected prefab or null if none are assigned</returns>
	protected GameObject GetSpawnObject()
	{
		if (spawnPrefabs == null || spawnPrefabs.Length == 0)
		{
			Debug.LogError("Spawner: No prefabs assigned!");
			return null;
		}
		return spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
	}

	/// <summary>
	/// Callback for when a spawned object is destroyed.
	/// Removes the object from the tracked list.
	/// </summary>
	/// <param name="go">The GameObject that was destroyed</param>
	void RemoveSpawn(GameObject go)
	{
		activeSpawns.Remove(go);
	}

	/// <summary>
	/// Checks if a potential spawn location is free of collisions.
	/// Ignores the spawner's own volume collider if it exists.
	/// </summary>
	/// <param name="position">The position to check for collisions</param>
	/// <returns>True if no relevant colliders are found within the check radius</returns>
	protected bool IsSpawnLocationClear(Vector3 position)
	{
		// Get all colliders within the check radius
		Collider[] colliders = Physics.OverlapSphere(position, checkRadius);

		// If no colliders found, position is clear
		if (colliders.Length == 0)
			return true;

		// If we have a volume collider, we need to check each collider
		Collider volumeCollider = GetComponent<Collider>();
		if (volumeCollider != null)
		{
			// Check each collider found
			foreach (Collider col in colliders)
			{
				// Skip our volume collider, but return false for any other collider
				if (col != volumeCollider)
					return false;
			}
			// If we only found our own collider, position is clear
			return true;
		}

		// No volume collider to filter, so any collider means position is not clear
		return false;
	}

	/// <summary>
	/// Activates the spawner and starts the spawn timer coroutine.
	/// </summary>
	void SetActive()
	{
		active = true;

		// Stop existing coroutine if it's running to avoid duplicates
		if (spawnTimerCoroutine != null)
		{
			StopCoroutine(spawnTimerCoroutine);
			spawnTimerCoroutine = null;
		}
		spawnTimerCoroutine = StartCoroutine(SpawnTimer());
	}

	/// <summary>
	/// Deactivates the spawner and stops the spawn timer coroutine.
	/// </summary>
	void SetInactive()
	{
		active = false;

		// Clean up the coroutine
		if (spawnTimerCoroutine != null)
		{
			StopCoroutine(spawnTimerCoroutine);
			spawnTimerCoroutine = null;
		}
	}
}