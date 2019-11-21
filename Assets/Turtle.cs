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

        Vector3 startshift = start + new Vector3(5, 5, 5);
        Vector3 endshift = start + new Vector3(5, 5, 5);
        //Vector3 start = new Vector3(0, 0, 0);
        //Vector3 end = new Vector3(5, 5, 0);
        Vector3 end2 = new Vector3(5, 6, 5);


        Vector3[] vertices = new Vector3[3];
        int[] indices = new int[3] { 0, 1, 2};

        vertices[0] = start;
        vertices[1] = end;
        vertices[2] = startshift;
        //vertices[3] = endshift;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        //mesh.triangles = indices;
        _lineMeshes.Add(mesh);

        var rotation = Quaternion.identity;
        var scale = Vector3.one * 0.01f;
        Matrix4x4 transform = Matrix4x4.TRS(end, rotation, scale);

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

        Debug.Log(finalMesh.vertexCount);
        _renderMesh = finalMesh;
    }

    public Mesh DrawTurtle()
    {
        return _renderMesh;
        //Graphics.DrawMesh(_renderMesh, Matrix4x4.identity, _material, 0);
        //Graphics.DrawMesh(_renderMesh, Matrix4x4.identity, _material, 0);
    }
}


//public class Controller
//{


//    void Start()
//    {
//        Mesh mesh = new Mesh();

//        Vertices = RandomPoints(100).ToArray();
//        //Vertices = new Vector3[8];
//        Lines = new int[100];
//        //Colors = new Color[Vertices.Length];

//        Vertices[0] = new Vector3(0, 0);
//        Vertices[1] = new Vector3(1, 0);
//        Vertices[2] = new Vector3(1, 1);
//        Vertices[3] = new Vector3(0, 1);

//        System.Random r = new System.Random();
//        for (int i = 0; i < 100; i++)
//            Lines[i] = r.Next(100);

//        //Colors[0] = Color.red;
//        //Colors[1] = Color.cyan;
//        //Colors[2] = Color.black;
//        //Colors[3] = Color.grey;

//        //mesh.colors = Colors;

//        mesh.vertices = Vertices;
//        //mesh.uv = UVs;
//        //mesh.triangles = Points;
//        mesh.SetIndices(Lines, MeshTopology.Lines, 0);

//        GameObject gameObject = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter));
//        gameObject.transform.localScale = new Vector3(5, 5, 1);

//        gameObject.GetComponent<MeshRenderer>().material = material;
//        gameObject.GetComponent<MeshFilter>().mesh = mesh;

//    }

//    IEnumerable<Vector3> RandomPoints(int count)
//    {
//        while (count > 0)
//        {
//            float x = Random.value;
//            float y = Random.value;
//            var position = new Vector3(x, y, 0);
//            count--;
//            yield return position;
//        }
//    }



//}
