using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

//allows user to save LSystems to a JSON database
public class LSystemDB
{
    string _pathDB;
    Dictionary<string, RuleSet> _DB;

    public LSystemDB()
    {
        _pathDB = @"C:\Users\antmi\Documents\Unity\TurtleGraphics\systems.json";
        _DB = new Dictionary<string, RuleSet>();
    }

    public void AddSystem(RuleSet rs)
    {
        _DB.Add(_DB.Count.ToString(), rs);
    }

    public void WriteToFile()
    {
        File.WriteAllText(_pathDB, JsonConvert.SerializeObject(_DB, Formatting.Indented));
    }

    public Dictionary<string, RuleSet> ReadFromFile()
    {
        string filetext = File.ReadAllText(_pathDB);
        return JsonConvert.DeserializeObject<Dictionary<string, RuleSet>>(filetext);
    }

}

public static class InitialiseDB
{
    public static void Initialise()
    {
        //fix issue with checking the alphabet versus the axiom
        //put the axiom into the L System


        //create ruleset
        RuleSet rs = new RuleSet(new char[2] { 'F', '-' });
        rs.AddRule("F", "F-FF--F-F");
        rs.ValidateTerminals();

        RuleSet rs2 = new RuleSet(new char[2] { 'F', '-' });
        rs2.AddRule("F", "F+F-F-F+F");
        rs2.ValidateTerminals();

        RuleSet rs3 = new RuleSet(new char[2] { 'F', '-' });
        rs3.AddRule("F", "F-Ff[F-F]-F+F");
        rs3.ValidateTerminals();

        RuleSet rs4 = new RuleSet(new char[3] { 'F', 'G', '-' });
        rs4.AddRule("F", "FF");
        rs4.AddRule("G", "G[+F][-F]");
        rs4.AddTerminal("G", "");
        rs4.ValidateTerminals();

        var DB = new LSystemDB();
        DB.AddSystem(rs);
        DB.AddSystem(rs2);
        DB.AddSystem(rs3); 
        DB.AddSystem(rs4);

        DB.WriteToFile();
    }


}
