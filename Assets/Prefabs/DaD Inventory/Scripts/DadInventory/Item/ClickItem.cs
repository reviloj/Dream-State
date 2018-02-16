using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// This item will operate double click on it.
/// </summary>
public class ClickItem : MonoBehaviour, IPointerClickHandler
{
    public enum Type
    {
        consumable,
        weapon,
        scroll
    }
    public Type type;
    public int value;
    public int uses;

    /// <summary>
    /// Raises the pointer click event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        DadCell cell = this.GetComponentInParent<DadCell>();
        if (cell.loc == DadCell.Location.crate)
        {
            DadCell[] cells = FindObjectsOfType<DadCell>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].GetDadItem() == null && cells[i].loc == DadCell.Location.backpack)
                {
                    if(cell.GetComponentInChildren<ClickItem>().type == Type.scroll)
                    {
                        int win = 0;
                        DadCell[] cellsI = FindObjectOfType<DummyInventoryControl>().inventory.GetComponentsInChildren<DadCell>();
                        for (int j = 0; j < cellsI.Length; j++)
                        {
                            if (cellsI[j].GetItem() != null && cellsI[j].GetComponentInChildren<ClickItem>().type == ClickItem.Type.scroll)
                            {
                                win++;
                            }
                        }
                        if (win == 8)
                        {
                            TimeController.butterflies = 9;
                            SceneManager.LoadScene("DeathScene");
                        }
                    }
                    cells[i].AddItem(cell.GetItem());
                    cell.RemoveItem();
                    cell.UpdateBackgroundState();
                    break;
                }
            }
        }
        else if (cell.loc == DadCell.Location.backpack && this.type == Type.consumable)
        {
            DadItem.eat(cell, this);
        }
        else if (cell.loc == DadCell.Location.backpack && this.type == Type.weapon)
        {
            DadCell[] cells = FindObjectsOfType<DadCell>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].GetDadItem() == null && cells[i].loc == DadCell.Location.body)
                {
                    cells[i].AddItem(cell.GetItem());
                    cell.RemoveItem();
                    cell.UpdateBackgroundState();
                    break;
                }
            }
        }
    }
}
