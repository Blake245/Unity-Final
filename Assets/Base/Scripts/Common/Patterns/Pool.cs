using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// MonoBehaviour-based object pool for GameObjects.
/// Objects are generated at startup to avoid runtime instantiations.
/// </summary>
public class Pool : MonoBehaviour, IPool<GameObject>
{
	[SerializeField] private GameObject prefab; // Prefab to be pooled
	[SerializeField] private int defaultCapacity = 20; // Initial pool size
	[SerializeField] private int maxSize = 100; // Maximum number of objects in the pool

	[SerializeField] private bool collectionCheck = true; // Enables Unity's internal safety checks
	[SerializeField] private bool preGenerateOnStartup = true; // Pre-generate objects at startup

	private IObjectPool<GameObject> pool;

	/// <summary>
	/// Initializes the pool and pre-generates objects at startup.
	/// </summary>
	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (pool != null) return; // Prevents duplicate initialization

		// Create Unity's ObjectPool
		pool = new ObjectPool<GameObject>(
			CreateNewObject,   // Function to create new instances
			OnGetFromPool,     // Called when retrieving an object
			OnReleaseToPool,   // Called when returning an object
			OnDestroyPooledObject, // Called when an object is destroyed
			collectionCheck,    // Enable Unity's collection safety check
			defaultCapacity,    // Initial pool size
			maxSize             // Maximum pool size
		);

		// Prefill the pool with inactive objects
		if (preGenerateOnStartup) PreGeneratePoolObjects();
	}

	/// <summary>
	/// Generates objects at startup to avoid runtime instantiations.
	/// </summary>
	private void PreGeneratePoolObjects()
	{
		for (int i = 0; i < defaultCapacity; i++)
		{
			GameObject go = CreateNewObject();
			go.transform.SetParent(transform);
			go.SetActive(false);
			pool.Release(go);
		}
	}

	/// <summary>
	/// Retrieves an object from the pool, creating a new one if necessary.
	/// Ensures the pool is initialized before use.
	/// </summary>
	/// <returns>A GameObject instance from the pool.</returns>
	public GameObject Get()
	{
		Initialize();
		return pool.Get();
	}

	/// <summary>
	/// Retrieves a specific component from a pooled GameObject.
	/// If the component doesn't exist, it is added dynamically.
	/// </summary>
	public T Get<T>() where T : Component
	{
		Initialize();
		GameObject go = pool.Get();
		go.TryGetComponent(out T component);
		Debug.Assert(component != null, $"Component {typeof(T).Name} not found on {prefab.name}.");

		return component;
	}

	/// <summary>
	/// Returns an object to the pool, making it available for reuse.
	/// </summary>
	/// <param name="obj">The GameObject to be returned to the pool.</param>
	public void Release(GameObject go)
	{
		if (pool == null) return;
		pool.Release(go);
	}

	/// <summary>
	/// Instantiates a new instance of the prefab and assigns the pool reference if applicable.
	/// </summary>
	/// <returns>A new instance of the prefab.</returns>
	private GameObject CreateNewObject()
	{
		GameObject pooledObject = Instantiate(prefab);
		pooledObject.transform.SetParent(transform); // Keep objects organized under the pool parent

		// If the object implements IPoolable, assign the pool reference
		if (pooledObject.TryGetComponent(out IPoolable<GameObject> poolable))
		{
			poolable.Pool = this;
		}

		return pooledObject;
	}

	/// <summary>
	/// Called when an object is retrieved from the pool.
	/// Reactivates the GameObject and calls OnSpawn() if applicable.
	/// </summary>
	/// <param name="pooledObject">The retrieved GameObject.</param>
	private void OnGetFromPool(GameObject pooledObject)
	{
		pooledObject.SetActive(true);

		// Notify the object that it has been spawned (if it implements IPoolable)
		if (pooledObject.TryGetComponent(out IPoolable<GameObject> poolable))
		{
			poolable.OnSpawn();
		}
	}

	/// <summary>
	/// Called when an object is returned to the pool.
	/// Deactivates the GameObject and calls OnDespawn() if applicable.
	/// </summary>
	/// <param name="pooledObject">The object being returned to the pool.</param>
	private void OnReleaseToPool(GameObject pooledObject)
	{
		pooledObject.SetActive(false);
		pooledObject.transform.SetParent(transform);

		// Notify the object that it is being despawned (if it implements IPoolable)
		if (pooledObject.TryGetComponent(out IPoolable<GameObject> poolable))
		{
			poolable.OnDespawn();
		}
	}

	/// <summary>
	/// Called when the pool exceeds its maximum size.
	/// Permanently destroys the pooled object.
	/// </summary>
	/// <param name="pooledObject">The object to be destroyed.</param>
	private void OnDestroyPooledObject(GameObject pooledObject)
	{
		Destroy(pooledObject);
	}
}
