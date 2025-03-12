using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// BoolEvent - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting boolean values to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/Bool Event")]
public class BoolEvent : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction<bool> OnEventRaised;

	/// <summary>
	/// Raises the event with the specified boolean value.
	/// </summary>
	/// <param name="value">The boolean value to pass to subscribers.</param>
	public void RaiseEvent(bool value)
	{
		OnEventRaised?.Invoke(value);
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction<bool> listener)
	{
		OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction<bool> listener)
	{
		OnEventRaised -= listener;
	}
}