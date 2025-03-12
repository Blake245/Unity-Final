using UnityEngine;

/// <summary>
/// Implements hit-scan (instant ray) based ammo that deals damage on a successful raycast hit
/// </summary>
public class HitScanAmmo : Ammo
{
	private void Start()
	{
		//Debug.DrawRay(transform.position, transform.forward * ammoData.distance, Color.red, 5f);

		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, ammoData.distance, ammoData.hitLayerMask))
		{
			// Check if the hit object is damageable
			if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
			{
				// Only generate collision info if we're applying damage
				CollisionInfo collisionInfo = new CollisionInfo
				{
					gameObject = hit.collider.gameObject,
					hitPoint = hit.point,
					hitDirection = transform.forward,
					hitNormal = hit.normal,
					collisionVelocity = transform.forward * ammoData.force // Approximate collision velocity
				};

				DamageInfo damageInfo = new DamageInfo
				{
					inflictor = gameObject,
					amount = ammoData.damage,
					type = DamageInfo.DamageType.Ammo,
					collisionInfo = collisionInfo
				};
				damageable.ApplyDamage(damageInfo);
			}

			// Spawn hit effect if configured
			if (ammoData.impactPrefab != null)
			{
				Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
				Instantiate(ammoData.impactPrefab, hit.point, hitRotation);
			}
		}

		// Always destroy the hitscan projectile after processing
		Destroy(gameObject);
	}
}