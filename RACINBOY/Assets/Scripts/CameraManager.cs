using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target;
    public float chaseSpeed;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(target.transform.position.x, target.transform.position.y,transform.position.z),
            chaseSpeed * Time.deltaTime);
    }
}
