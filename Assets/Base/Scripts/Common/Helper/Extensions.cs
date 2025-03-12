using UnityEngine;

/// <summary>
/// Provides utility extension methods for common Vector3 operations.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Returns a new Vector3 with the same X and Z components but with Y set to zero.
	/// Useful for horizontal plane calculations that should ignore vertical differences.
	/// </summary>
	/// <param name="v">The Vector3 to process</param>
	/// <returns>A new Vector3 with Y=0</returns>
	public static Vector3 VectorXZ(this Vector3 v)
	{
		return new Vector3(v.x, 0, v.z);
	}

	/// <summary>
	/// Calculates the magnitude of a vector projected onto the XZ plane.
	/// This gives the horizontal length of the vector, ignoring its height.
	/// </summary>
	/// <param name="v">The Vector3 to calculate the horizontal magnitude of</param>
	/// <returns>The magnitude of the vector when projected onto the XZ plane</returns>
	public static float MagnitudeXZ(this Vector3 v)
	{
		return new Vector3(v.x, 0, v.z).magnitude;
	}

	/// <summary>
	/// Calculates the horizontal distance between two Vector3 points,
	/// ignoring any difference in height (Y component).
	/// </summary>
	/// <param name="a">The first position</param>
	/// <param name="b">The second position</param>
	/// <returns>The distance between points a and b projected onto the XZ plane</returns>
	public static float DistanceXZ(this Vector3 a, Vector3 b)
	{
		Vector3 v = b - a;
		v.y = 0;
		return v.magnitude;
	}
}