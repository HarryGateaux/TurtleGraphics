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
		DrawLine(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Color(0,0,0));
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

	 void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
         {
             GameObject myLine = new GameObject();
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



