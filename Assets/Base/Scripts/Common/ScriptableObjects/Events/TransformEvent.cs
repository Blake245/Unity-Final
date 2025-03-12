using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TransformEvent - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting Transform references to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/Transform Event")]
public class TransformEvent : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction<Transform> OnEventRaised;

	/// <summary>
	/// Raises the event with the specified Transform reference.
	/// </summary>
	/// <param name="value">The Transform reference to pass to subscribers.</param>
	public void RaiseEvent(Transform value)
	{
		OnEventRaised?.Invoke(value);
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction<Transform> listener)
	{
		OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction<Transform> listener)
	{
		OnEventRaised -= listener;
	}
}