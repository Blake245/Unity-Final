using UnityEngine;

/// <summary>
/// Defines the type of ammunition behavior for weapons.
/// </summary>
public enum AmmoType
{
	/// <summary>
	/// Projectile that uses physics and colliders for detection.
	/// Follows a trajectory and can be affected by gravity.
	/// </summary>
	Projectile,

	/// <summary>
	/// Instant hit detection using raycasts.
	/// Commonly used for firearms and laser weapons.
	/// </summary>
	HitScan
}

/// <summary>
/// ScriptableObject that defines ammunition properties and behavior.
/// Controls how projectiles function, damage calculation, and impact effects.
/// </summary>
[CreateAssetMenu(fileName = "Ammo", menuName = "Weapon/Ammo")]
public class AmmoData : ScriptableObject
{
	/// <summary>
	/// Determines the core behavior of the ammunition (Projectile or HitScan).
	/// </summary>
	[Tooltip("The fundamental behavior type of this ammunition")]
	public AmmoType ammoType;

	/// <summary>
	/// How long the projectile exists in the world before being destroyed (in seconds).
	/// Only used if hasLifetime is true.
	/// </summary>
	[Tooltip("Duration in seconds before the projectile is automatically destroyed")]
	public float lifetime;

	/// <summary>
	/// Whether the projectile has a limited lifetime.
	/// If false, the projectile will exist until it hits something or is manually destroyed.
	/// </summary>
	[Tooltip("If true, the projectile will be destroyed after lifetime seconds")]
	public bool hasLifetime;

	/// <summary>
	/// Base damage value dealt to targets hit by this ammunition.
	/// </summary>
	[Tooltip("Amount of damage dealt to targets hit by this ammunition")]
	public float damage;

	/// <summary>
	/// If true, the projectile applies damage over time rather than all at once.
	/// Useful for lingering effects like fire or acid.
	/// </summary>
	[Tooltip("If true, damage is applied continuously while in contact")]
	public bool damageOverTime;

	/// <summary>
	/// Whether the projectile is destroyed when it hits something.
	/// If false, the projectile may pass through targets or bounce.
	/// </summary>
	[Tooltip("If true, the projectile is destroyed upon hitting a valid target")]
	public bool destroyOnImpact;

	/// <summary>
	/// Visual effect prefab spawned when the projectile hits something.
	/// Can include particle effects, decals, or other visual feedback.
	/// </summary>
	[Tooltip("Visual effect spawned at the point of impact")]
	public GameObject impactPrefab;

	/// <summary>
	/// Area of Effect data for explosive ammunition.
	/// If assigned, creates an explosion at the impact point.
	/// </summary>
	[Tooltip("If assigned, creates an area damage effect on impact")]
	public AoEData aoeData;

	/// <summary>
	/// Layers that the hit detection will check against.
	/// Use to exclude certain objects from being hit.
	/// </summary>
	[Tooltip("Which layers will be detected by this ammunition")]
	public LayerMask hitLayerMask = Physics.AllLayers;

	#region Projectile Ammo Properties

	[Header("Projectile")]
	/// <summary>
	/// Force applied to the projectile's Rigidbody when launched.
	/// Controls the projectile's initial velocity and momentum.
	/// </summary>
	[Tooltip("Force applied to projectile Rigidbody when fired")]
	public float force;

	/// <summary>
	/// How the force is applied to the projectile's Rigidbody.
	/// Affects the projectile's acceleration behavior.
	/// </summary>
	[Tooltip("How the force is applied: Impulse (instant), Force (continuous), etc.")]
	public ForceMode forceMode;

	/// <summary>
	/// If true, the projectile is affected by gravity.
	/// Creates natural arcing trajectories for projectiles like grenades or arrows.
	/// </summary>
	[Tooltip("If true, projectile will be affected by gravity and follow an arc")]
	public bool hasGravity;

	/// <summary>
	/// Whether the projectile bounces off surfaces instead of impacting.
	/// Physics materials will affect the bounce behavior.
	/// </summary>
	[Tooltip("If true, projectile will bounce off surfaces instead of being destroyed")]
	public bool bounce;

	/// <summary>
	/// If true, the projectile rotates to align with its movement direction.
	/// Useful for arrows, missiles, etc.
	/// </summary>
	[Tooltip("If true, projectile will orient itself along its velocity vector")]
	public bool rotateToVelocity;

	/// <summary>
	/// If true, the projectile will trigger its impact effect when its lifetime expires.
	/// Useful for timed explosives or flares.
	/// </summary>
	[Tooltip("If true, impact effects trigger when lifetime expires")]
	public bool impactOnExpired;

	#endregion

	#region HitScan Ammo Properties

	[Header("HitScan")]
	/// <summary>
	/// Maximum distance the hit detection will check.
	/// Longer distances may impact performance.
	/// </summary>
	[Tooltip("Maximum distance in world units the weapon can hit")]
	public float distance;

	#endregion
}