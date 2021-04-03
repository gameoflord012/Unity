using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public GameObject base_prefab;
    private new Camera camera;
    private float base_wide_size;
    bool spawned;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        base_wide_size = GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        spawned = false;
    }

    private bool IsOutOfBound()
    {        
        Vector3 postocam = camera.WorldToScreenPoint(gameObject.transform.position);        
        return (postocam.x + base_wide_size / 2f < 0);
    }

    private bool IsInMiddle()
    {
        Vector3 postocam = camera.WorldToScreenPoint(gameObject.transform.position);        
        return (postocam.x + base_wide_size / 2f < camera.pixelWidth);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position -= new Vector3(Globals.GAME_SPEED, 0, 0) * Time.deltaTime;

        if (!spawned && IsInMiddle())
        {
            spawned = true;
            Instantiate(base_prefab, gameObject.transform.position + new Vector3(base_wide_size * 1.6f / 200f , 0, 0), Quaternion.identity);
        }
        if (IsOutOfBound())
            Destroy(gameObject);
    }
}
