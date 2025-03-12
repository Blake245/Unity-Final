using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting events with no parameters to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event")]
public class Event : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction OnEventRaised;

	/// <summary>
	/// Raises the event with no parameters.
	/// </summary>
	public void RaiseEvent()
	{
		OnEventRaised?.Invoke();
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction listener)
	{
		if (listener != null) OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction listener)
	{
		if (listener != null) OnEventRaised -= listener;
	}
}