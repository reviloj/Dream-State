using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Does nothing.

/// <summary>
/// Unite stack cells in group and change it's stack logic.
/// </summary>
public class StackGroup : MonoBehaviour
{
	public static bool globalSplit;												// Global items split mode enabled

	[Tooltip("Split items when drop between two different stack groups")]
	public bool splitOuter;														// Split items when drop between two different stack groups
	[Tooltip("Arrange item placement in group's cells on drop")]
	public bool arrangeMode;													// Arrange item placement in group's cells on drop
	[Tooltip("This stack group will destroy any dropped item")]
	public bool trashBinMode;													// This stack group will destroy any dropped item
	[Tooltip("SFX for item destroying")]
	public AudioClip trashBinSound;												// SFX for item destroying
	[Tooltip("Interface for items splitting")]
	public SplitInterface splitInterface;										// Interface for items splitting
	[Tooltip("Audio source for SFX")]
	public AudioSource audioSource;												// Audio source for SFX
	[Tooltip("This game objests will be notified on stack events")]
	public List<GameObject> eventAdditionalReceivers = new List<GameObject>();	// This GOs will be notified on stack events

	private enum MyState
	{
		WaitForRequest,
		WaitForEvent,
		Busy
	}

	private MyState myState = MyState.Busy;							// State machine

	/// <summary>
	/// Toggles the global split enabling.
	/// </summary>
	public void ToggleGlobalSplit()
	{
		globalSplit = !globalSplit;
	}

	/// <summary>
	/// Adds the item.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="limit">Limit.</param>
	public int AddItem(StackItem stackItem, int limit)
	{
        print(1);
		int res = 0;
		if (stackItem != null)
		{
			// Try to distribute item inside group's items and cells
			res += DistributeAnywhere(stackItem, limit, null);
			StackGroup sourceStackGroup = stackItem.GetComponentInParent<StackGroup>();
			// Send stack event notification
			SendNotification(sourceStackGroup != null ? sourceStackGroup.gameObject : null, gameObject);
			if (res > 0)
			{
				PlaySound(stackItem.sound);
			}
		}
		return res;
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <param name="stackCell">Stack cell.</param>
	/// <param name="limit">Limit.</param>
	public void RemoveItem(StackCell stackCell, int limit)
	{
        print(2);
		if (stackCell != null)
		{
			StackItem stackItem = stackCell.GetStackItem();
			if (stackItem != null)
			{
				RemoveItem(stackItem, limit);
			}
		}
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="limit">Limit.</param>
	public void RemoveItem(StackItem stackItem, int limit)
	{
        print(3);
		if (stackItem != null)
		{
			stackItem.ReduceStack(limit);
			SendNotification(null, gameObject);
			PlaySound(trashBinSound);
		}
	}

	/// <summary>
	/// Swap items between groups.
	/// </summary>
	/// <returns><c>true</c>, if item was replaced, <c>false</c> otherwise.</returns>
	/// <param name="currentStackItem">Current stack item.</param>
	/// <param name="sourceStackItem">Source stack item.</param>
	/// <param name="sourceStackGroup">Source stack group.</param>
	public bool ReplaceItems(StackItem currentStackItem, StackItem sourceStackItem, StackGroup sourceStackGroup)
	{
        print(4);
		bool res = false;
		if (currentStackItem != null && sourceStackItem != null && sourceStackGroup != null)
		{
			StackCell currentStackCell = currentStackItem.GetStackCell();
			sourceStackGroup.DistributeAnywhere(currentStackItem, currentStackItem.GetStack(), null);
			if (currentStackCell.GetStackItem() == null)
			{
				currentStackCell.UniteStack(sourceStackItem, sourceStackItem.GetStack());
				PlaySound(sourceStackItem.sound);
				res = true;
			}
			// Send stack event notification
			SendNotification(sourceStackGroup.gameObject, gameObject);
		}
		return res;
	}

	/// <summary>
	/// Gets the allowed space for specified item.
	/// </summary>
	/// <returns>The allowed space.</returns>
	/// <param name="stackItem">Stack item.</param>
	public int GetAllowedSpace(StackItem stackItem)
	{
        print(5);
		double res = 0;
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>())
			{
				StackItem item = stackCell.GetStackItem();
				if (item != null)
				{
					if (stackCell.HasSameItem(stackItem) == true)
					{
						res += stackCell.GetAllowedSpace();
					}
				}
				else
				{
					res += stackCell.GetAllowedSpace();
				}
			}
		}
		if (res > int.MaxValue)
		{
			res = int.MaxValue;
		}
		return (int)res;
	}

	/// <summary>
	/// Gets the free stack cells.
	/// </summary>
	/// <returns>The free stack cells.</returns>
	/// <param name="stackItem">Stack item.</param>
	public List<StackCell> GetFreeStackCells(StackItem stackItem)
	{
        print(6);
		List<StackCell> res = new List<StackCell>();
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>())
			{
				if (stackCell.GetStackItem() == null)
				{
					if (SortCell.IsSortAllowed(stackCell.gameObject, stackItem.gameObject) == true)
					{
						res.Add(stackCell);
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Gets the similar stack items (with same sort).
	/// </summary>
	/// <returns>The similar stack items.</returns>
	/// <param name="stackItem">Stack item.</param>
	public List<StackItem> GetSimilarStackItems(StackItem stackItem)
	{
        print(7);
		List<StackItem> res = new List<StackItem>();
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>())
			{
				StackItem sameStackItem = stackCell.GetStackItem();
				if (sameStackItem != null)
				{
					if (SortCell.IsSortAllowed(stackCell.gameObject, stackItem.gameObject) == true)
					{
						res.Add(sameStackItem);
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Raises the DaD group event event.
	/// </summary>
	/// <param name="desc">Desc.</param>
	private void OnDadGroupEvent(DadCell.DadEventDescriptor desc)
	{
        /*print(desc.triggerType);
		switch (desc.triggerType)
		{
		case DadCell.TriggerType.DragGroupRequest:
		case DadCell.TriggerType.DropGroupRequest:
            print(8.1);
			if (myState == MyState.WaitForRequest)
			{
                    print(8.11);
				// Disable standard DaD logic
				desc.groupPermission = false;
				myState = MyState.WaitForEvent;
			}
			break;
		case DadCell.TriggerType.DragEnd:
            print(8.2);
			if (myState == MyState.WaitForEvent)
			{
                print(8.22);
				StackGroup sourceStackControl = desc.sourceCell.GetComponentInParent<StackGroup>();
				StackGroup destStackControl = desc.destinationCell.GetComponentInParent<StackGroup>();
				if (sourceStackControl != destStackControl)
				{
					// If this group is source group - do nothing
					myState = MyState.WaitForRequest;
				}
			}
			break;
		case DadCell.TriggerType.DropEnd:
            print(8.3);
			if (myState == MyState.WaitForEvent)
			{
                print(8.33);
				// If this group is destination group
				myState = MyState.Busy;
				// Operate item's drop
				StartCoroutine(EventHandler(desc));
			}
			break;
		}*/
	}

	/// <summary>
	/// Try to distribute item in similar items inside group.
	/// </summary>
	/// <returns>The in items.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeInItems(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
        print(9);
		int res = 0;

		if (stackItem != null)
		{
			if (amount > 0)
			{
				foreach (StackCell stackCell in GetComponentsInChildren<StackCell>())
				{
					if (stackCell != reservedStackCell)
					{
						if (amount > 0)
						{
							if (stackCell.HasSameItem(stackItem) == true)
							{
								int unitedPart = stackCell.UniteStack(stackItem, amount);
								res += unitedPart;
								amount -= unitedPart;
							}
						}
						else
						{
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Try to distribute item in free cells inside group.
	/// </summary>
	/// <returns>The in cells.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeInCells(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
        print(10);
		int res = 0;

		if (stackItem != null)
		{
			if (amount > 0)
			{
				foreach (StackCell emptyStackCell in GetFreeStackCells(stackItem))
				{
					if (emptyStackCell != reservedStackCell)
					{
						int unitedPart = emptyStackCell.UniteStack(stackItem, amount);
						res += unitedPart;
						amount -= unitedPart;
						if (amount <= 0)
						{
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Try to distribute between items than between free cells.
	/// </summary>
	/// <returns>The anywhere.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeAnywhere(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
        print(11);
		int res = 0;
		res += DistributeInItems(stackItem, amount, reservedStackCell);
		amount -= res;
		if (amount > 0)
		{
			res += DistributeInCells(stackItem, amount, reservedStackCell);
		}
		return res;
	}

	/// <summary>
	/// Stack event handler.
	/// </summary>
	/// <returns>The handler.</returns>
	/// <param name="desc">Desc.</param>
	private IEnumerator EventHandler(DadCell.DadEventDescriptor desc)
	{
		StackGroup sourceStackGroup = desc.sourceCell.GetComponentInParent<StackGroup>();
		StackGroup destStackGroup = desc.destinationCell.GetComponentInParent<StackGroup>();

		StackCell myStackCell = desc.destinationCell.GetComponent<StackCell>();
		StackCell theirStackCell = desc.sourceCell.GetComponent<StackCell>();
		StackItem myStackItem = myStackCell.GetStackItem();
		StackItem theirStackItem = theirStackCell.GetStackItem();

		//AudioClip itemSound = theirStackItem.sound;									// Item's SFX

		//int amount = theirStackItem.GetStack();                                     // Item's stack amount

        yield return 0;
	}

	/// <summary>
	/// Sends the stack event notification.
	/// </summary>
	/// <param name="sourceGroup">Source group.</param>
	/// <param name="destinationGroup">Destination group.</param>
	private void SendNotification(GameObject sourceGroup, GameObject destinationGroup)
	{
        print(13);
		if (sourceGroup != null)
		{
			// Send notification to source GO
			sourceGroup.SendMessage("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
		if (destinationGroup != null)
		{
			// Send notification to destination GO
			destinationGroup.SendMessage("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
		foreach (GameObject receiver in eventAdditionalReceivers)
		{
			// Send notification to additionaly specified GOs
			receiver.SendMessage("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="sound">Sound.</param>
	private void PlaySound(AudioClip sound)
	{
		if (audioSource != null && sound != null)
		{
			audioSource.PlayOneShot(sound);
		}
	}
}
