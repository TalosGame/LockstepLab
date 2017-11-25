using UnityEngine;
using System.Collections.Generic;

public static class MonoExtendUtil
{
    private const string DDOL_MANAGER_TAG = "DDOLManagerRoot";

	/// <summary>
    /// Gets the or add component.
	/// </summary>
	/// <returns>The or add compent.</returns>
	/// <param name="go">Go.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T> ();
		if (t == null) 
		{
			t = go.AddComponent<T> ();
		}

		return t;
	}

	public static T GetOrAddComponent<T>(this Transform trans) where T : Component
	{
		T t = trans.GetComponent<T>();
		if (t == null) 
		{
			t = trans.gameObject.AddComponent<T> ();
		}

		return t;
	}

	/// <summary>
	/// 扩展对象添加到永不销毁到根节点下
	/// </summary>
	/// <param name="gameObject">game object.</param>
    public static void AddToDDOLRoot(this GameObject gameObject)
	{
		GameObject ddolMgr = GameObject.FindGameObjectWithTag(DDOL_MANAGER_TAG);
		if (ddolMgr == null)
		{
			ddolMgr = new GameObject("DDOLManagerRoot");
			ddolMgr.tag = DDOL_MANAGER_TAG;

			GameObject.DontDestroyOnLoad(ddolMgr);
		}

		gameObject.transform.parent = ddolMgr.transform;
	}

    /// <summary>
    /// 查询子节点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindDeepChild(GameObject target, string childName)
    {
        Transform ret = target.transform.Find(childName);
        if (ret == null)
        {
            // 如果没有搜寻到， 递归搜寻各子节点
            foreach(Transform childTrans in target.transform)
            {
                ret = FindDeepChild(childTrans.gameObject, childName);
                if(ret != null)
                {
                    return ret;
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 查找子节点所拥有的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="target">父节点</param>
    /// <param name="childName">子节点名称</param>
    /// <returns></returns>
    public static T FindDeepChild<T>(GameObject target, string childName) where T : Component
    {
        Transform childTrans = FindDeepChild(target, childName);
        if (childTrans == null)
        {
            return null;
        }

        T ret = childTrans.GetComponent<T>();
        return ret;
    }



    /// <summary>
    /// 创建一个childPrefab的实例，并设置其所有子GameObject的layer为parent的layer
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childPrefab"></param>
    /// <returns></returns>
    public static GameObject CreateChild(GameObject parent, GameObject childPrefab)
    {
        if (childPrefab == null)
        {
            return null;
        }

        GameObject child = GameObject.Instantiate(childPrefab) as GameObject;
        child.name = childPrefab.name;

        if(parent == null)
        {
            return child;
        }

        AddChildToTarget(parent.transform, child.transform);
        return child;
    }

    /// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target, Transform child, Vector3 pos)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = pos;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 修改子节点Layer  NGUITools.SetLayer();
    /// </summary>
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }

    public static void SetChildIdentityNoLayer(GameObject parent, GameObject child)
    {
        child.transform.parent = parent.transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 删除所有子节点
    /// </summary>
    /// <param name="root"></param>
    public static void DestroyChildren(Transform root)
    {
        for (int i = root.childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(root.GetChild(i).gameObject);
        }
        root.DetachChildren();
    }
}