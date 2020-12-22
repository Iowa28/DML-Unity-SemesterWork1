using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private HealthManager healtManager;

    [SerializeField]
    private Renderer renderer;

    [SerializeField]
    private Material cpOff;
    [SerializeField]
    private Material cpOn;

    void Start()
    {
        healtManager = FindObjectOfType<HealthManager>();
    }

    void Update()
    {
        
    }

    private void CheckpointOn()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint cp in checkpoints)
        {
            cp.CheckpointOff();
        }

        renderer.material = cpOn;
    }

    private void CheckpointOff()
    {
        renderer.material = cpOff;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            healtManager.SetSpawnPoint(transform.position);
            CheckpointOn();
        }
    }
}
