using UnityEngine;

/// <summary>
/// Base class for all equippable and usable items in the game.
/// Provides common functionality and defines the interface that all items must implement.
/// </summary>
public abstract class Item : MonoBehaviour
{
	/// <summary>
	/// Whether this item is currently equipped by the player.
	/// </summary>
	public virtual bool IsEquipped { get; protected set; } = false;

	/// <summary>
	/// Equips this item, making it visible and usable.
	/// </summary>
	public virtual void Equip()
	{
		if (IsEquipped) return;

		gameObject.SetActive(true);
		IsEquipped = true;
	}

	/// <summary>
	/// Unequips this item, hiding it and preventing its use.
	/// </summary>
	public virtual void Unequip()
	{
		if (!IsEquipped) return;

		gameObject.SetActive(false);
		IsEquipped = false;
	}

	/// <summary>
	/// Returns the data associated with this item.
	/// </summary>
	/// <returns>The item's data object</returns>
	public abstract ItemData GetData();

	/// <summary>
	/// Checks if the item is ready to be used.
	/// </summary>
	/// <returns>True if the item can be used, false otherwise</returns>
	public abstract bool IsReady();

	/// <summary>
	/// Begins using the item.
	/// </summary>
	public abstract void Use();

	/// <summary>
	/// Stops using the item.
	/// </summary>
	public abstract void StopUse();

	/// <summary>
	/// Called by animation events during item use sequences.
	/// </summary>
	public abstract void OnAnimEventItemUse();
}