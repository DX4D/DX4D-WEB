using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; // copied from Unity/Mono/lib/mono/2.0 to Plugins
using UnityEngine;

public partial class Database {

    // returns a list of character names for all accounts
    public static List<string> AllCharacters()
    {
        var result = new List<string>();
        var table = ExecuteReader("SELECT name FROM characters WHERE deleted=0");
        foreach (var row in table)
            result.Add((string)row[0]);
        return result;
    }
}
