// - - - - - - - - - - - - - - - - - - - - - -
//- - -     C O M M A N D S  L I S T     - - -
// - - - - - - - - - - - - - - - - - - - - - -
using System.Collections.Generic;

public partial class Chat
{
    //COMMANDS
    public static List<string> Commands { get { return commands; } }
    static List<string> commands = new List<string>();
    static string commandList;

    /// <summary> Register a chat command. this method adds it to the Chat.Commands list </summary>
    public static void RegisterCommand(string commandName) { commands.Add(commandName); }

    //[Command(channel = Channels.DefaultUnreliable)] // unimportant => unreliable
    public static string HelpCommandList() {

        foreach ( string commandName in commands)
        {
            commandList += commandPrefix + commandName + lineBreak;
        }

        return (commandHelpHeader + commandList);
    }
}