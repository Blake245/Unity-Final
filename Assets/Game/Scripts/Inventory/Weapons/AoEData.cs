using UnityEngine;

/// <summary>
/// Scriptable Object that defines Area of Effect (AoE) properties for weapons and ammunition.
/// Used for explosive or splash damage effects like grenades, rockets, or bombs.
/// </summary>
[CreateAssetMenu(fileName = "AoE", menuName = "Weapon/AoE")]
public class AoEData : ScriptableObject
{
	/// <summary>
	/// The maximum distance from the explosion center that will be affected by the AoE.
	/// </summary>
	[Tooltip("Radius of the explosion effect in world units")]
	public float radius = 5f;

	/// <summary>
	/// Defines how damage decreases from center to edge of the explosion.
	/// Value of 1 at time 0 means full damage at center.
	/// Value of 0 at time 1 means no damage at the edge of radius.
	/// </summary>
	[Tooltip("Controls how damage falls off from center (1) to edge (0) of explosion")]
	public AnimationCurve damageFalloff = AnimationCurve.Linear(0, 1, 1, 0);

	/// <summary>
	/// Determines which layers will be affected by the explosion.
	/// Use this to ensure explosions only damage appropriate objects.
	/// </summary>
	[Tooltip("Which layers will be affected by the explosion")]
	public LayerMask affectedLayers = Physics.AllLayers;

	/// <summary>
	/// Whether the explosion applies physical force to affected objects with Rigidbodies.
	/// </summary>
	[Tooltip("If enabled, explosion will push affected objects with Rigidbodies")]
	public bool applyForceToAffectedObjects = true;

	/// <summary>
	/// The amount of physical force applied to affected objects.
	/// Only used if applyForceToAffectedObjects is true.
	/// </summary>
	[Tooltip("Strength of the physical force applied to objects")]
	public float force = 10f;

	/// <summary>
	/// How the physical force is applied to affected objects.
	/// Impulse applies an instantaneous force, Force applies a continuous force, etc.
	/// </summary>
	[Tooltip("How the force is applied: Impulse (instant), Force (continuous), etc.")]
	public ForceMode forceMode = ForceMode.Impulse;

	/// <summary>
	/// Visual effect prefab that will be instantiated at the explosion point.
	/// Can include particle effects, light flashes, etc.
	/// </summary>
	[Tooltip("Visual effect spawned when the explosion occurs")]
	public GameObject effectPrefab;
}