﻿using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphController : MonoBehaviour {

    // Use this for initialization
    Turtle turtle;
    public Material material;

    private void Start () {

        string systemchoice = gameObject.GetComponent<Text>().text;

        InitialiseDB.Initialise();

        var test = new LSystemDB();
        var systemsJSON = test.ReadFromFile();
		var rsTest = systemsJSON[systemchoice];
		
		//create L system
		LSystem ls = new LSystem(rsTest._axiom, 4, rsTest);
		string lSystemOutput = ls.Generate();
		ls.Information();
		
		//use turtle to draw l system
		turtle = new Turtle(rsTest._angle);
		turtle.Decode(lSystemOutput);
        turtle.DrawMesh();
    }

	// Update is called once per frame
	void Update () {
        Graphics.DrawMesh(turtle.DrawTurtle(), Matrix4x4.identity, material, 0);
    }
}

//subdivision as rescaling
//evolve an L system that approximates a line?
//database of systems
//quad subdivision and apply evo algo
