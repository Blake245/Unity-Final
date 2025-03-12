using UnityEngine;

/// <summary>
/// Handles physics-based interactions with objects implementing the IInteractable interface.
/// Detects both trigger and collision events and forwards them to appropriate interaction methods.
/// </summary>
public class CollisionInteractor : Interactor
{
	/// <summary>
	/// The type of this interactor, used when creating InteractorInfo.
	/// </summary>
	[SerializeField]
	[Tooltip("Specifies what kind of entity this interactor represents.")]
	protected InteractorInfo.InteractorType interactorType = InteractorInfo.InteractorType.Object;

	/// <summary>
	/// Called when this object's trigger collider begins overlapping with another collider.
	/// Initiates interaction with compatible objects.
	/// </summary>
	/// <param name="other">The collider that entered this object's trigger</param>
	public void OnTriggerEnter(Collider other)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(other.gameObject))
		{
			// Create interaction info
			var interactInfo = CreateTriggerInteractorInfo(other);

			// Try to get an IInteractable component and check if it can be interacted with
			if (other.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction start method
				interactable.OnInteractStart(interactInfo);
			}
		}
	}

	/// <summary>
	/// Called every frame while this object's trigger collider overlaps with another collider.
	/// Maintains continuous interaction with compatible objects.
	/// </summary>
	/// <param name="other">The collider that is staying in this object's trigger</param>
	public void OnTriggerStay(Collider other)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(other.gameObject))
		{
			// Create interaction info
			var interactInfo = CreateTriggerInteractorInfo(other);

			// Try to get an IInteractable component and check if it can be interacted with
			if (other.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction active method
				interactable.OnInteractActive(interactInfo);
			}
		}
	}

	/// <summary>
	/// Called when this object's trigger collider stops overlapping with another collider.
	/// Ends interaction with compatible objects.
	/// </summary>
	/// <param name="other">The collider that exited this object's trigger</param>
	public void OnTriggerExit(Collider other)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(other.gameObject))
		{
			// Create interaction info
			var interactInfo = CreateTriggerInteractorInfo(other);

			// Try to get an IInteractable component and check if it can be interacted with
			if (other.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction end method
				interactable.OnInteractEnd(interactInfo);
			}
		}
	}

	/// <summary>
	/// Called when this object's collider first makes physical contact with another collider.
	/// Initiates interaction with compatible objects.
	/// </summary>
	/// <param name="collision">Information about the collision event</param>
	public void OnCollisionEnter(Collision collision)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(collision.gameObject))
		{
			// Create interaction info with collision data
			var interactInfo = CreateCollisionInteractorInfo(collision);

			// Try to get an IInteractable component and check if it can be interacted with
			if (collision.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction start method
				interactable.OnInteractStart(interactInfo);
			}
		}
	}

	/// <summary>
	/// Called every frame while this object's collider remains in contact with another collider.
	/// Maintains continuous interaction with compatible objects.
	/// </summary>
	/// <param name="collision">Information about the ongoing collision</param>
	public void OnCollisionStay(Collision collision)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(collision.gameObject))
		{
			// Create interaction info with collision data
			var interactInfo = CreateCollisionInteractorInfo(collision);

			// Try to get an IInteractable component and check if it can be interacted with
			if (collision.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction active method
				interactable.OnInteractActive(interactInfo);
			}
		}
	}

	/// <summary>
	/// Called when this object's collider stops making physical contact with another collider.
	/// Ends interaction with compatible objects.
	/// </summary>
	/// <param name="collision">Information about the collision that ended</param>
	public void OnCollisionExit(Collision collision)
	{
		// Check if the object passes tag and layer filtering requirements
		if (IsValidInteraction(collision.gameObject))
		{
			// Create interaction info with collision data
			var interactInfo = CreateCollisionInteractorInfo(collision);

			// Try to get an IInteractable component and check if it can be interacted with
			if (collision.gameObject.TryGetComponent(out IInteractable interactable) &&
				interactable.CanInteract(interactInfo))
			{
				// Call the interaction end method
				interactable.OnInteractEnd(interactInfo);
			}
		}
	}

	/// <summary>
	/// Creates an InteractorInfo structure with basic information about this interactor.
	/// </summary>
	/// <returns>A filled InteractorInfo structure</returns>
	protected virtual InteractorInfo CreateBaseInteractorInfo()
	{
		return new InteractorInfo
		{
			gameObject = this.gameObject,
			type = interactorType
		};
	}

	/// <summary>
	/// Creates an InteractorInfo structure with collision information.
	/// </summary>
	/// <param name="collision">Collision data to include in the info</param>
	/// <returns>A filled InteractorInfo structure with collision data</returns>
	protected virtual InteractorInfo CreateCollisionInteractorInfo(Collision collision)
	{
		var info = CreateBaseInteractorInfo();
		/*
		// Check if we have any contact points
		if (collision.contactCount > 0)
		{
			var contactPoint = collision.contacts[0];
			info.hitPoint = contactPoint.point;
			info.hitNormal = contactPoint.normal;
			info.hitDirection = -contactPoint.normal; // Direction is opposite of normal
		}
		else
		{
			// Fallback values if no contact points are available
			info.hitPoint = collision.transform.position;
			info.hitNormal = Vector3.up;
			info.hitDirection = Vector3.down;
			Debug.LogWarning("Collision occurred with no contact points");
		}

		info.collisionVelocity = collision.relativeVelocity;
		*/
		return info;
	}

	/// <summary>
	/// Creates an InteractorInfo structure with trigger information.
	/// </summary>
	/// <param name="other">Collider that triggered the interaction</param>
	/// <returns>A filled InteractorInfo structure with trigger data</returns>
	protected virtual InteractorInfo CreateTriggerInteractorInfo(Collider other)
	{
		var info = CreateBaseInteractorInfo();

		// For triggers, we don't have precise collision info,
		// so we estimate using the closest points between colliders
		Vector3 thisPoint = Vector3.zero;
		Vector3 otherPoint = Vector3.zero;

		Collider thisCollider = GetComponent<Collider>();
		if (thisCollider != null)
		{
			thisPoint = thisCollider.ClosestPoint(other.transform.position);
			otherPoint = other.ClosestPoint(transform.position);
		}
		else
		{
			// Fallback if no collider is found
			thisPoint = transform.position;
			otherPoint = other.transform.position;
		}

		//info.hitPoint = otherPoint;
		//info.hitDirection = (otherPoint - thisPoint).normalized;

		return info;
	}
}