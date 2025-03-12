using System.Collections;
using UnityEngine;

/// <summary>
/// Handles weapon behavior including firing mechanics, ammunition management, and animation.
/// Implements the Item base class for integration with inventory and equipment systems.
/// Supports different firing modes (single, auto, burst, stream) based on weapon data.
/// </summary>
public class Weapon : Item
{
	/// <summary>
	/// Data containing weapon-specific properties and configuration.
	/// </summary>
	[SerializeField, Tooltip("Scriptable Object with all weapon-specific settings")]
	WeaponData weaponData;

	/// <summary>
	/// Reference to the animator component for weapon animations.
	/// </summary>
	[SerializeField, Tooltip("Animator component for weapon animations")]
	Animator animator;

	//[SerializeField] RigBuilder rigBuilder;

	/// <summary>
	/// Transform where ammunition will be instantiated when firing.
	/// </summary>
	[SerializeField, Tooltip("Point where projectiles will be spawned")]
	Transform ammoTransform;

	/// <summary>
	/// Current ammunition count available for this weapon.
	/// </summary>
	private int ammoCount = 0;

	/// <summary>
	/// Whether the weapon is ready to fire (not in cooldown).
	/// </summary>
	private bool weaponReady = false;

	/// <summary>
	/// Coroutine reference for automatic firing mode.
	/// </summary>
	private IEnumerator autoFireCoroutine;

	/// <summary>
	/// Initialize weapon state and references.
	/// </summary>
	private void Start()
	{
		autoFireCoroutine = AutoFire();
		if (ammoTransform == null) ammoTransform = transform;
	}

	/// <summary>
	/// Returns the weapon's data.
	/// </summary>
	/// <returns>The weapon's data as ItemData</returns>
	public override ItemData GetData() { return weaponData; }

	/// <summary>
	/// Equips the weapon, making it visible and ready to use.
	/// Sets up animation parameters and rig weights.
	/// </summary>
	public override void Equip()
	{
		base.Equip();
		weaponReady = true;
		if (weaponData.animEquipName != "") animator.SetBool(weaponData.animEquipName, true);
		for (int i = 0; i < weaponData.rigLayerWeight.Length; i++)
		{
			//rigBuilder.layers[i].rig.weight = weaponData.rigLayerWeight[i];
		}
	}

	/// <summary>
	/// Unequips the weapon, hiding it and updating animation state.
	/// </summary>
	public override void Unequip()
	{
		base.Unequip();
		if (weaponData.animEquipName != "") animator.SetBool(weaponData.animEquipName, false);
	}

	/// <summary>
	/// Initiates weapon use (firing) based on the weapon's usage type.
	/// Handles animation triggering and projectile spawning.
	/// </summary>
	public override void Use()
	{
		if (!weaponReady) return;

		// Trigger weapon animation if trigger name set and animator exists
		// Ammo will be created through animation event
		if (weaponData.animTriggerName != "" && animator != null)
		{
			animator.SetTrigger(weaponData.animTriggerName);
			weaponReady = false;
		}
		else
		{
			// Create ammo prefab directly if no animation is specified
			if (weaponData.usageType == UsageType.Single || weaponData.usageType == UsageType.Burst)
			{
				// For single-shot weapons, spawn one projectile
				Instantiate(weaponData.ammoPrefab, ammoTransform.position, ammoTransform.rotation);

				// Apply cooldown between shots if fire rate is specified
				if (weaponData.fireRate > 0)
				{
					weaponReady = false;
					StartCoroutine(ResetFireTimer());
				}
			}
			else
			{
				// For automatic weapons, start the continuous firing coroutine
				StartCoroutine(autoFireCoroutine);
			}
		}
	}

	/// <summary>
	/// Stops weapon use, interrupting automatic fire and resetting state.
	/// </summary>
	public override void StopUse()
	{
		if (weaponData.usageType == UsageType.Single || weaponData.usageType == UsageType.Burst) weaponReady = true;
		StopCoroutine(autoFireCoroutine);
	}

	/// <summary>
	/// Checks if the weapon is ready to be used.
	/// Considers cooldown state and ammunition count.
	/// </summary>
	/// <returns>True if the weapon can be fired, false otherwise</returns>
	public override bool IsReady()
	{
		// Check if weapon is not in cooldown and has ammo (or doesn't require ammo)
		return weaponReady && (ammoCount > 0 || weaponData.rounds == 0);
	}

	/// <summary>
	/// Called by animation events during the weapon firing sequence.
	/// Spawns projectiles at the designated point in the animation.
	/// </summary>
	public override void OnAnimEventItemUse()
	{
		// Create ammo prefab at the spawn point
		Instantiate(weaponData.ammoPrefab, ammoTransform.position, ammoTransform.rotation);
	}

	/// <summary>
	/// Coroutine that handles the weapon cooldown between shots.
	/// </summary>
	/// <returns>Wait time based on weapon's fire rate</returns>
	IEnumerator ResetFireTimer()
	{
		yield return new WaitForSeconds(weaponData.fireRate);
		weaponReady = true;
	}

	/// <summary>
	/// Coroutine that handles automatic weapon firing.
	/// Continuously spawns projectiles at intervals based on the fire rate.
	/// </summary>
	/// <returns>Wait time between automatic shots</returns>
	IEnumerator AutoFire()
	{
		while (true)
		{
			Instantiate(weaponData.ammoPrefab, ammoTransform.position, ammoTransform.rotation);
			yield return new WaitForSeconds(weaponData.fireRate);
		}
	}
}