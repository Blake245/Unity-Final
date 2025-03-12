using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that allows game objects to take damage, show effects, and be destroyed.
/// Implements the IDamagable interface for standardized damage handling.
/// </summary>
public class Destructable : MonoBehaviour, IDamageable
{
	// Health configuration
	/// <summary>
	/// Current health points of this object.
	/// </summary>
	[SerializeField, Tooltip("Current health points of this object")]
	float health = 100;

	/// <summary>
	/// Maximum possible health this object can have.
	/// </summary>
	[SerializeField, Tooltip("Maximum possible health this object can have")]
	float maxHealth = 100;

	/// <summary>
	/// Optional visual effect prefab that appears when this object takes damage.
	/// Will be instantiated at the hit point location.
	/// </summary>
	[SerializeField, Tooltip("Visual effect that appears when taking damage")]
	GameObject damageFxPrefab;

	/// <summary>
	/// Optional visual effect prefab that appears when this object is destroyed.
	/// Will be instantiated at the object's position.
	/// </summary>
	[SerializeField, Tooltip("Visual effect that appears when this object is destroyed")]
	GameObject destroyFxPrefab;

	/// <summary>
	/// Whether to actually destroy the GameObject when health reaches zero.
	/// </summary>
	[SerializeField, Tooltip("Whether the GameObject should be destroyed when health reaches zero")]
	bool destroyOnDeath = true;

	/// <summary>
	/// Optional delay before destroying the GameObject after death.
	/// Useful for allowing effects to play before removal.
	/// </summary>
	[SerializeField, Tooltip("Time to wait before destroying the GameObject (in seconds)")]
	float destroyTimer = 0;

	/// <summary>
	/// Event triggered whenever this object takes damage.
	/// Passes damage information to any listeners.
	/// </summary>
	[SerializeField, Tooltip("Event triggered when damage is taken")]
	UnityEvent<DamageInfo> onDamaged;

	/// <summary>
	/// Event triggered when this object is destroyed (health reaches zero).
	/// Passes the final damage information that caused destruction.
	/// </summary>
	[SerializeField, Tooltip("Event triggered when this object is destroyed")]
	UnityEvent<DamageInfo> onDestroyed;

	/// <summary>
	/// Flag that tracks if this object has been destroyed to prevent multiple destructions.
	/// </summary>
	bool destroyed = false;

	// Public properties required by IDamagable interface
	/// <summary>
	/// Read-only access to current health value.
	/// </summary>
	public float CurrentHealth => health;

	/// <summary>
	/// Read-only access to maximum health value.
	/// </summary>
	public float MaxHealth => maxHealth;

	/// <summary>
	/// Read-only access to destroyed state.
	/// </summary>
	public bool Destroyed => destroyed;

	/// <summary>
	/// Applies damage to this object, reducing its health and potentially destroying it.
	/// Implements the IDamagable interface method.
	/// </summary>
	/// <param name="damage">Information about the damage being applied</param>
	public void ApplyDamage(DamageInfo damage)
	{
		// Prevent damage if already destroyed
		if (destroyed) return;

		// Reduce health by damage amount
		health -= damage.amount;
		// Clamp health between 0 and max health
		health = Mathf.Clamp(health, 0, maxHealth);

		// Spawn damage effect if one is set
		if (damageFxPrefab != null) Instantiate(damageFxPrefab, damage.collisionInfo.hitPoint, Quaternion.identity);

		// Check if health is depleted
		if (health <= 0)
		{
			destroyed = true;

			// Call event when destroyed, passing the damage info that caused destruction
			onDestroyed?.Invoke(damage);

			// Spawn destruction effect if one is set
			if (destroyFxPrefab != null) Instantiate(destroyFxPrefab, transform.position, Quaternion.identity);

			// Destroy this game object if configured to do so
			if (destroyOnDeath) Destroy(gameObject, destroyTimer);
		}

		// Trigger the damaged event with the damage info
		onDamaged?.Invoke(damage);
	}

	/// <summary>
	/// Restores health to this object, limited by its maximum health.
	/// Implements the IDamagable interface method.
	/// </summary>
	/// <param name="amount">Amount of health to restore</param>
	public void Heal(float amount)
	{
		// Prevent healing if already destroyed
		if (destroyed) return;

		// Increase health by healing amount
		health += amount;

		// Clamp health between 0 and max health
		health = Mathf.Clamp(health, 0, maxHealth);
	}
}