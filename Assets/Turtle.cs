using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//turtle graphics class
class Turtle
{
    private Vector3 _pos;
    private Vector3 _heading;
    private Stack<Vector3> _pointStack;

    public Turtle()
    {
        _pos = new Vector3();
        _heading = Vector3.up;
        _pointStack = new Stack<Vector3>();
    }

    //parses the string instructions to draw the shape
    public void Generate(string instructions){

        foreach(char c in instructions){

            switch(c)
            {
                case 'F':
                    Move();
                    break;

                 case 'f':
                    Move(false, 1f);
                    break;

                 case '+':
                    Turn();
                    break;

                 default:

                    throw new ArgumentException(c + " is not a valid argument");
            }
        }
    }

    private void Move (bool draw = true, float distance = 1f) {

        if(draw){

            AddPoint();
            _pos += _heading * distance;
            AddPoint();
            DrawLine();

        } else {

            _pos += _heading * distance;
        }
    }

    private void AddPoint(){

        _pointStack.Push(_pos);
    }

    private void Turn (float angle = 90f){

        var rotation  = Quaternion.AngleAxis(angle , Vector3.up);
        _heading = rotation * _heading;
    }

    //draws a line connecting the two points in the point stack
    private void DrawLine(){

        GameObject myLine = new GameObject();
        Vector3 start = _pointStack.Pop();
        Vector3 end = _pointStack.Pop();
        Color color = new Color(1,1,1);
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }

}


