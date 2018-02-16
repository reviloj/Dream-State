using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dummy example of health bar realization.
/// </summary>
public class DummyHealthBar : MonoBehaviour
{
	[Tooltip("UI image of health bar")]
	public Image healthBarImage;										// UI image of health bar
	[Tooltip("Health bar's text amount")]
	public Text healthBarText;											// Health bar's text amount
	[Tooltip("Audio source for SFX")]
	public AudioSource audioSource;										// Audio source for SFX
	[Tooltip("Heal SFX")]
	public AudioClip healSound;											// Heal SFX

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(healthBarImage && healthBarText, "Wrong settings");
		SetHealth(GetHealth());
	}

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            AddHealth(20);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ReduceHealth(20);
        }*/
        if(GetHealth() <= 0)
        {
            GameObject.FindObjectOfType<TimeController>().freeze(false, false);
        }
    }

    /// <summary>
    /// Updates the health bar image.
    /// </summary>
    /// <param name="fillAmount">Fill amount.</param>
    private void UpdateHealthBarImage(float fillAmount)
	{
		healthBarImage.fillAmount = fillAmount;
	}

	/// <summary>
	/// Gets the health amount.
	/// </summary>
	/// <returns>The health.</returns>
	public float GetHealth()
	{
		float health;
		float.TryParse(healthBarText.text, out health);
		return health;
	}

	/// <summary>
	/// Gets the max health amount.
	/// </summary>
	/// <returns>The max health.</returns>
	public float GetMaxHealth()
	{
		return 100;
	}

	/// <summary>
	/// Sets the health amounth.
	/// </summary>
	/// <param name="health">Health.</param>
	public void SetHealth(float health)
	{
		float maxHealth = GetMaxHealth();
		float res = Mathf.Min(health, maxHealth);
		res = Mathf.Max(res, 0);
		healthBarText.text = ((int)res).ToString();
		UpdateHealthBarImage((float)res / maxHealth);
	}

	/// <summary>
	/// Sets the max health amounth.
	/// </summary>
	/// <param name="maxHealth">Max health.</param>
	public void SetMaxHealth(float maxHealth)
	{
		float health = Mathf.Min(GetHealth(), maxHealth);
		SetHealth(health);
	}

	/// <summary>
	/// Adds health.
	/// </summary>
	/// <param name="health">Health.</param>
	public void AddHealth(float health)
	{
		SetHealth(GetHealth() + health);
	}

	/// <summary>
	/// Reduces health.
	/// </summary>
	/// <param name="health">Health.</param>
	public void ReduceHealth(float health)
	{
		SetHealth(GetHealth() - health);
	}

	/// <summary>
	/// Raises the item click event.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnItemClick(GameObject item)
	{
		if (item != null)
		{
			// Heal on potion use
			DummyHealthPotion healthPotion = item.GetComponent<DummyHealthPotion>();
			if (healthPotion != null)
			{
				if (GetHealth() < GetMaxHealth())
				{
					AddHealth(healthPotion.healAmount);
					if (audioSource != null && healSound != null)
					{
						audioSource.PlayOneShot(healSound);
					}
					StackItem stackItem = item.GetComponent<StackItem>();
					if (stackItem != null)
					{
						// Reduce potion's stack
						stackItem.ReduceStack(1);
					}
				}
			}
		}
	}
}
