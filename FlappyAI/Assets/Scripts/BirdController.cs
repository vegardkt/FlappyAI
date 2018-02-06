using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdController : MonoBehaviour {

    public Rigidbody2D BirdBody;
    public NeuralNetwork net;
    public List<GameObject> pipes;
    public GameObject pipe = null;
    public float FlapForce = 2;
    public bool initialized = false;
    public Manager managerScript;
    public float timeAlive;
    public float timeInit;
    private float fitness = 0;
    public bool alive = true;
    public PipeSpawnerController SpawnController;


    void Start ()
    {
        BirdBody = GetComponent<Rigidbody2D>();
        managerScript = GameObject.Find("ManagerObject").GetComponent<Manager>();
        SpawnController = GameObject.Find("PipeSpawner").GetComponent<PipeSpawnerController>();
        timeInit = Time.time;
	}
	
	// Update is called once per frame
	private void FixedUpdate ()
    {
        timeAlive = Time.time - timeInit;
        if (initialized && alive)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPosition.y > Screen.height || screenPosition.y < 0)
            {
                BirdBody.constraints = RigidbodyConstraints2D.FreezeAll;
                managerScript.BirdKilled();
                alive = false;
            }

            //calculate inputs
            float[] inputs = new float[managerScript.getNoOfInputs()];
            pipe = GetClosestPipe();
            if (pipe != null)
            {
                inputs[0] = (pipe.transform.position.x + transform.position.x)/5.5f ; //relative position of pipe
                inputs[1] = (pipe.transform.position.y - transform.position.y)/2f; //relative position of pipe
                //inputs[2] = transform.position.y/5;
                
            }
            else
            {
                inputs[0] = 0;
                inputs[1] = 0;
                //inputs[2] = transform.position.y/5;
            }
            

            float[] output = net.FeedForward(inputs);
            //Debug.Log(output[0]);
            if(output[0] > 0.2f)
            {
                BirdBody.velocity = new Vector2(0, FlapForce);
                
            }

            //set fitness here
            if (pipe != null)
            {
                fitness = timeAlive - Mathf.Abs(transform.position.y - pipe.transform.position.y);
                net.SetFitness(fitness);               
            }
            else
            {
                net.SetFitness(timeAlive);
            }
        }

	}

    public GameObject GetClosestPipe()
    {
        //pipes = GameObject.FindGameObjectsWithTag("PipeTag");
        pipes = SpawnController.GetPipes();

        GameObject closestPipe = null;
        float dist = Mathf.Infinity;
        Vector3 pos = transform.position;

            foreach (GameObject pipe in pipes)
            {
                if (pipe != null)
                {
                    if (pipe.transform.position.x > transform.position.x)
                    {
                        float diff = pipe.transform.position.x - pos.x;
                        float curDist = diff;
                        if (curDist < dist)
                        {
                            closestPipe = pipe;
                            dist = curDist;
                        }
                    }
                }
        }
        

        return closestPipe;

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "PipeTag")
        {
            if(alive)
            {
                //Destroy(gameObject);
                alive = false;
                managerScript.BirdKilled();
            }
            
        }
    }

    public void Init(NeuralNetwork net)
    {
        this.net = net;
        initialized = true;
        alive = true;
    }



}
