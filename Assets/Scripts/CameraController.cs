using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private float speed = 10;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        speed *= FindObjectOfType<DeckManager>().spacing;
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            cam.orthographicSize -= scrollWheel * Time.deltaTime * speed*4;
            cam.transform.position += transform.forward*scrollWheel * Time.deltaTime * speed*12;
        }
        Vector3 vectorPositive = new Vector3(1,0,1);
        Vector3 vectorNegative = new Vector3(-1,0,1);
        
        Vector3 dir = Input.GetAxisRaw("Horizontal")*vectorPositive +Input.GetAxisRaw("Vertical")*vectorNegative;

        if (dir != Vector3.zero)
        {
            transform.position += dir*Time.deltaTime*speed/3;
        }
    }
}
