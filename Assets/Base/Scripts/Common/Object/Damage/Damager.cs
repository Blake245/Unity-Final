using UnityEngine;

/// <summary>
/// A component that applies damage to objects implementing the IDamageable interface upon collision.
/// Extends CollisionEventReceiver to handle collision events.
/// </summary>
[RequireComponent(typeof(CollisionProcessor))]
public class Damager : CollisionEventReceiver
{
	[SerializeField]
	[Tooltip("The type of damage this component will inflict (e.g. Fire, Poison, Physical)")]
	DamageInfo.DamageType damageType;

	[SerializeField]
	[Tooltip("Amount of damage to apply on each hit")]
	float damage;
	[SerializeField]
	[Tooltip("Time between damage applications. Set to 0 for instant-only damage, or > 0 for continuous damage.")]
	private float damageRate = 0f;
	[SerializeField]
	[Tooltip("If enabled, this game object will be destroyed after inflicting damage")]
	bool destroyOnDamage;

	[SerializeField]
	[Tooltip("Visual effect prefab to spawn when damage is applied (optional)")]
	GameObject damagePrefab;

	[SerializeField]
	[Tooltip("Specifies which layers this damager can affect. Objects on other layers will be ignored")]
	LayerMask layerMask = Physics.AllLayers;

	private float lastDamageTime;

	/// <summary>
	/// Registers collision event handlers on startup
	/// </summary>
	private void Start()
	{
		// Register the collision handler method
		OnCollisionEnterEvent = OnCollisionStart;
		if (damageRate > 0) OnCollisionStayEvent = OnCollisionActive;
	}

	/// <summary>
	/// Called when this object first makes contact with another collider.
	/// </summary>
	/// <param name="info">Information about the collision</param>
	public void OnCollisionStart(CollisionInfo info)
	{
		// Check if the collided object's layer is in our layerMask
		if ((layerMask & (1 << info.gameObject.layer)) == 0) return;

		// Always apply damage on first hit regardless of damageRate
		ApplyDamage(info);
		SpawnDamageEffect();

		// Update timestamp for potential continuous damage
		lastDamageTime = Time.time;

		// If configured to destroy on damage, destroy this game object
		if (destroyOnDamage)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Called every frame while this object remains in contact with another collider.
	/// Used for continuous damage effects.
	/// </summary>
	/// <param name="info">Information about the ongoing collision</param>
	public void OnCollisionActive(CollisionInfo info)
	{
		// If damageRate is 0, we only apply damage on collision start
		if (damageRate <= 0) return;

		// Check if the collided object's layer is in our layerMask
		if ((layerMask & (1 << info.gameObject.layer)) == 0) return;

		// Check if enough time has passed since last damage application
		if (Time.time < lastDamageTime + damageRate) return;

		ApplyDamage(info);
		SpawnDamageEffect();

		// Update timestamp for rate limiting
		lastDamageTime = Time.time;
	}

	/// <summary>
	/// Applies damage to the target if it implements IDamageable
	/// </summary>
	private void ApplyDamage(CollisionInfo info)
	{
		// Check if the target object has an IDamageable component
		if (info.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
		{
			// Create and populate damage information
			DamageInfo damageInfo = new DamageInfo
			{
				inflictor = gameObject,                 // Who/what caused the damage
				amount = damage,                        // How much damage to apply
				type = damageType,                      // What type of damage (fire, ice, physical, etc.)
				collisionInfo = info                    // Details about the collision
			};
			damageable.ApplyDamage(damageInfo);         // Apply the damage to the hit object
		}
	}

	/// <summary>
	/// Spawns the damage effect prefab if one is specified
	/// </summary>
	private void SpawnDamageEffect()
	{
		if (damagePrefab != null)
		{
			Instantiate(damagePrefab, transform.position, transform.rotation);
		}
	}
}