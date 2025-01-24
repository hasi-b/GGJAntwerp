using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add The Name of your object pool you want to create to this enum afterwards you can call the PoolManager to create new object pools from the desired objects
/// </summary>
public enum PoolNames
{

}

public class ObjectPoolManager
{
	#region Fields
	private Transform _parent;
	private Dictionary<PoolNames, object> _pools = new Dictionary<PoolNames, object>();

	private static ObjectPoolManager _instance;
	#endregion

	#region Properties
	public static ObjectPoolManager Instance
	{
		get
		{
			// compound assigning, does a check if the object exists, if it doesn't then make one
			_instance ??= new ObjectPoolManager();
			return _instance;
		}
	}
	#endregion

	#region Constructor
	public ObjectPoolManager()
	{
		CreateManagerParent();
	}
	#endregion

	#region Methods
	#region Public
	#region Generic Component
	public static ObjectPool<T> CreateObjectPool<T>(PoolNames type, T original, bool canGrow = true, int initialSize = 2)
		where T : Component
	{
		return Instance.CreateObjectPoolInternal(type, original, canGrow, initialSize);
	}
	public static bool TryGetPool<T>(PoolNames type, out ObjectPool<T> pool)
		where T : Component
	{
		return Instance.TryGetPoolInternal(type, out pool);
	}
	public static bool TryGetObject<T>(PoolNames type, out T poolableObject)
		where T : Component
	{
		return Instance.TryGetFromPoolInternal(type, out poolableObject);
	}
	public static bool TryReturnObject<T>(PoolNames type, T poolableObject)
		where T : Component
	{
		return Instance.TryReturnToPoolInternal(type, poolableObject);
	}
	#endregion

	#region GameObject Pool
	public static ObjectPool CreateObjectPool(PoolNames type, GameObject original, bool canGrow = true, int initialSize = 2)
	{
		return Instance.CreateObjectPoolInternal(type, original, canGrow, initialSize);
	}
	public static bool TryGetPool(PoolNames type, out ObjectPool pool)
	{
		return Instance.TryGetPoolInternal(type, out pool);
	}
	public static bool TryGetObject(PoolNames type, out GameObject poolableObject)
	{
		return Instance.TryGetFromPoolInternal(type, out poolableObject);
	}
	public static bool TryReturnObject(PoolNames type, GameObject poolableObject)
	{
		return Instance.TryReturnToPoolInternal(type, poolableObject);
	}
	#endregion
	#endregion

	#region Private
	#region Generic Component
	private ObjectPool<T> CreateObjectPoolInternal<T>(PoolNames type, T original, bool canGrow, int initialSize)
		where T : Component
	{
		// check before making the pool if it already exists, if it does then return that one
		if (TryGetPoolInternal(type, out ObjectPool<T> preMadePool))
			return preMadePool;

		var pool = new ObjectPool<T>(original.gameObject, CreatePoolParent(type), canGrow, initialSize);
		_pools.Add(type, pool);
		return pool;
	}
	private bool TryGetPoolInternal<T>(PoolNames type, out ObjectPool<T> pool)
		where T : Component
	{
		if (_pools.TryGetValue(type, out object objectPool))
		{
			pool = (ObjectPool<T>)objectPool;
			return true;
		}

		pool = null;
		return false;
	}
	private bool TryGetFromPoolInternal<T>(PoolNames type, out T poolableObject)
		where T : Component
	{
		if (TryGetPoolInternal(type, out ObjectPool<T> objectPool))
		{
			return objectPool.TryGet(out poolableObject);
		}

		poolableObject = null;
		return false;
	}
	private bool TryReturnToPoolInternal<T>(PoolNames type, T poolableObject)
		where T : Component
	{
		if (TryGetPoolInternal(type, out ObjectPool<T> objectPool))
		{
			return objectPool.TryReturn(poolableObject);
		}

		return false;
	}
	#endregion

	#region GameObjectPool
	private ObjectPool CreateObjectPoolInternal(PoolNames type, GameObject original, bool canGrow, int initialSize)
	{
		// check before making the pool if it already exists, if it does then return that one
		if (TryGetPoolInternal(type, out ObjectPool preMadePool))
			return preMadePool;

		var pool = new ObjectPool(original, CreatePoolParent(type), canGrow, initialSize);
		_pools.Add(type, pool);
		return pool;
	}
	private bool TryGetPoolInternal(PoolNames type, out ObjectPool pool)
	{
		if (_pools.TryGetValue(type, out object objectPool))
		{
			pool = (ObjectPool)objectPool;
			return true;
		}

		pool = null;
		return false;
	}
	private bool TryGetFromPoolInternal(PoolNames type, out GameObject poolableObject)
	{
		if (TryGetPoolInternal(type, out ObjectPool objectPool))
		{
			return objectPool.TryGet(out poolableObject);
		}

		poolableObject = null;
		return false;
	}
	private bool TryReturnToPoolInternal(PoolNames type, GameObject poolableObject)
	{
		if (TryGetPoolInternal(type, out ObjectPool objectPool))
		{
			return objectPool.TryReturn(poolableObject);
		}

		return false;
	}
	#endregion

	// parent creation
	private void CreateManagerParent()
	{
		_parent = new GameObject("PoolManager").transform;
	}
	private Transform CreatePoolParent(PoolNames type)
	{
		var poolParent = new GameObject("Pool_" + type.ToString());

		poolParent.transform.parent = _parent;
		return poolParent.transform;
	}

	#endregion
	#endregion
}


