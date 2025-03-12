using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for objects that can participate in collision events.
/// Derived classes can override specific collision behaviors as needed.
/// </summary>
public abstract class CollisionEventReceiver : MonoBehaviour
{
	// Private backing fields for the events
	private UnityAction<CollisionInfo> onCollisionEnterEvent;
	private UnityAction<CollisionInfo> onCollisionStayEvent;
	private UnityAction<CollisionInfo> onCollisionExitEvent;

	/// <summary>
	/// Event triggered when this object first makes contact with another collider.
	/// Subscribe to this event to handle the initial frame of collision.
	/// </summary>
	public virtual UnityAction<CollisionInfo> OnCollisionEnterEvent
	{
		get => onCollisionEnterEvent;
		set => onCollisionEnterEvent = value;
	}

	/// <summary>
	/// Event triggered on each frame while this object remains in contact with another collider.
	/// Subscribe to this event to handle ongoing collision interactions.
	/// </summary>
	public virtual UnityAction<CollisionInfo> OnCollisionStayEvent
	{
		get => onCollisionStayEvent;
		set => onCollisionStayEvent = value;
	}

	/// <summary>
	/// Event triggered when this object stops making contact with another collider.
	/// Subscribe to this event to handle cleanup or effects when objects separate.
	/// </summary>
	public virtual UnityAction<CollisionInfo> OnCollisionExitEvent
	{
		get => onCollisionExitEvent;
		set => onCollisionExitEvent = value;
	}
}