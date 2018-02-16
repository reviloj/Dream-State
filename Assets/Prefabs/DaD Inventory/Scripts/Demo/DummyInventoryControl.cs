using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Dummy inventory control for demo scene.
/// </summary>
public class DummyInventoryControl : MonoBehaviour
{
	[Tooltip("Inventory cells sheet")]
	public GameObject inventory;											// Inventory cells sheet
    [Tooltip("Inventory cells sheet")]
    public GameObject crateInventory;                                            // Inventory cells sheet
    [Tooltip("Inventory stack group")]
	public StackGroup inventoryStackGroup;									// Inventory stack group
    public GameObject apple;
    public GameObject meat;
    public GameObject axe;
    public GameObject fire;
    public GameObject scroll;
    public Vector3 chestPos;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
	{
        inventory.SetActive(false);
        crateInventory.SetActive(false);
    }

    public void setUpCrate(int[] items)
    {
        inventory.SetActive(true);
        crateInventory.SetActive(true);
        for (int i = 0; i < items.Length; i++)
        {
            DadCell[] cell = FindObjectsOfType<DadCell>();
            for (int j = 0; j < cell.Length; j++)
            {
                if (cell[j].GetDadItem() == null && cell[j].loc == DadCell.Location.crate)
                {
                    switch (items[i])
                    {
                        case 0:
                            cell[j].AddItem((GameObject)Instantiate(apple));
                            break;
                        case 1:
                            cell[j].AddItem((GameObject)Instantiate(axe));
                            break;
                        case 2:
                            cell[j].AddItem((GameObject)Instantiate(meat));
                            break;
                        case 3:
                            cell[j].AddItem((GameObject)Instantiate(fire));
                            break;
                        case 4:
                            cell[j].AddItem((GameObject)Instantiate(scroll));
                            break;
                    }
                    break;
                }
            }
        }
    }

    private void destroyCrate()
    {
        DadCell[] cell = FindObjectsOfType<DadCell>();
        for (int j = 0; j < cell.Length; j++)
        {
            if (cell[j].loc == DadCell.Location.crate)
            {
                cell[j].RemoveItem();
            }
        }
        crateInventory.SetActive(false);
    }

	/// <summary>
	/// Show/Hide the inventory.
	/// </summary>
	public void ToggleInventory()
	{
		if (inventory.activeSelf == false)
		{
			inventory.SetActive(true);
		}
		else
		{
			inventory.SetActive(false);
		}
        if (crateInventory.activeSelf == true)
            destroyCrate();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Dreamer").transform.position, chestPos) > 5)
        {
            if (crateInventory.activeSelf == true)
            {
                GameObject.FindObjectOfType<TimeController>().freeze();
                destroyCrate();
                inventory.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            DadCell[] cell = FindObjectsOfType<DadCell>();
            for(int i = 0; i < cell.Length; i++) {
                if (cell[i].GetDadItem() == null && cell[i].loc == DadCell.Location.backpack)
                {
                    cell[i].AddItem((GameObject)Instantiate(scroll));
                    break;
                }
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            DadCell[] cell = FindObjectsOfType<DadCell>();
            for (int i = 0; i < cell.Length; i++)
            {
                if (cell[i].GetDadItem() == null && cell[i].loc == DadCell.Location.backpack)
                {
                    cell[i].AddItem((GameObject)Instantiate(axe));
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            DadCell[] cell = FindObjectsOfType<DadCell>();
            for (int i = 0; i < cell.Length; i++)
            {
                if (cell[i].GetDadItem() == null && cell[i].loc == DadCell.Location.backpack)
                {
                    cell[i].AddItem((GameObject)Instantiate(fire));
                    break;
                }
            }
        }*/
    }
}
