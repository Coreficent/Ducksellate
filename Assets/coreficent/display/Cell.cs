using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Cell : MonoBehaviour
{
    private float space = 1f;
    private bool activated = false;
    private Vector3 previousAngle;
    public int X { get; set; }
    public int Y { get; set; }

    void Start()
    {
        transform.position = new Vector3(X * space, Y * space, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            // Log.Output(transform.eulerAngles);
            transform.Rotate(Vector3.forward, 90f * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - previousAngle.z) > 90f)
            {
                //TODO set to exactly that degree
                activated = false;
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Log.Output("clicked on::" + X + "::" + Y);
            previousAngle = transform.eulerAngles;
            activated = true;
        }
    }
}
