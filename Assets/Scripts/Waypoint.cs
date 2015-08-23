using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovingObject))]
public class Waypoint : MonoBehaviour
{
    private MovingObject movingObject;
    private int index = 0;
    public Vector3[] wayPoints;
    public bool loop = false;

    void Start()
    {
        movingObject = GetComponent<MovingObject>();
    }

    void Update()
    {
        if (movingObject.moveOption == MovingObject.MoveOption.Arrived && index < wayPoints.Length)
        {
            movingObject.setMovement = true;
            movingObject.moveOption = MovingObject.MoveOption.MoveToLocation;
            movingObject.moveToLocation = wayPoints[index];
            index = loop ? (index + 1) % wayPoints.Length : index +1;
        }
    }
}
