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
    private Stack<Vector3[]> _drawStack;
    private float _defaultAngle;
    private float _lineWidth;
    private float _stepSize;   
    private int _lineCount;

    public Turtle(float defaultAngle)
    {
        _pos = new Vector3();
        _heading = Vector3.up;
        _pointStack = new Stack<Vector3>();
        _drawStack = new Stack<Vector3[]>();
        _defaultAngle = defaultAngle;
        _lineWidth = 0.01f;
        _stepSize = 0.05f;
        _lineCount = 0;
    }

    //parses the string instructions to draw the shape
    public void Decode(string instructions){

        if(instructions == null) {throw new NullReferenceException("ERROR : null input to Turtle");}
        foreach(char c in instructions){

            switch(c)
            {
                case 'F': //move fwd and draw
                    Move();
                    break;

                 case 'f': //move fwd only
                    Move(false);
                    break;

                 case '+': //turn left by angle
                    Turn(-1 * _defaultAngle);
                    break;

                 case '-': //turn right by angle
                    Turn(_defaultAngle);
                    break;
                     
                 case '|': //turn around
                    Turn(180f);
                    break;                   

                 case '[': //push pos & heading to memory
                    var memory = new Vector3[2] {_pos, _heading};
                    _drawStack.Push(memory);
                    break;  

                 case ']': //pop pos & heading from memory

                    var recall = new Vector3[2];
                    recall = _drawStack.Pop();
                    _pos = recall[0];
                    _heading = recall[1];
                    break; 

                 case '"': //rescale step size

                    _stepSize *= 0.5f;
                    break;

                 case '!': //rescale line width

                    _lineWidth *= 0.5f;
                    break;

                 default:

                    throw new ArgumentException(c + " is not a valid argument");
            }
        }
    }

    private void Move (bool draw = true) {

        if(draw){

            AddPoint();
            _pos += _heading * _stepSize;
            AddPoint();
            DrawLine();

        } else {

            _pos += _heading * _stepSize;
        }
    }

    private void AddPoint(){

        _pointStack.Push(_pos);
    }

    private void Turn (float angle){

        var rotation  = Quaternion.AngleAxis(angle , Vector3.forward);
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
        myLine.name = "Line" + _lineCount.ToString();

        LineRenderer lineRenderer = myLine.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
        _lineCount++;
    }

}


