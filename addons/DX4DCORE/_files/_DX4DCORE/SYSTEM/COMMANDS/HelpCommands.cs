//- - - - - - - - - - - - - - - - - -
// - - -         H E L P        - - - 
// - - - C O M M A N D  L I S T - - -
//- - - - - - - - - - - - - - - - - - 
//HELP (?)
//

using UnityEngine;
using UnityEngine.Networking;

public partial class Chat
{
    [Command(channel = Channels.DefaultUnreliable)] // unimportant => unreliable
    internal void CmdHelpCommands(string command)
    {
        //REGISTER COMMANDS
        Chat.RegisterCommand("help");

        //PROCESS COMMANDS
        switch (_activeCommand.ToUpper())
        {
            // - - - H E L P - - -
            #region HELP
            case "?": { goto case "HELP"; }
            case "HELP":
                {
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    my.chat.TargetMsgInfo(connectionToClient, Chat.HelpCommandList());
                    break;
                }
                #endregion
        }
    }
}