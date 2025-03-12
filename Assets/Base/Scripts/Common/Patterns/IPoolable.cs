using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Interface for objects that can be pooled.
/// Provides methods for handling object spawning and despawning.
/// </summary>
/// <typeparam name="T">The type of object being pooled (e.g., GameObject, Component).</typeparam>
public interface IPoolable<T>
{
	/// <summary>
	/// Reference to the pool that this object belongs to.
	/// This allows the object to return itself to the pool when no longer needed.
	/// </summary>
	IPool<T> Pool { get; set; }

	/// <summary>
	/// Called when the object is retrieved from the pool.
	/// Used to reset or initialize the object before it becomes active.
	/// </summary>
	void OnSpawn();

	/// <summary>
	/// Called when the object is returned to the pool.
	/// Used to clean up, reset, or deactivate the object before it is stored.
	/// </summary>
	void OnDespawn();
}

