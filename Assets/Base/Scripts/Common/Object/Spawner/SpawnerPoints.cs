using System.Linq; // Required for the Select extension method
using UnityEngine;

/// <summary>
/// A concrete implementation of the Spawner class that spawns objects at predefined points.
/// This spawner randomly selects from an array of Transform points for each spawn operation.
/// Points can be manually assigned in the inspector or found by tag at runtime.
/// </summary>
public class SpawnerPoints : Spawner
{
	[Header("Points")]
	[SerializeField] Transform[] points; // Array of transform points where objects can be spawned
	[SerializeField] string tagName;     // Tag to identify spawn points in the scene

	/// <summary>
	/// Called when the script instance is being loaded.
	/// Automatically finds spawn points based on tag if none are manually assigned.
	/// </summary>
	void Awake()
	{
		// Check if points are not assigned AND we have a valid tag to search for
		if ((points == null || points.Length == 0) && !string.IsNullOrEmpty(tagName))
		{
			// Find all GameObjects with the specified tag and convert to Transform array
			points = GameObject.FindGameObjectsWithTag(tagName)
							  .Select(go => go.transform)
							  .ToArray();

			// Log warning if no objects with the tag were found
			if (points.Length == 0)
			{
				Debug.LogWarning($"SpawnerPoints: No GameObjects found with tag '{tagName}'");
			}
		}
	}

	/// <summary>
	/// Implements the abstract Spawn method from the base Spawner class.
	/// Randomly selects a spawn point from the points array and spawns an object at that location.
	/// If onlySpawnInEmptySpace is true, makes multiple attempts to find a clear spawn location.
	/// </summary>
	public override void Spawn()
	{
		// Validate that spawn points have been assigned
		if (points == null || points.Length == 0)
		{
			Debug.LogError("SpawnerPoints: No spawn points assigned!");
			return;
		}

		// Find a spawn point
		Transform spawnTransform = null;
		int attempts = MAX_SPAWN_ATTEMPTS; // Maximum number of attempts to find a clear spawn position
		while (attempts-- > 0)
		{
			// Select a random spawn point from the array
			spawnTransform = points[Random.Range(0, points.Length)];

			// If we don't need empty space or the space is clear, exit the loop
			if (!onlySpawnInEmptySpace || IsSpawnLocationClear(spawnTransform.position))
				break;
		}

		// Call the base class Spawn method to handle instantiation and tracking
		if (spawnTransform != null)
		{
			// Get a prefab from the base class
			GameObject spawnGameObject = GetSpawnObject();
			if (spawnGameObject == null) return; // Prevents null object instantiation

			Spawn(spawnGameObject, spawnTransform.position, spawnTransform.rotation);
		}
		else
		{
			Debug.LogWarning("SpawnerPoints: Unable to find a valid spawn position after multiple attempts!", this);
		}
	}
}