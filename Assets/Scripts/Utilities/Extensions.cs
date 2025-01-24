using UnityEngine;

public static class Extensions
{
	public static bool TryGetComponentInParent<T>(this Transform transform, out T component) where T : Component
	{
		component = null;
		var parent = transform.parent;
		if (parent == null) return false;

		return parent.TryGetComponent(out component);
	}

	public static Vector3 Absolute(this Vector3 reference)
	{
		reference.x = Mathf.Abs(reference.x);
		reference.y = Mathf.Abs(reference.y);
		reference.z = Mathf.Abs(reference.z);

		return reference;
	}

}
