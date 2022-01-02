using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Finder
{
    public static GameObject PRODUCTION;

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static GameObject FindComponentInChildrenByName(GameObject parent, string name)
    {
        List<Component> components = new List<Component>();
        parent.GetComponentsInChildren(true, components);
        foreach (Component c in components)
        {
            if (c.name.Equals(name))
            {
                return c.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindActiveComponentByName(string name)
    {
        return GameObject.Find(name);
    }

    public static GameObject FindGameObjectByName(string name)
    {
        foreach (GameObject o in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (o.name.Equals(name))
            {
                return o;
            }
        }

        return null;
    }
}

