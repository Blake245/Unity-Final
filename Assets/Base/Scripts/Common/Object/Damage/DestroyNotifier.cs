using UnityEngine;
using UnityEngine.Events;

public class DestroyNotifier : MonoBehaviour
{
	public UnityAction<GameObject> OnDestroyed;

	private void OnDestroy()
	{
		OnDestroyed?.Invoke(gameObject);
	}
}
