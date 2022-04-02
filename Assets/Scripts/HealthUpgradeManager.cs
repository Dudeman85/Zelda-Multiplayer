using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeManager : MonoBehaviour {

    private GameObject[] item;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.tag = "Untagged";
        item = GameObject.FindGameObjectsWithTag("Health Upgrade");
        for(var i = 0; i < item.Length; i++)
            Destroy(item[i]);
    }
}
