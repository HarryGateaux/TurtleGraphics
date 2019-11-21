using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//turtle graphics class
class Turtle
{
    Vector3 _pos;
    Vector3 _heading;
    Stack<Vector3> _pointStack;
    Stack<Vector3[]> _drawStack;
    float _defaultAngle;
    float _lineWidth;
    float _stepSize;   
    int _lineCount;

    List<Mesh> _lineMeshes;
    List<Matrix4x4> _transforms;
    Mesh _renderMesh;
    Material _material = new Material(Shader.Find("Particles/Standard Unlit"));
    MeshRenderer meshRenderer;

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
        _lineMeshes = new List<Mesh>();
        _transforms = new List<Matrix4x4>();
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

    //private void Move (bool draw = true) {

    //    if(draw){

    //        AddPoint();
    //        _pos += _heading * _stepSize;
    //        AddPoint();
    //        DrawLine();

    //    } else {

    //        _pos += _heading * _stepSize;
    //    }
    //}

    private void Move(bool draw = true)
    {

        if (draw)
        {

            AddPoint();
            _pos += _heading * _stepSize;
            AddPoint();
            DrawLineMesh();

        }
        else
        {

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

    //replace drawline with mesh lines
    //requires the points, and a list of the lines
    private void DrawLineMesh()
    {

        Vector3 start = _pointStack.Pop();
        Vector3 end = _pointStack.Pop();

        Vector3 lineVector = end - start;
        Vector3 lineNormal = Vector3.Cross(lineVector, Vector3.forward);
        Vector3 lineMidPoint = start + lineVector / 2f;

        Vector3 startshift = start + 0.1f * lineNormal;
        Vector3 endshift = startshift + lineVector;

        Vector3[] vertices = new Vector3[4];
        int[] indices = new int[6] { 0, 2, 3, 3, 1, 0 };

        vertices[0] = start;
        vertices[1] = startshift;
        vertices[2] = end;
        vertices[3] = endshift;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        //mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.triangles = indices;
        _lineMeshes.Add(mesh);

        var rotation = Quaternion.identity;
        var scale = Vector3.one;// * 0.01f;
        Matrix4x4 transform = Matrix4x4.TRS(lineMidPoint, rotation, scale);

        _transforms.Add(transform);
    }

    public void DrawMesh()
    {
        //combine meshes
        var finalMesh = new Mesh();

        // var instances = _lineMeshes
        //   .Select(mesh => new CombineInstance() { mesh = mesh});
        CombineInstance[] instances = new CombineInstance[_lineMeshes.Count];

        for(int i = 0; i < _lineMeshes.Count; i++)
        {
            CombineInstance instance = new CombineInstance();
            instance.mesh = _lineMeshes[i];
            instance.transform = _transforms[i];
            instances[i] = instance;
        }


        finalMesh.CombineMeshes(instances.ToArray());

        _renderMesh = finalMesh;
    }

    public void DrawTurtle()
    {
        //return _renderMesh;
        Graphics.DrawMesh(_renderMesh, Matrix4x4.identity, _material, 0);
    }
}
