using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A concrete implementation of the Spawner class that spawns objects within a defined volume.
/// This spawner generates random positions within the assigned collider's bounds.
/// Supports both BoxCollider and SphereCollider types for defining spawn volumes.
/// </summary>
public class SpawnerVolume : Spawner
{
	[Header("Volume")]
	[SerializeField] private Collider volume; // Collider that defines the spawn volume boundaries

	/// <summary>
	/// Implements the abstract Spawn method from the base Spawner class.
	/// Generates a random position within the assigned collider volume and spawns an object at that location.
	/// Makes multiple attempts to find a collision-free position when onlySpawnInEmptySpace is true.
	/// </summary>
	public override void Spawn()
	{
		// Validate that a collider has been assigned
		if (volume == null)
		{
			Debug.LogError("SpawnerVolume: No collider assigned for volume-based spawning!", this);
			return;
		}

		bool validPosition = false; // Flag to track if a valid spawn position has been found
		Vector3 position = transform.position; // Default position if no valid position is found

		// Handle box-shaped volumes
		var boxVolume = volume as BoxCollider;
		if (boxVolume != null)
		{
			int attempts = MAX_SPAWN_ATTEMPTS; // Maximum number of attempts to find a clear spawn position
			while (attempts-- > 0)
			{
				// Generate random position within box bounds
				position.x = Random.Range(boxVolume.bounds.min.x, boxVolume.bounds.max.x);
				position.y = Random.Range(boxVolume.bounds.min.y, boxVolume.bounds.max.y);
				position.z = Random.Range(boxVolume.bounds.min.z, boxVolume.bounds.max.z);

				// If not checking for collisions or location is clear, use this position
				if (!onlySpawnInEmptySpace || IsSpawnLocationClear(position))
				{
					validPosition = true;
					break; // Found a valid spawn location
				}
			}
		}

		// Handle sphere-shaped volumes
		var sphereVolume = volume as SphereCollider;
		if (sphereVolume != null)
		{
			int attempts = MAX_SPAWN_ATTEMPTS; // Maximum number of attempts to find a clear spawn position
			while (attempts-- > 0)
			{
				// Generate random position within sphere
				position = sphereVolume.transform.position + (Random.insideUnitSphere * sphereVolume.radius);

				// If not checking for collisions or location is clear, use this position
				if (!onlySpawnInEmptySpace || IsSpawnLocationClear(position))
				{
					validPosition = true;
					break; // Found a valid spawn location
				}
			}
		}

		// If a valid position was found, spawn the object at that location
		if (validPosition)
		{
			// Get a random prefab from the base class
			GameObject spawnGameObject = GetSpawnObject();
			if (spawnGameObject == null) return; // Prevents null object instantiation

			// Call the base class Spawn method to handle instantiation and tracking
			Spawn(spawnGameObject, position, transform.rotation);
		}
		else
		{
			Debug.LogWarning("SpawnerVolume: Unable to find a valid spawn position after multiple attempts!", this);
		}
	}
}