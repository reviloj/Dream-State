using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DadItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static bool dragDisabled = false;										// Drag start global disable

	[Tooltip("Item that is dragged now")]
	public static GameObject draggedItem;                                      		// Item that is dragged now
	[Tooltip("Icon of dragged item")]
	public static GameObject icon;                                                  // Icon of dragged item
	[Tooltip("From this cell dragged item is")]
	public static DadCell sourceCell;                                       		// From this cell dragged item is

	public delegate void DragEvent(GameObject item);
    public static event DragEvent OnItemDragStartEvent;                             // Drag start event
    public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 100;										// Sort order for canvas

    public static void eat(DadCell cell, ClickItem item)
    {
        HungerBar hunger = GameObject.FindObjectOfType<HungerBar>();
        hunger.AddHunger(item.value);
        cell.RemoveItem();
    }
    public static void attack(DadCell cell, ClickItem item)
    {
        Spider[] spiders = FindObjectsOfType<Spider>();
        int index = -1;
        float dist = 15;
        for (int i = 0; i < spiders.Length; i++)
        {
            Vector3 relative = GameObject.FindGameObjectWithTag("Dreamer").transform.InverseTransformPoint(spiders[i].transform.position);
            if (relative.x < 3 && relative.x > -3 && relative.z > 0 && dist > Vector3.Distance(spiders[i].transform.position, GameObject.FindGameObjectWithTag("Dreamer").transform.position))
            {
                index = i;
                dist = Vector3.Distance(spiders[i].transform.position, GameObject.FindGameObjectWithTag("Dreamer").transform.position);
            }
        }
        if (dist < 5)
        {
            spiders[index].walking = false;
            spiders[index].CancelInvoke();
            spiders[index].anim.SetTrigger("Die");
            Destroy(spiders[index].gameObject, 1f);
            item.uses--;
            item.GetComponentInChildren<EditText>().updateText(item.uses);
            if (item.uses == 0)
                cell.RemoveItem();
        }
    }
    public static void burn(DadCell cell, ClickItem item)
    {
        SpiderSpawner[] spiders = FindObjectsOfType<SpiderSpawner>();
        int index = -1;
        float dist = 100;
        for (int i = 0; i < spiders.Length; i++)
        {
            Vector3 relative = GameObject.FindGameObjectWithTag("Dreamer").transform.InverseTransformPoint(spiders[i].transform.position);
            if (relative.x < 3 && relative.x > -3 && relative.z > 0 && dist > Vector3.Distance(spiders[i].transform.position, GameObject.FindGameObjectWithTag("Dreamer").transform.position))
            {
                index = i;
                dist = Vector3.Distance(spiders[i].transform.position, GameObject.FindGameObjectWithTag("Dreamer").transform.position);
            }
        }
        if (dist < 10)
        {
            Destroy(spiders[index].gameObject, 1f);
            item.uses--;
            if (item.uses == 0)
                cell.RemoveItem();
        }
    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
		if (canvas == null)
        {
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
        }
    }

    /// <summary>
    /// This item started to drag.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
		if (dragDisabled == false)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = gameObject;                                             	// Set as dragged item
			// Create item's icon
			icon = new GameObject();
			icon.transform.SetParent(canvas.transform);
			icon.name = "Icon";
			Image myImage = GetComponent<Image>();
			myImage.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
			Image iconImage = icon.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.sprite = myImage.sprite;
			RectTransform iconRect = icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

			if (OnItemDragStartEvent != null)
	        {
				OnItemDragStartEvent(gameObject);                                	// Notify all items about drag start for raycast disabling
	        }
		}
    }

    /// <summary>
    /// Every frame on this item drag.
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (icon != null)
        {
            icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
        }
    }

    /// <summary>
    /// This item is dropped.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
		ResetConditions();
    }

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	private void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(gameObject);                                       	// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;
	}

    /// <summary>
    /// Enable item's raycast.
    /// </summary>
    /// <param name="condition"> true - enable, false - disable </param>
    public void MakeRaycast(bool condition)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = condition;
        }
    }

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DadCell GetCell()
	{
		return GetComponentInParent<DadCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}
}
