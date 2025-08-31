using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleVermelho : MonoBehaviour
{
    public AudioSource source;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void OnCollisionEnter2D (Collision2D coll) {
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //no update
Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
var pos = transform.position;
pos.x = mousePos.x;
pos.y = mousePos.y;
transform.position = pos;
    }

    

}
