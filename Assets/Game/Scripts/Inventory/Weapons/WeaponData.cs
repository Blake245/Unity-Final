using UnityEngine;

/// <summary>
/// Scriptable Object defining weapon-specific properties.
/// Extends ItemData with additional fields for weapon functionality.
/// </summary>
[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
public class WeaponData : ItemData
{
	[Header("Firing Properties")]
	[Tooltip("Time in seconds between shots")]
	public float fireRate = 0.5f;

	[Tooltip("Random spread/inaccuracy of projectiles (x,y,z)")]
	public Vector3 spread = Vector3.one * 0.1f;

	[Header("Ammunition")]
	[Tooltip("Number of shots before reload required (0 = infinite)")]
	public int rounds;

	[Tooltip("Prefab instantiated when weapon fires")]
	public GameObject ammoPrefab;

	[Header("Audio")]
	[Tooltip("Sound played when active")]
	public AudioClip activeSound;

	[Tooltip("Sound played when reloading")]
	public AudioClip reloadSound;
}
