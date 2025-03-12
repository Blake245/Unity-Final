using UnityEngine;

/// <summary>
/// Contains detailed information about a collision or trigger event.
/// Provides a unified structure for both collision and trigger interactions.
/// </summary>
public struct CollisionInfo
{
	/// <summary>
	/// Reference to the GameObject that this object collided with.
	/// Can be used to access components or properties of the other object.
	/// </summary>
	public GameObject gameObject;

	/// <summary>
	/// World position where the interaction occurred.
	/// For collisions, this is the contact point; for triggers, it's an approximation.
	/// </summary>
	public Vector3 hitPoint;

	/// <summary>
	/// Direction from which the interaction came.
	/// For collisions, this is typically opposite to the surface normal.
	/// Can be used for calculating reflection vectors or response forces.
	/// </summary>
	public Vector3 hitDirection;

	/// <summary>
	/// Surface normal at the point of interaction.
	/// Perpendicular to the surface at the contact point.
	/// Useful for reflection calculations, orienting effects, or sliding mechanics.
	/// </summary>
	public Vector3 hitNormal;

	/// <summary>
	/// Relative velocity at the moment of interaction.
	/// Only applicable for collision-based interactions; zero for triggers.
	/// Can be used to calculate impact force or sound volume.
	/// </summary>
	public Vector3 collisionVelocity;
}