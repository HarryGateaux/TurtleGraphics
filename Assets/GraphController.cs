using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour {

	// Use this for initialization
	private void Start () {

		//create L system
		LSystem ls = new LSystem("X", 4, "FX");
		ls.AddRule("X", "F[+X][-X]");
        ls.AddRule("F", "F");
		ls.AddTerminal("X","");
		string lSystemOutput = ls.Generate();
		Debug.Log(lSystemOutput);

		//use turtle to draw l system
		Turtle turtle = new Turtle(25f);
		turtle.Decode(lSystemOutput);

    }

	// Update is called once per frame
	void Update () {

	}

}



