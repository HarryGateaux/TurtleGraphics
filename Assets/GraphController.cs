using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour {

	[SerializeField] Texture2D _texture = null;
    [SerializeField] Mesh _mesh = null;
    [SerializeField] Material _material = null;
	// Use this for initialization
	void Start () {
		
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



