using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    Dictionary<GameObject, List<GameObject>> _listObjects = new();

    public GameObject GetObject(GameObject defaultPref)
    {
        if (_listObjects.ContainsKey(defaultPref))
        {
            foreach (GameObject g in _listObjects[defaultPref])
            {
                if (g.activeSelf)
                    continue;
                return g;
            }
            GameObject b = Instantiate(defaultPref, this.transform.position, Quaternion.identity, this.transform);
            _listObjects[defaultPref].Add(b);
            b.SetActive(false);

            return b;
        }
        List<GameObject> _newList = new();

        GameObject o = Instantiate(defaultPref, this.transform.position, Quaternion.identity, this.transform);
        _newList.Add(o);
        o.SetActive(false);
        _listObjects.Add(defaultPref, _newList);

        return o;
    }
}
