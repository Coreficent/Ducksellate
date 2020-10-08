using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Cell : MonoBehaviour
{
    private readonly float rotationAngle = 90f;
    private float targetAngle = 0f;
    private readonly float space = 1f;
    private float direction = 1f;
    private bool activated = false;
    public int X { get; set; }
    public int Y { get; set; }

    void Start()
    {
        transform.position = new Vector3(X * space, Y * space, 0f);
        CorrectAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.Rotate(Vector3.forward * direction, rotationAngle * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - targetAngle) < 10f)
            {
                CorrectAngle();
                activated = false;
                if (X < Main.cells.GetLength(0) - 1)
                {
                    Main.activatedCells.Enqueue(Main.cells[X + 1, Y]);
                }
                if (Y < Main.cells.GetLength(1) - 1)
                {
                    Main.activatedCells.Enqueue(Main.cells[X, Y + 1]);
                }
            }
        }
    }

    void OnMouseOver()
    {
        if (!activated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Log.Output("left clicked on::" + X + "::" + Y);
                direction = 1f;
                targetAngle = CalculateTargetAngle();
                activated = true;

            }
            else if (Input.GetMouseButtonDown(1))
            {
                Log.Output("right clicked on::" + X + "::" + Y);
                direction = -1f;
                targetAngle = CalculateTargetAngle();
                activated = true;
            }
        }
    }

    public void React()
    {
        targetAngle = CalculateTargetAngle();
        activated = true;
    }

    private float CalculateTargetAngle()
    {
        float target = transform.eulerAngles.z + rotationAngle * direction;
        target = target > 360f ? target - 360f : target;
        target = target < 0f ? target + 360f : target;
        Log.Output("target " + target);

        return target;
    }

    private void CorrectAngle()
    {
        // prevents inaccuracy over many iterations
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Round(transform.eulerAngles.z / 90f) * 90f);
    }
}
