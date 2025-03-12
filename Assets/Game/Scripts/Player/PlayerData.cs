using UnityEngine;

/// <summary>
/// Scriptable Object that stores all player movement and physics parameters.
/// Can be created in the Unity editor and shared across different player instances.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
	/// <summary>
	/// Base movement speed of the player in meters per second.
	/// </summary>
	[Range(0, 20), Tooltip("Base movement speed of the player in m/s")]
	public float speed = 3;

	/// <summary>
	/// Maximum speed when sprinting in meters per second.
	/// </summary>
	[Range(0, 20), Tooltip("Maximum speed when sprinting in m/s")]
	public float sprintSpeed = 6;

	/// <summary>
	/// How quickly the player reaches maximum speed.
	/// Higher values mean more responsive movement.
	/// </summary>
	[Range(0, 50), Tooltip("How quickly the player reaches maximum speed")]
	public float acceleration = 3;

	/// <summary>
	/// How quickly the player slows down when no input is given while on ground.
	/// </summary>
	[Range(0, 50), Tooltip("How quickly the player slows down on ground")]
	public float deceleration = 10;

	/// <summary>
	/// How quickly the player slows down when no input is given while in air.
	/// Usually lower than ground deceleration for more floaty jumps.
	/// </summary>
	[Range(0, 50), Tooltip("How quickly the player slows down in air")]
	public float decelerationInAir = 4;

	/// <summary>
	/// Maximum height of the player's jump in meters.
	/// </summary>
	[Range(0, 10), Tooltip("Maximum height of the player's jump in meters")]
	public float jumpHeight = 2;

	/// <summary>
	/// Gravity force applied to the player. Negative values pull downward.
	/// </summary>
	[Range(0, -20), Tooltip("Gravity force applied to the player (negative values)")]
	public float gravity = -9.8f;

	/// <summary>
	/// How quickly the player rotates to face movement direction.
	/// Higher values result in faster turning.
	/// </summary>
	[Range(0, 20), Tooltip("How quickly the player rotates to face movement direction")]
	public float turnRate = 1;

	/// <summary>
	/// Force applied to rigidbodies that the player pushes into.
	/// </summary>
	[Range(0, 10), Tooltip("Force applied to rigidbodies that the player pushes")]
	public float pushForce = 5;

	[Header("Wall Run Data")]

	[Range(0, 20)]
	public float wallJumpforce;

    [Range(0, 20)]
    public float wallJumpUpforce;

	[Range(0, 50)]
	public float maxCameraTilt;

    [Range(0, 10)]
    public float maxWallRunTime;

    [Range(0, 10)]
	public float maxWallSpeed;
}