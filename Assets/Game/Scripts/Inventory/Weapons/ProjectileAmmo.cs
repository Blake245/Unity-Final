using UnityEngine;

/// <summary>
/// Handles physics-based projectile ammunition behavior.
/// Applies force to projectiles and manages their lifetime and collision responses.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ProjectileAmmo : Ammo
{
	/// <summary>
	/// Reference to the rigidbody component for physics manipulation.
	/// Used to apply forces and control the projectile's motion.
	/// </summary>
	private Rigidbody rb;

	/// <summary>
	/// Initializes the projectile components and sets up the collision event handler.
	/// </summary>
	private void Awake()
	{
		// Cache the rigidbody reference for better performance
		rb = GetComponent<Rigidbody>();

		// Register the collision handler method
		OnCollisionEnterEvent = OnCollisionStart;
	}

	/// <summary>
	/// Applies initial force to the projectile and schedules its destruction.
	/// Called after Awake() and all objects are initialized.
	/// </summary>
	private void Start()
	{
		// Apply initial forward force based on ammo data configuration
		if (ammoData.force != 0)
		{
			rb.AddRelativeForce(Vector3.forward * ammoData.force, ammoData.forceMode);
		}

		// Schedule object destruction after lifetime expires to prevent memory leaks
		if (ammoData.hasLifetime) Destroy(gameObject, ammoData.lifetime);
	}
}