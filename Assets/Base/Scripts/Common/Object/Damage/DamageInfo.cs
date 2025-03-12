using UnityEngine;

/// <summary>
/// Structure containing detailed information about damage events.
/// Used to pass damage data between components in a standardized format.
/// </summary>
public struct DamageInfo
{
	/// <summary>
	/// Categorizes the type of damage being inflicted.
	/// </summary>
	public enum DamageType { Physical, Ammo, Fire, Fall, Zone }

	/// <summary>
	/// Information about the entity that inflicted the damage.
	/// </summary>
	public GameObject inflictor;

	/// <summary>
	/// The amount of damage to apply to the target.
	/// </summary>
	public float amount;

	/// <summary>
	/// The type of damage being applied.
	/// Used for damage resistances and special effects.
	/// </summary>
	public DamageType type;

	/// <summary>
	///	</summary>
	public CollisionInfo collisionInfo;
}