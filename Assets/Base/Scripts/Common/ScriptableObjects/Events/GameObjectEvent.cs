using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// GameObjectEvent - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting GameObject references to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/GameObject Event")]
public class GameObjectEvent : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction<GameObject> OnEventRaised;

	/// <summary>
	/// Raises the event with the specified GameObject reference.
	/// </summary>
	/// <param name="value">The GameObject reference to pass to subscribers.</param>
	public void RaiseEvent(GameObject value)
	{
		OnEventRaised?.Invoke(value);
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction<GameObject> listener)
	{
		OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction<GameObject> listener)
	{
		OnEventRaised -= listener;
	}
}