using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class GraphController : MonoBehaviour {

    // Use this for initialization
    Turtle turtle;

    private void Start () {

		//create ruleset
		RuleSet rs = new RuleSet(new char[2]{'F','-'});
		rs.AddRule("F", "F-FF--F-F");
        // rs.AddRule("Y", "-FX-Y");
		// rs.AddTerminal("X","");
		// rs.AddTerminal("Y","");
		rs.ValidateTerminals();

		RuleSet rs2 = new RuleSet(new char[2]{'F','-'});
		rs2.AddRule("F", "F-F[+F]ff");
		rs2.ValidateTerminals();

		Dictionary<string, RuleSet> systems = new Dictionary<string, RuleSet>();
		systems.Add("one", rs);
		systems.Add("two", rs2);
		
		// Debug.Log(t);
		string rootPath = @"C:\Users\antmi\Documents\Unity\TurtleGraphics";
		File.WriteAllText(rootPath + @"\systems.json", JsonConvert.SerializeObject(systems, Formatting.Indented));
		
		string filetext = File.ReadAllText(rootPath + @"\systems.json");
		
		Dictionary<string, RuleSet> systemsJSON = JsonConvert.DeserializeObject<Dictionary<string, RuleSet>>(filetext);
		var rsTest = systemsJSON["two"];
		
		//create L system
		LSystem ls = new LSystem("F-F-F-F", 4, rsTest);
		string lSystemOutput = ls.Generate();
		ls.Information();
	
		
		//use turtle to draw l system
		turtle = new Turtle(90f);
		turtle.Decode(lSystemOutput);
        turtle.DrawMesh();
    }

	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<MeshFilter>().mesh = turtle.DrawTurtle();
    }

}


//subdivision as rescaling
//evolve an L system that approximates a line?
//database of systems
//quad subdivision and apply evo algo
