using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LSystem
{
    public Dictionary<string, string> _ruleset;
    private string _axiom;
    private int _numIterations;

    public LSystem(string axiom, int numIterations)
    {
        _axiom = axiom;
        _ruleset = new Dictionary<string, string>();
        _numIterations = numIterations;
        //check axiom is valid....
        //check in alphabet, if so ignore
        SetRules();
    }

    private void SetRules(){

        _ruleset.Add("F", "F+F-F-F+F");
    }

    public string Generate(){

        return ReplaceRecursive(_axiom, _numIterations);
    }

    private string ReplaceRecursive(string prevString, int numIterations){



        if(numIterations == 0) {return prevString;}
        
        string nextString = "";

        foreach(char c in prevString)
        {   try
            {
                nextString += _ruleset[c.ToString()];
            }

            catch 
            {
                nextString += c;
            }
        }
        numIterations--;
        return ReplaceRecursive(nextString, numIterations);   
    }
}
