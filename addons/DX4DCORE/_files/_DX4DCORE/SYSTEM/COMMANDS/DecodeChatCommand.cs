//- - - - - - - - - - - - - -
// D E C O D E  C O M M A N D
//- - - - - - - - - - - - - -

using UnityEngine;
using UnityEngine.Networking;

public partial class Chat
{
    string _activeCommand;
    string _activeSubject;
    string _activeProperty;
    string _activeValue;

    //This is called on the Reliable channel because Decoding is necessary for other commands
    [Command(channel = Channels.DefaultReliable)] //important => reliable
    public void CmdDecodeCommand(string value)
    {
        //Debug.Log("DECODING COMMAND...: " + value);
        
        //CLEAR CACHED COMMANDS
        _activeCommand = string.Empty;
        _activeSubject = string.Empty;
        _activeProperty = string.Empty;
        _activeValue = string.Empty;

        value = value.Trim();
        string[] args = value.Split(commandArgsDelimiter);

        for (int i = 0; i < args.Length; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        _activeCommand = args[0].TrimStart(commandPrefix); //Trims out the commandPrefix
                        break;
                    }
                case 1:
                    {
                        _activeSubject = args[1];
                        break;
                    }
                case 2:
                    {
                        _activeProperty = args[2];
                        break;
                    }
                case 3:
                    {
                        _activeValue = args[3];
                        break;
                    }
                default:
                    {
                        _activeValue += " " + args[i];
                        break;
                    }
            }
        }

        //Debug.Log("DECODED COMMAND: "+commandPrefix+_activeCommand+" "+_activeSubject+" "+_activeProperty+" "+_activeValue);
    }
}