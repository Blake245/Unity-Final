using UnityEngine;

/// <summary>
/// Detects and processes Unity collision and trigger events, forwarding them to a CollisionEventReceiver.
/// Acts as a bridge between Unity's collision system and custom collision event handling.
/// </summary>
public class CollisionProcessor : MonoBehaviour
{
	/// <summary>
	/// Reference to the collision receiver that will handle collision events.
	/// </summary>
	private CollisionEventReceiver receiver;

	/// <summary>
	/// Finds and caches the collision event receiver on this object or its parent.
	/// </summary>
	private void Awake()
	{
		// Find the component that will handle the collisions
		receiver = GetComponent<CollisionEventReceiver>() ?? GetComponentInParent<CollisionEventReceiver>();

		if (receiver == null)
		{
			Debug.LogWarning("No CollisionEventReceiver implementation found on " + gameObject.name);
		}
	}

	/// <summary>
	/// Called when this collider/rigidbody begins touching another rigidbody/collider.
	/// Invokes the OnCollisionEnterEvent if a target receiver exists.
	/// </summary>
	/// <param name="collision">Information about the collision</param>
	private void OnCollisionEnter(Collision collision)
	{
		if (receiver == null || receiver.OnCollisionEnterEvent == null) return;

		CollisionInfo info = CreateCollisionInfo(collision);
		receiver.OnCollisionEnterEvent(info);
	}

	/// <summary>
	/// Called when another collider enters a trigger collider attached to this object.
	/// Invokes the OnCollisionEnterEvent if a target receiver exists.
	/// </summary>
	/// <param name="other">The collider that entered the trigger</param>
	private void OnTriggerEnter(Collider other)
	{
		if (receiver == null || receiver.OnCollisionEnterEvent == null) return;

		CollisionInfo info = CreateTriggerInfo(other, transform);
		receiver.OnCollisionEnterEvent(info);
	}

	/// <summary>
	/// Called every frame while this collider/rigidbody is touching another rigidbody/collider.
	/// Invokes the OnCollisionStayEvent if a target receiver exists.
	/// </summary>
	/// <param name="collision">Information about the collision</param>
	private void OnCollisionStay(Collision collision)
	{
		if (receiver == null || receiver.OnCollisionStayEvent == null) return;

		CollisionInfo info = CreateCollisionInfo(collision);
		receiver.OnCollisionStayEvent(info);
	}

	/// <summary>
	/// Called every frame while another collider is within a trigger collider attached to this object.
	/// Invokes the OnCollisionStayEvent if a target receiver exists.
	/// </summary>
	/// <param name="other">The collider that is staying in the trigger</param>
	private void OnTriggerStay(Collider other)
	{
		if (receiver == null || receiver.OnCollisionStayEvent == null) return;

		CollisionInfo info = CreateTriggerInfo(other, transform);
		receiver.OnCollisionStayEvent(info);
	}

	/// <summary>
	/// Called when this collider/rigidbody stops touching another rigidbody/collider.
	/// Invokes the OnCollisionExitEvent if a target receiver exists.
	/// </summary>
	/// <param name="collision">Information about the collision that ended</param>
	private void OnCollisionExit(Collision collision)
	{
		if (receiver == null || receiver.OnCollisionExitEvent == null) return;

		CollisionInfo info = CreateCollisionInfo(collision);
		receiver.OnCollisionExitEvent(info);
	}

	/// <summary>
	/// Called when another collider exits a trigger collider attached to this object.
	/// Invokes the OnCollisionExitEvent if a target receiver exists.
	/// </summary>
	/// <param name="other">The collider that exited the trigger</param>
	private void OnTriggerExit(Collider other)
	{
		if (receiver == null || receiver.OnCollisionExitEvent == null) return;

		CollisionInfo info = CreateTriggerInfo(other, transform);
		receiver.OnCollisionExitEvent(info);
	}

	/// <summary>
	/// Creates a CollisionInfo struct from a Unity Collision object.
	/// Extracts contact points, normals, and velocities when available.
	/// </summary>
	/// <param name="collision">Unity collision data</param>
	/// <returns>A filled CollisionInfo struct with collision data</returns>
	private static CollisionInfo CreateCollisionInfo(Collision collision)
	{
		CollisionInfo info = new CollisionInfo();

		if (collision.contactCount > 0)
		{
			ContactPoint contact = collision.contacts[0];
			info.gameObject = collision.gameObject;
			info.hitPoint = contact.point;
			info.hitNormal = contact.normal;
			info.hitDirection = -contact.normal;
			info.collisionVelocity = collision.relativeVelocity;
		}
		else
		{
			// Fallback if no contact points
			info.gameObject = collision.gameObject;
			info.hitPoint = collision.transform.position;
			info.hitNormal = Vector3.up;
			info.hitDirection = Vector3.down;
			info.collisionVelocity = collision.relativeVelocity;
		}

		return info;
	}

	/// <summary>
	/// Creates a CollisionInfo struct from a trigger event.
	/// Approximates contact information since triggers don't provide detailed collision data.
	/// </summary>
	/// <param name="other">The collider involved in the trigger event</param>
	/// <param name="transform">Transform of this object</param>
	/// <returns>A filled CollisionInfo struct with approximated trigger data</returns>
	private static CollisionInfo CreateTriggerInfo(Collider other, Transform transform)
	{
		CollisionInfo info = new CollisionInfo();

		// For triggers, we don't have contact points, so we make an approximation
		info.gameObject = other.gameObject;

		// Use closest point on the collider as an approximation
		Vector3 closestPoint = other.ClosestPoint(transform.position);
		info.hitPoint = closestPoint;

		// Calculate an approximate direction and normal
		Vector3 toCollider = closestPoint - transform.position;
		if (toCollider.sqrMagnitude > 0.001f)
		{
			info.hitDirection = toCollider.normalized;
			info.hitNormal = -info.hitDirection; // Approximate
		}
		else
		{
			info.hitDirection = transform.forward;
			info.hitNormal = -transform.forward;
		}

		// No velocity data for triggers
		info.collisionVelocity = Vector3.zero;

		return info;
	}
}