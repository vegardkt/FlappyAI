using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesController : MonoBehaviour {

    public Vector2 PipeVelocity = new Vector2(-4, 0);
    public float range;
    void Start ()
    {
        GetComponent<Rigidbody2D>().velocity = PipeVelocity;
        transform.position = new Vector3(transform.position.x, transform.position.y - range * Random.value, transform.position.z);
	}
	
}
