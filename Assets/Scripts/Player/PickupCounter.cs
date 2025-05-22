using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCounter : MonoBehaviour
{
    public int currentPickups;
    public int allPickups;
    [SerializeField] private UIController UIController;
    [SerializeField] private SceneController sceneController;


    void Start()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        allPickups = pickups.Length;
        currentPickups = 0;
    }

}
