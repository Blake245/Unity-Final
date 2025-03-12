using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IntEvent - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting integer values to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/Int Event")]
public class IntEvent : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction<int> OnEventRaised;

	/// <summary>
	/// Raises the event with the specified integer value.
	/// </summary>
	/// <param name="value">The integer value to pass to subscribers.</param>
	public void RaiseEvent(int value)
	{
		OnEventRaised?.Invoke(value);
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction<int> listener)
	{
		OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction<int> listener)
	{
		OnEventRaised -= listener;
	}
}