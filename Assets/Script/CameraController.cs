using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public float ymin, ymax, xmin, xmax;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        targetPos.z = -10;
        transform.position = new Vector3(Mathf.Clamp(targetPos.x, xmin, xmax), Mathf.Clamp(targetPos.y, ymin, ymax), targetPos.z);


       // transform.position = targetPos;
    }
}
