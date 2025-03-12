using UnityEngine;

/// <summary>
/// Defines the general category of an item
/// </summary>
public enum ItemType
{
	Equipment,    // Wearable or usable gear that provides passive benefits
	Weapon,       // Items used to deal damage
	Consumable,   // Single-use items with immediate effects
	Resource,     // Crafting or currency items
	Quest         // Items related to quests or story progression
}

/// <summary>
/// Defines how an item is used when activated
/// </summary>
public enum UsageType
{
	Single,   // One use per activation
	Auto,     // Continuous automatic use while activated
	Burst,    // Multiple uses in quick succession per activation
	Stream    // Continuous stream effect while activated
}

/// <summary>
/// Scriptable Object that defines the properties and behavior of an item.
/// Used as a template for creating item instances in the game.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
	[Header("Basic Information")]
	[Tooltip("Unique identifier for this item")]
	public string id;

	[Tooltip("Display name shown to the player")]
	public string displayName;

	[TextArea(3, 5)]
	[Tooltip("Description shown in inventory and tooltips")]
	public string description;

	[Tooltip("Icon displayed in inventory and UI")]
	public Sprite icon;

	[Tooltip("The general category this item belongs to")]
	public ItemType itemType;

	[Header("Inventory Properties")]
	[Tooltip("Whether multiple copies can be stacked in inventory")]
	public bool allowMultiple = false;

	[Tooltip("Maximum stack size if multiple copies are allowed")]
	public int maxStackSize = 1;

	[Tooltip("Whether this item can be equipped by the player")]
	public bool equipable = false;

	[Tooltip("How this item is used when activated")]
	public UsageType usageType = UsageType.Single;

	[Header("Animation")]
	[Tooltip("Trigger name for usage animation")]
	public string animTriggerName;

	[Tooltip("Trigger name for equip animation")]
	public string animEquipName;

	[Tooltip("Weights for animation rig layers when item is equipped")]
	public float[] rigLayerWeight;

	[Header("Prefabs")]
	[Tooltip("Prefab instantiated when item is equipped")]
	public GameObject itemPrefab;

	[Tooltip("Prefab instantiated when item is dropped in the world")]
	public GameObject pickupPrefab;

	// Validation to ensure critical fields are set
	private void OnValidate()
	{
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogWarning($"Item {name} is missing an ID!");
		}

		if (itemPrefab == null && equipable)
		{
			Debug.LogWarning($"Equipable item {name} has no item prefab assigned!");
		}
	}
}