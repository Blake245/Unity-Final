using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
	[SerializeField] IntData scoreData;
	[SerializeField] FloatData healthData;

	[SerializeField] TMP_Text scoreText;
	[SerializeField] Slider healthSlider;

	void Start()
	{
		scoreText.text = scoreData.Value.ToString("0000");
		healthSlider.value = healthData.Value;
	}

	private void Update()
	{
		scoreText.text = scoreData.Value.ToString("0000");
		healthSlider.value = healthData.Value;
	}
}
