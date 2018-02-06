using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDestroyerController : MonoBehaviour {

    public PipeSpawnerController SpawnController;
    public List<GameObject> Pipes;
    private void Start()
    {
        SpawnController = GameObject.Find("PipeSpawner").GetComponent<PipeSpawnerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PipeTag")
        {
            SpawnController.DestroyPipe(collision.gameObject);
        }
    }
}
