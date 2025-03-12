using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammo : CollisionEventReceiver
{
	[SerializeField] protected AmmoData ammoData;
	
	public void OnCollisionStart(CollisionInfo info)
	{
		if ((ammoData.hitLayerMask & (1 << info.gameObject.layer)) == 0) return;

		// apply damage if game object is damagable
		if (!ammoData.damageOverTime && info.gameObject.TryGetComponent<IDamageable>(out IDamageable damagable))
		{
			DamageInfo damageInfo = new DamageInfo
			{
				inflictor = gameObject,
				amount = ammoData.damage,
				type = DamageInfo.DamageType.Ammo,
				collisionInfo = info
			};
			damagable.ApplyDamage(damageInfo);
		}

		// create impact prefab
		if (ammoData.impactPrefab != null)
		{
			Instantiate(ammoData.impactPrefab, transform.position, transform.rotation);
		}

		// destroy game object
		if (ammoData.destroyOnImpact)
		{
			Destroy(gameObject);
		}
	}

	public void OnCollisionActive(CollisionInfo info)
	{
		if ((ammoData.hitLayerMask & (1 << info.gameObject.layer)) == 0) return;

		// apply damage if game object is damagable
		if (ammoData.damageOverTime && gameObject.TryGetComponent<IDamageable>(out IDamageable damagable))
		{
			DamageInfo damageInfo = new DamageInfo
			{
				inflictor = gameObject,
				amount = ammoData.damage,
				type = DamageInfo.DamageType.Ammo,
				collisionInfo = info
			};
			damagable.ApplyDamage(damageInfo);
		}
	}

	public void OnCollisionEnd(InteractorInfo info)
	{
		//
	}
}
