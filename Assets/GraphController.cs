using UnityEngine;

public class GraphController : MonoBehaviour {

	// Use this for initialization
	private void Start () {

		//create ruleset
		RuleSet rs = new RuleSet("F-");
		rs.AddRule("F", "F-FF--F-F");
        // rs.AddRule("Y", "-FX-Y");
		// rs.AddTerminal("X","");
		// rs.AddTerminal("Y","");
		rs.ValidateTerminals();

		//create L system
		LSystem ls = new LSystem("F-F-F-F", 4, rs);
		string lSystemOutput = ls.Generate();
		ls.Information()
		;
		//use turtle to draw l system
		Turtle turtle = new Turtle(90f);
		turtle.Decode(lSystemOutput);

    }

	// Update is called once per frame
	void Update () {

	}

}


//subdivision as rescaling
//evolve an L system that approximates a line?
//separate out the ruleset logic so I can have a database of systems?
//quad subdivision and apply evo algo
