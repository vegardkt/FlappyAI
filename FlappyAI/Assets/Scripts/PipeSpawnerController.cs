using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnerController : MonoBehaviour {

    public GameObject PipeGroup;
    public GameObject ScoreTrigger;
    public GameObject Bird;
    public List<GameObject> Pipes;
    public GameObject pipe;
    int pipeCount = 0;
    
	void Start ()
    {
        InvokeRepeating("SpawnPipe", 1f, 1.5f);
        
    }
	
	void SpawnPipe ()
    {
        pipe = Instantiate(PipeGroup);
        pipe.name = "pipe" + pipeCount.ToString();
        Pipes.Add(pipe);
        pipeCount += 1;
	}

    public void DestroyPipe(GameObject pipe)
    {
        Pipes.Remove(pipe);
        GameObject.Destroy(pipe);
    }

    public List<GameObject> GetPipes()
    {
        return Pipes;
    }
}
