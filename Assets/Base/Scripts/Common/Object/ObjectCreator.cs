using UnityEngine;

/// <summary>
/// Creates objects from a prefab at specified positions and rotations.
/// Can create automatically on start or manually via function calls.
/// </summary>
public class ObjectCreator : MonoBehaviour
{
	[SerializeField] GameObject prefab;      // The prefab to instantiate
	[SerializeField] Transform parent;       // Optional parent transform for the instantiated object
	[SerializeField] bool createOnStart = true;  // Whether to create an object automatically on Start

	/// <summary>
	/// Validates component settings in the Unity Editor.
	/// Warns if prefab is not assigned to prevent errors at runtime.
	/// </summary>
	private void OnValidate()
	{
		if (prefab == null)
		{
			Debug.LogWarning("Prefab is not assigned in ObjectCreator on " + gameObject.name);
		}
	}

	/// <summary>
	/// Called when the script instance is being loaded.
	/// Creates an object if createOnStart is enabled.
	/// </summary>
	void Start()
	{
		if (createOnStart) Create();
	}

	/// <summary>
	/// Instantiates a new GameObject from the prefab.
	/// </summary>
	/// <param name="position">Optional position override. Uses transform.position if null.</param>
	/// <param name="rotation">Optional rotation override. Uses transform.rotation if null.</param>
	/// <returns>The instantiated GameObject, or null if prefab is not assigned.</returns>
	public GameObject Create(Vector3? position = null, Quaternion? rotation = null)
	{
		if (prefab == null)
		{
			Debug.LogError("Prefab is not assigned in ObjectCreator on " + gameObject.name + ". Cannot create object.");
			return null;
		}

		// Use provided position/rotation or fall back to transform's values
		Vector3 spawnPosition = position ?? transform.position;
		Quaternion spawnRotation = rotation ?? transform.rotation;

		// Instantiate the object and store the reference
		GameObject instance = Instantiate(prefab, spawnPosition, spawnRotation, parent);

		return instance;
	}
}