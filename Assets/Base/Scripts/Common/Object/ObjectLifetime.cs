using UnityEngine;

/// <summary>
/// Destroys the attached GameObject after a specified lifetime.
/// </summary>
public class ObjectLifetime : MonoBehaviour
{
	/// <summary>
	/// The time in seconds before the GameObject is destroyed.
	/// </summary>
	[SerializeField] private float lifetime = 1.0f;

	/// <summary>
	/// If true, the lifetime will be randomly selected between minLifetime and maxLifetime.
	/// </summary>
	[SerializeField] private bool useRandomRange = false;

	/// <summary>
	/// The minimum possible lifetime when useRandomRange is enabled.
	/// </summary>
	[SerializeField] private float minLifetime = 0.5f;

	/// <summary>
	/// The maximum possible lifetime when useRandomRange is enabled.
	/// </summary>
	[SerializeField] private float maxLifetime = 1.5f;

	/// <summary>
	/// Initializes the destruction timer when this component starts.
	/// </summary>
	void Start()
	{
		// Determine the actual lifetime based on whether random range is enabled
		float actualLifetime = useRandomRange ? Random.Range(minLifetime, maxLifetime) : lifetime;

		// Schedule the GameObject for destruction after the calculated lifetime
		Destroy(gameObject, actualLifetime);
	}
}