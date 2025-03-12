using UnityEngine;

/// <summary>
/// Generic interface for an object pool.
/// Defines the contract for retrieving and returning objects in a pool.
/// </summary>
/// <typeparam name="T">The type of object being pooled (e.g., GameObject, Component).</typeparam>
public interface IPool<T>
{
	/// <summary>
	/// Retrieves an object from the pool.
	/// If the pool is empty, a new object may be created.
	/// </summary>
	/// <returns>An instance of type T from the pool.</returns>
	T Get();

	/// <summary>
	/// Returns an object back to the pool for reuse.
	/// The object may be deactivated or reset depending on the pool's implementation.
	/// </summary>
	/// <param name="_object">The object to return to the pool.</param>
	void Release(T _object);
}

