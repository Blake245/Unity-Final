using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's inventory of items, handling equipping, switching, and usage.
/// </summary>
public class Inventory : MonoBehaviour
{
	[SerializeField, Tooltip("Initial items in the inventory")]
	private List<Item> items = new List<Item>();

	/// <summary>
	/// The currently equipped item that will receive use commands.
	/// </summary>
	public Item CurrentItem { get; private set; }

	/// <summary>
	/// Index of the currently equipped item in the items list.
	/// </summary>
	private int currentItemIndex = -1;

	/// <summary>
	/// Initialize the inventory and equip the first item if available.
	/// </summary>
	void Start()
	{
		if (items.Count > 0)
		{
			SwitchItem(0);
		}
	}

	/// <summary>
	/// Activates the currently equipped item.
	/// </summary>
	public void Use()
	{
		CurrentItem?.Use();
	}

	/// <summary>
	/// Stops usage of the currently equipped item.
	/// </summary>
	public void StopUse()
	{
		CurrentItem?.StopUse();
	}

	/// <summary>
	/// Switches to the item at the specified index in the inventory.
	/// </summary>
	/// <param name="index">Index of the item to equip</param>
	/// <returns>True if switch was successful, false otherwise</returns>
	public bool SwitchItem(int index)
	{
		if (index < 0 || index >= items.Count)
			return false;

		// Skip if trying to equip the already equipped item
		if (index == currentItemIndex)
			return true;

		// Unequip current item if one is equipped
		if (CurrentItem != null)
			CurrentItem.Unequip();

		// Equip new item
		CurrentItem = items[index];
		currentItemIndex = index;

		if (CurrentItem != null)
			CurrentItem.Equip();

		return true;
	}

	/// <summary>
	/// Switches to the next item in the inventory.
	/// </summary>
	public void NextItem()
	{
		int nextIndex = (currentItemIndex + 1) % items.Count;
		SwitchItem(nextIndex);
	}

	/// <summary>
	/// Switches to the previous item in the inventory.
	/// </summary>
	public void PreviousItem()
	{
		int prevIndex = (currentItemIndex - 1 + items.Count) % items.Count;
		SwitchItem(prevIndex);
	}

	/// <summary>
	/// Adds an item to the inventory.
	/// </summary>
	/// <param name="item">The item to add</param>
	/// <returns>True if added successfully</returns>
	public bool AddItem(Item item)
	{
		if (item == null)
			return false;

		items.Add(item);

		// If this is the first item, equip it automatically
		if (items.Count == 1)
			SwitchItem(0);

		return true;
	}

	/// <summary>
	/// Removes an item from the inventory.
	/// </summary>
	/// <param name="item">The item to remove</param>
	/// <returns>True if removed successfully</returns>
	public bool RemoveItem(Item item)
	{
		if (item == null)
			return false;

		int index = items.IndexOf(item);

		// If removing the currently equipped item, unequip it first
		if (item == CurrentItem)
		{
			CurrentItem.Unequip();
			CurrentItem = null;
			currentItemIndex = -1;
		}

		bool removed = items.Remove(item);

		// If we removed the current item and have other items, equip another one
		if (removed && currentItemIndex == -1 && items.Count > 0)
		{
			SwitchItem(0);
		}
		// If we removed an item before the current one, adjust the index
		else if (removed && index < currentItemIndex)
		{
			currentItemIndex--;
		}

		return removed;
	}

	/// <summary>
	/// Gets the item at the specified index.
	/// </summary>
	/// <param name="index">Index of the item to retrieve</param>
	/// <returns>The item at the specified index, or null if out of range</returns>
	public Item GetItem(int index)
	{
		if (index < 0 || index >= items.Count)
			return null;

		return items[index];
	}

	/// <summary>
	/// Gets the number of items in the inventory.
	/// </summary>
	public int ItemCount => items.Count;
}