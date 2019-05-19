using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitCameraController : MonoBehaviour
{
    public float speed;
    public GameObject target;

    

    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime,0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position;
    }
}
