using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAward : MonoBehaviour
{
    public GameObject HideEnemies;
    public GameObject ShowTurret;
    public GameObject ShowAllies;

    void OnTriggerExit(Collider other)
    {
       Destroy(HideEnemies); 
       ShowAllies.SetActive(true);
       ShowTurret.SetActive(true);
    }
}