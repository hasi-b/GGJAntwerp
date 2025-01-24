using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
	private Queue<T> _pool = new Queue<T>();
	private bool _canGrow;
	private GameObject _originalObject;

	private Transform _parent;

	public ObjectPool(GameObject original, Transform parent, bool canGrow, int initialSize)
	{
		_canGrow = canGrow;
		_originalObject = original;
		_parent = parent;

		FillPool(initialSize);
	}

	private void FillPool(int poolSize)
	{
		for (int i = 0; i < poolSize; i++)
		{
			_pool.Enqueue(CreateOriginalObject().GetComponent<T>());
		}
	}

	private GameObject CreateOriginalObject()
	{
		var prefab = GameObject.Instantiate(_originalObject, _parent);
		prefab.gameObject.SetActive(false);

		return prefab;
	}

	public bool TryGet(out T poolObj)
	{
		poolObj = null;
		if (_pool.Count == 0)
		{
			if (_canGrow)
			{
				poolObj = CreateOriginalObject().GetComponent<T>();
				poolObj.gameObject.SetActive(true);
				return true;
			}
			return false;
		}
		poolObj = _pool.Dequeue();
		poolObj.gameObject.SetActive(true);
		return true;
	}

	public bool TryReturn(T poolObj)
	{
		if (poolObj == null)
		{
			return false;
		}

		poolObj.gameObject.SetActive(false);
		_pool.Enqueue(poolObj);
		return true;
	}
}

public class ObjectPool
{
	private Queue<GameObject> _pool = new Queue<GameObject>();
	private bool _canGrow;
	private GameObject _original;

	private Transform _parent;

	public ObjectPool(GameObject original, Transform parent, bool canGrow, int initialSize)
	{
		_canGrow = canGrow;
		_original = original;
		_parent = parent;

		FillPool(initialSize);
	}

	private void FillPool(int poolSize)
	{
		for (int i = 0; i < poolSize; i++)
		{
			_pool.Enqueue(CreateOriginal());
		}
	}

	private GameObject CreateOriginal()
	{
		var prefab = GameObject.Instantiate(_original, _parent);
		prefab.SetActive(false);

		return prefab;
	}

	public bool TryGet(out GameObject poolObj)
	{
		poolObj = null;
		if (_pool.Count == 0)
		{
			if (_canGrow)
			{
				poolObj = CreateOriginal();
				poolObj.gameObject.SetActive(true);
				return true;
			}
			return false;
		}
		poolObj = _pool.Dequeue();
		poolObj.SetActive(true);
		return true;
	}

	public bool TryReturn(GameObject poolObj)
	{
		if (poolObj == null)
		{
			return false;
		}

		poolObj.SetActive(false);
		_pool.Enqueue(poolObj);
		return true;
	}
}

