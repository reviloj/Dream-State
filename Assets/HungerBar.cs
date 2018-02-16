using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dummy example of Hunger bar realization.
/// </summary>
public class HungerBar : MonoBehaviour
{
    [Tooltip("UI image of Hunger bar")]
    public Image HungerBarImage;                                        // UI image of Hunger bar
    [Tooltip("Hunger bar's text amount")]
    public Text HungerBarText;                                          // Hunger bar's text amount
    [Tooltip("Audio source for SFX")]
    public AudioSource audioSource;                                     // Audio source for SFX
    [Tooltip("Heal SFX")]
    public AudioClip healSound;                                         // Heal SFX

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        Debug.Assert(HungerBarImage && HungerBarText, "Wrong settings");
        SetHunger(GetHunger());
        StartCoroutine(Deteriorate());
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            AddHunger(20);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ReduceHunger(20);
        }*/
    }

    void DecreaseHealth()
    {
        if(GetHunger() < 20)
        {
            DummyHealthBar health = GameObject.FindObjectOfType<DummyHealthBar>();
            health.ReduceHealth((20 - GetHunger()) /10 + 1);
        }
    }

    private IEnumerator Deteriorate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (GetHunger() > 0)
            {
                ReduceHunger(1);
            }
            DecreaseHealth();
        }
    }

    /// <summary>
    /// Updates the Hunger bar image.
    /// </summary>
    /// <param name="fillAmount">Fill amount.</param>
    private void UpdateHungerBarImage(float fillAmount)
    {
        HungerBarImage.fillAmount = fillAmount;
    }

    /// <summary>
    /// Gets the Hunger amount.
    /// </summary>
    /// <returns>The Hunger.</returns>
    public int GetHunger()
    {
        int Hunger;
        int.TryParse(HungerBarText.text, out Hunger);
        return Hunger;
    }

    /// <summary>
    /// Gets the max Hunger amount.
    /// </summary>
    /// <returns>The max Hunger.</returns>
    public int GetMaxHunger()
    {
        return 100;
    }

    /// <summary>
    /// Sets the Hunger amounth.
    /// </summary>
    /// <param name="Hunger">Hunger.</param>
    public void SetHunger(int Hunger)
    {
        int maxHunger = GetMaxHunger();
        int res = Mathf.Min(Hunger, maxHunger);
        res = Mathf.Max(res, 0);
        HungerBarText.text = res.ToString();
        UpdateHungerBarImage((float)res / maxHunger);
    }

    /// <summary>
    /// Sets the max Hunger amounth.
    /// </summary>
    /// <param name="maxHunger">Max Hunger.</param>
    public void SetMaxHunger(int maxHunger)
    {
        int Hunger = Mathf.Min(GetHunger(), maxHunger);
        SetHunger(Hunger);
    }

    /// <summary>
    /// Adds Hunger.
    /// </summary>
    /// <param name="Hunger">Hunger.</param>
    public void AddHunger(int Hunger)
    {
        SetHunger(GetHunger() + Hunger);
    }

    /// <summary>
    /// Reduces Hunger.
    /// </summary>
    /// <param name="Hunger">Hunger.</param>
    public void ReduceHunger(int Hunger)
    {
        SetHunger(GetHunger() - Hunger);
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
            /*DummyHungerPotion HungerPotion = item.GetComponent<DummyHungerPotion>();
            if (HungerPotion != null)
            {
                if (GetHunger() < GetMaxHunger())
                {
                    AddHunger(HungerPotion.healAmount);
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
            }*/
        }
    }
}
