using UnityEngine;

/// <summary>
/// Interface for any entity that can receive damage in the game.
/// Provides default implementations for common methods.
/// </summary>
public interface IDamageable
{
	/// <summary>
	/// Gets the current health of the entity.
	/// </summary>
	float CurrentHealth { get; }

	/// <summary>
	/// Gets the maximum possible health for the entity.
	/// </summary>
	float MaxHealth { get; }

	/// <summary>
	/// Determines if the entity is currently alive.
	/// Returns true when CurrentHealth is greater than zero.
	/// </summary>
	bool IsAlive => CurrentHealth > 0;

	/// <summary>
	/// Indicates whether the entity has been destroyed.
	/// </summary>
	bool Destroyed { get; }

	/// <summary>
	/// Applies damage to the entity based on the provided damage information.
	/// Default implementation logs the damage amount to the console.
	/// </summary>
	/// <param name="damageInfo">Information about the damage being applied</param>
	void ApplyDamage(DamageInfo damageInfo) => Debug.Log($"Apply Damage: {damageInfo.amount}");

	/// <summary>
	/// Heals the entity by the specified amount.
	/// Default implementation logs the healing amount to the console.
	/// </summary>
	/// <param name="amount">The amount of health to restore</param>
	void Heal(float amount) => Debug.Log($"Heal: {amount}");
}