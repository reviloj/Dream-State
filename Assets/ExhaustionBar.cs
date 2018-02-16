using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dummy example of Exhaustion bar realization.
/// </summary>
public class ExhaustionBar : MonoBehaviour
{
    [Tooltip("UI image of Exhaustion bar")]
    public Image ExhaustionBarImage;                                        // UI image of Exhaustion bar
    [Tooltip("Exhaustion bar's text amount")]
    public Text ExhaustionBarText;                                          // Exhaustion bar's text amount
    [Tooltip("Audio source for SFX")]
    public AudioSource audioSource;                                     // Audio source for SFX
    [Tooltip("Heal SFX")]
    public AudioClip healSound;                                         // Heal SFX

    private float exhaustion;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        Debug.Assert(ExhaustionBarImage && ExhaustionBarText, "Wrong settings");
        exhaustion = 100;
        SetExhaustion(GetExhaustion());
        StartCoroutine(Deteriorate());
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            AddExhaustion(20);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            ReduceExhaustion(20);
        }*/
    }

    void DecreaseSpeed()
    {

        Movement move = GameObject.FindObjectOfType<Movement>();
        move.speed = (float)5 * GetExhaustion() / 100 + 5;
    }
  
    private IEnumerator Deteriorate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (GetExhaustion() < 100 && !GameObject.FindObjectOfType<Movement>().getAnim().GetBool("Shift"))
                AddExhaustion(3 * ((float)GameObject.FindObjectOfType<HungerBar>().GetHunger() / 100 + 0.6f));
        }
    }

    /// <summary>
    /// Updates the Exhaustion bar image.
    /// </summary>
    /// <param name="fillAmount">Fill amount.</param>
    private void UpdateExhaustionBarImage(float fillAmount)
    {
        ExhaustionBarImage.fillAmount = fillAmount;
    }

    /// <summary>
    /// Gets the Exhaustion amount.
    /// </summary>
    /// <returns>The Exhaustion.</returns>
    public float GetExhaustion()
    {
        return exhaustion;
    }

    /// <summary>
    /// Gets the max Exhaustion amount.
    /// </summary>
    /// <returns>The max Exhaustion.</returns>
    public float GetMaxExhaustion()
    {
        return 100;
    }

    /// <summary>
    /// Sets the Exhaustion amounth.
    /// </summary>
    /// <param name="Exhaustion">Exhaustion.</param>
    public void SetExhaustion(float Exhaustion)
    {
        float maxExhaustion = GetMaxExhaustion();
        float res = Mathf.Min(Exhaustion, maxExhaustion);
        res = Mathf.Max(res, 0);
        ExhaustionBarText.text = (Mathf.Round(res)).ToString();
        UpdateExhaustionBarImage((float)res / maxExhaustion);
        if (GetExhaustion() < 50)
            DecreaseSpeed();
    }

    /// <summary>
    /// Sets the max Exhaustion amounth.
    /// </summary>
    /// <param name="maxExhaustion">Max Exhaustion.</param>
    public void SetMaxExhaustion(float maxExhaustion)
    {
        float Exhaustion = Mathf.Min(GetExhaustion(), maxExhaustion);
        SetExhaustion(Exhaustion);
    }

    /// <summary>
    /// Adds Exhaustion.
    /// </summary>
    /// <param name="Exhaustion">Exhaustion.</param>
    public void AddExhaustion(float Exhaustion)
    {
        exhaustion += Exhaustion;
        SetExhaustion(exhaustion);
    }

    /// <summary>
    /// Reduces Exhaustion.
    /// </summary>
    /// <param name="Exhaustion">Exhaustion.</param>
    public void ReduceExhaustion(float Exhaustion)
    {
        exhaustion -= Exhaustion;
        if (exhaustion < 0)
            exhaustion = 0;
        SetExhaustion(exhaustion);
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
            /*DummyExhaustionPotion ExhaustionPotion = item.GetComponent<DummyExhaustionPotion>();
            if (ExhaustionPotion != null)
            {
                if (GetExhaustion() < GetMaxExhaustion())
                {
                    AddExhaustion(ExhaustionPotion.healAmount);
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
