using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LSystem
{
    public Dictionary<string, string> _ruleset, _terminals;
    private string _axiom;
    private string _generatedString;
    private int _numIterations;
    private char[] _symbols;

    private bool _validSetup;

    public LSystem(string axiom, int numIterations, string symbols)
    {
        _axiom = axiom;
        _ruleset = new Dictionary<string, string>();
        _terminals = new Dictionary<string, string>();
        _numIterations = numIterations;
        _symbols = symbols.ToCharArray();
        ValidateAxiom();
    }

    private void ValidateAxiom(){

        _validSetup = true;
        if(!_symbols.Contains(char.Parse(_axiom)))
        {
            Debug.Log("ERROR : " + _axiom + " not contained in symbol set, please check");
            _validSetup = false;
        }

    }
    //defines what each symbol maps to in the next generation
    public void AddRule(string input, string output){

        if(_symbols.Contains(char.Parse(input))){

            _ruleset.Add(input, output);

        } else {
            
            Debug.Log("ERROR : " + input + " not in symbol set, rule not added");
            _validSetup = false;
        }
    }

    //defines what each symbol maps to in final generation
    public void AddTerminal(string input, string output){

        if(_symbols.Contains(char.Parse(input))){

            _terminals.Add(input, output);

        } else {
            
            Debug.Log("ERROR : " + input + " not in symbol set, rule not added");
            _validSetup = false;
        }
    }

    //maps the final generation with terminals
    private void ApplyTerminals(){

        foreach(string s in _terminals.Keys){
            _generatedString = _generatedString.Replace(s , _terminals[s]);
        }
    }

    public string Generate(){

        if(_validSetup && _ruleset.Count != 0){
            _generatedString = ReplaceRecursive(_axiom, _numIterations);
            ApplyTerminals();
            return _generatedString;
        }
        else 
        {
            Debug.Log("ERROR : LSystem not generated, problem with setup, please check");
            return null;
        }

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
