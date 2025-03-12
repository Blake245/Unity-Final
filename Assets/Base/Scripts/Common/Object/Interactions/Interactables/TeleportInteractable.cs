using UnityEngine;

/// <summary>
/// Handles scene transitions when a player interacts with this object.
/// Implements the IInteractable interface to work with the interaction system.
/// </summary>
public class TeleportInteractable : MonoBehaviour, IInteractable
{
	/// <summary>
	/// The name of the scene to load when this transition is activated.
	/// Must match a scene name in the build settings.
	/// </summary>
	[SerializeField]
	[Tooltip("The name of the scene to load. Must match exactly with a scene in your build settings.")]
	string transitionSceneName;

	/// <summary>
	/// Event channel used to request scene loading.
	/// This event will be raised with the target scene name when interaction occurs.
	/// </summary>
	[SerializeField]
	[Tooltip("Event that will be raised to trigger scene loading.")]
	StringEvent onLoadSceneEvent;

	/// <summary>
	/// Determines if the provided GameObject can interact with this transition.
	/// Only allows interaction from objects tagged as "Player".
	/// </summary>
	/// <param name="interactor">The GameObject attempting to interact</param>
	/// <returns>True if the interactor is tagged as "Player", false otherwise</returns>
	public bool CanInteract(InteractorInfo interactor)
	{
		return interactor.gameObject.CompareTag("Player");
	}

	/// <summary>
	/// Called when interaction with this transition begins.
	/// Triggers scene loading via the event system if a valid scene name is set.
	/// </summary>
	/// <param name="interactor">The GameObject that initiated the interaction</param>
	public void OnInteractStart(InteractorInfo interactor)
	{
		// Only attempt to load a scene if the scene name is valid
		if (!string.IsNullOrEmpty(transitionSceneName))
		{
			// Raise the load scene event with the target scene name
			onLoadSceneEvent.RaiseEvent(transitionSceneName);
		}
	}
}