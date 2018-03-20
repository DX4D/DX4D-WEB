// * * * * * * * * * * * * * * * *
// HOW TO: ENABLE A NEW COMMAND  *
// * * * * * * * * * * * * * * * *
// <<<=== [   Delete the "//" at the beginning of the appropriate line of code for the Command Groups you want to use  ] ===>>>
//            //CmdExampleCommands(commandText);
//            //CmdCustomCommands(commandText); // <<<=== CUSTOM COMMANDS
//

using UnityEngine;
using UnityEngine.Networking;

public partial class Chat
{
    // C O M M A N D  F O R M A T T I N G
    const char commandPrefix = '/';
    const char commandArgsDelimiter = ' ';
    const string lineBreak = "\r\n";
    const string commandHelpHeader = (lineBreak + " <| COMMAND LIST |> " + lineBreak);

    // R U N  C O M M A N D
    void OnSubmit_Commands(string commandText)
    {
        // D E C O D E  C O M M A N D
        CmdDecodeCommand(commandText);

        // C O M M A N D  P A C K S
        //CmdTravelCommands(commandText); // <<<=== TRAVEL COMMANDS
        //CmdEntityCommands(commandText); // <<<=== ENTITY COMMANDS
        //CmdInventoryCommands(commandText); // <<<=== INVENTORY COMMANDS

        // C O R E  C O M M A N D  P A C K S
        CmdHelpCommands(commandText); // <<<=== HELP COMMANDS - this must be called last
    }

}