using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//turtle graphics class
class Turtle
{
    private Vector3 pos;
    private Vector3 heading;

    public Turtle()
    {
        pos = new Vector3();
        heading = Vector3.up;
    }

    public void move (float distance) {

        pos += heading * distance;

    }

    public void turn (float angle){

        var rotation  = Quaternion.AngleAxis(angle , Vector3.up);
        heading = rotation * heading;
    }



}
