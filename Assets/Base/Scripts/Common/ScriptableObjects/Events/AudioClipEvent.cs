using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// AudioClipEvent - A simple observer pattern implementation using ScriptableObject.
/// Allows broadcasting AudioClip values to multiple listeners.
/// </summary>
[CreateAssetMenu(menuName = "Events/AudioClip Event")]
public class AudioClipEvent : ScriptableObjectBase
{
	/// <summary>
	/// Unity Action that holds references to all subscribed methods.
	/// Allows dynamically calling multiple functions when the event is raised.
	/// </summary>
	public UnityAction<AudioClip> OnEventRaised;

	/// <summary>
	/// Raises the event with the specified AudioClip value.
	/// </summary>
	/// <param name="value">The AudioClip value to pass to subscribers.</param>
	public void RaiseEvent(AudioClip value)
	{
		OnEventRaised?.Invoke(value);
	}

	/// <summary>
	/// Subscribes a listener to the event.
	/// </summary>
	/// <param name="listener">The method that will be called when the event is raised.</param>
	public void Subscribe(UnityAction<AudioClip> listener)
	{
		OnEventRaised += listener;
	}

	/// <summary>
	/// Unsubscribes a listener from the event.
	/// </summary>
	/// <param name="listener">The method that should no longer be called when the event is raised.</param>
	public void Unsubscribe(UnityAction<AudioClip> listener)
	{
		OnEventRaised -= listener;
	}
}