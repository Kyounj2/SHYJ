using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float sinTime;

    // Update is called once per frame
    void Update()
    {
        sinTime += Time.deltaTime;
        transform.position += Vector3.down * Mathf.Sin(20 * sinTime) * 1f * Time.deltaTime;
    }
}
