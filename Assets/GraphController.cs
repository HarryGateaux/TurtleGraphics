using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour {
    [SerializeField] Texture2D _texture = null;
    [SerializeField] Mesh _mesh = null;
    [SerializeField] Material _material = null;

    Mesh _renderMesh;
    Color32[] _colors;

	// Use this for initialization
	private void Start () {
		//DrawLine(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Color(0,0,0));
		Turtle turtle = new Turtle(90f);
		turtle.Decode("F[+F][F][-F]\"F+F-F-F+F+F+F\"-F-F+F-F+\"F-F-F+\"F-F+F-F-F+F+F+F-F-F+F");
    }

	// Update is called once per frame
	void Update () {

	}

//generates random points
	IEnumerable<Vector3> SetPoints(int count)
    {
        while (count > 0)
        {
            float x = Random.value;
            float y = Random.value;

            var color = new Color(1, 1, 1, 1);

            count--;
            yield return new Vector2(x, y);
            
        }
    }

}



