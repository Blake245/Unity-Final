using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles screen fade transitions using a UI Image component.
/// Can fade in/out with customizable colors, duration and easing.
/// </summary>
public class ScreenFade : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Image image;    // UI Image used for the fade effect

	[Header("Fade Settings")]
	[SerializeField] private float time = 1f;    // Duration of the fade transition
	[SerializeField] private Color startColor = Color.black;    // Starting fade color (usually fully opaque)
	[SerializeField] private Color endColor = new Color(0, 0, 0, 0);    // End fade color (usually fully transparent) 
	[SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);    // Easing curve for the transition
	[SerializeField] private bool startOnAwake = true;    // Whether to automatically fade in when enabled

	/// <summary>
	/// Current state of the fade transition
	/// </summary>
	public enum FadeState { Idle, FadingIn, FadingOut }
	public FadeState CurrentState { get; private set; } = FadeState.Idle;

	/// <summary>
	/// Whether the current fade transition has completed
	/// </summary>
	public bool isDone { get; private set; } = false;

	// Reference to the active fade coroutine
	private Coroutine currentFade;

	/// <summary>
	/// Validate required references on startup
	/// </summary>
	private void Awake()
	{
		if (image == null)
		{
			Debug.LogError("Image reference not set in ScreenFade!");
			enabled = false;
			return;
		}
	}

	/// <summary>
	/// Start initial fade if configured
	/// </summary>
	private void Start()
	{
		if (startOnAwake)
		{
			FadeIn();
		}
	}

	/// <summary>
	/// Debug controls in editor only
	/// </summary>
	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.UpArrow)) FadeIn();
		if (Input.GetKeyDown(KeyCode.DownArrow)) FadeOut();
#endif
	}

	/// <summary>
	/// Begin fade from opaque to transparent
	/// </summary>
	public void FadeIn()
	{
		if (currentFade != null)
			StopCoroutine(currentFade);
		currentFade = StartCoroutine(FadeRoutine(startColor, endColor, time));
	}

	/// <summary>
	/// Begin fade from transparent to opaque
	/// </summary>
	public void FadeOut()
	{
		if (currentFade != null)
			StopCoroutine(currentFade);
		currentFade = StartCoroutine(FadeRoutine(endColor, startColor, time));
	}

	/// <summary>
	/// Coroutine that handles the fade transition
	/// </summary>
	/// <param name="color1">Starting color</param>
	/// <param name="color2">Target color</param>
	/// <param name="duration">Fade duration in seconds</param>
	private IEnumerator FadeRoutine(Color color1, Color color2, float duration)
	{
		isDone = false;
		CurrentState = color1 == startColor ? FadeState.FadingIn : FadeState.FadingOut;

		float timer = 0;

		while (timer < duration)
		{
			timer += Time.deltaTime;
			float progress = timer / duration;
			float curvedProgress = fadeCurve.Evaluate(progress);    // Apply easing curve

			image.color = Color.Lerp(color1, color2, curvedProgress);

			yield return null;
		}

		image.color = color2;    // Ensure we reach the target color exactly
		isDone = true;
		CurrentState = FadeState.Idle;
		currentFade = null;
	}

	/// <summary>
	/// Immediately set the fade state without transition
	/// </summary>
	/// <param name="fadeIn">If true, sets to end color (transparent). If false, sets to start color (opaque)</param>
	public void SetImmediate(bool fadeIn)
	{
		if (currentFade != null)
			StopCoroutine(currentFade);
		image.color = fadeIn ? endColor : startColor;
		isDone = true;
		CurrentState = FadeState.Idle;
		currentFade = null;
	}
}