using UnityEngine;

/// <summary>
/// Interface for objects that can be interacted with.
/// Implements methods for handling different stages of interaction.
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Determines if this object can be interacted with by the specified interactor.
	/// Default implementation always allows interaction.
	/// </summary>
	/// <param name="info">Information about the interacting entity</param>
	/// <returns>True if interaction is allowed, false otherwise</returns>
	bool CanInteract(InteractorInfo info) => true;

	/// <summary>
	/// Called when interaction begins.
	/// Default implementation logs the start of interaction.
	/// </summary>
	/// <param name="info">Information about the interacting entity</param>
	void OnInteractStart(InteractorInfo info) => Debug.Log($"Interaction started with {info.gameObject.name}");

	/// <summary>
	/// Called while interaction is ongoing.
	/// Default implementation does nothing.
	/// </summary>
	/// <param name="info">Information about the interacting entity</param>
	void OnInteractActive(InteractorInfo info) { }

	/// <summary>
	/// Called when interaction ends.
	/// Default implementation logs the end of interaction.
	/// </summary>
	/// <param name="info">Information about the interacting entity</param>
	void OnInteractEnd(InteractorInfo info) => Debug.Log($"Interaction ended with {info.gameObject.name}");
}