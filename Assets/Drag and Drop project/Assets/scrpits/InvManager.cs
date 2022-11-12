using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour {

    public List<GameObject> Items;

    public GameObject SearchItemByName(string Name)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].transform.name == Name)
            {
                return Items[i];
            }
        }
        return null;
    }

    void Awake() {
    }
}
