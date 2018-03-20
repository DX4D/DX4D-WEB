// - - - - - - - - - - - - - - - - - -
// - - -       T R A V E L      - - -
// - - - C O M M A N D  L I S T - - -
// - - - - - - - - - - - - - - - - - -
//GO (GOTO)
//POS (COORDS)
//

using UnityEngine;
using UnityEngine.Networking;

public partial class Chat
{
    [Command(channel = Channels.DefaultUnreliable)] // unimportant => unreliable
    internal void CmdTravelCommands(string command)
    {
        //REGISTER COMMANDS
        Chat.RegisterCommand("go");
        Chat.RegisterCommand("goto");
        Chat.RegisterCommand("pos");
        Chat.RegisterCommand("coords");

        //PROCESS COMMANDS
        switch (_activeCommand.ToUpper())
        {
            // - - - G O - - -
            #region GO
            case "GOTO": { goto case "GO"; }
            case "GO":
                {
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    //new position
                    Vector3 newPosition = Vector3.zero;

                    //set new position check for region keywords
                    switch(_activeSubject.ToUpper())
                    {
                        case "HOUSE": { goto case "HOME"; }
                        case "HOME":
                            {
                                newPosition = Location.Home;
                                break;
                            }
                        case "STORE": { goto case "SHOP"; }
                        case "SHOP":
                            {
                                newPosition = Location.Shop;
                                break;
                            }
                        case "FIGHT": { goto case "ARENA"; }
                        case "BATTLE": { goto case "ARENA"; }
                        case "ARENA":
                            {
                                newPosition = Location.Arena;
                                break;
                            }
                        case "FIELD":
                            {
                                newPosition = Location.Field;
                                break;
                            }
                        case "HEAVEN": { goto case "SKY"; }
                        case "SKY":
                            {
                                newPosition = Location.Sky;
                                break;
                            }
                        case "HELL": { goto case "UNDERWORLD"; }
                        case "UNDERWORLD":
                            {
                                newPosition = Location.Underworld;
                                break;
                            }
                        default:
                            {
                                newPosition = new Vector3Int(_activeSubject.ToInt(), _activeProperty.ToInt(), _activeValue.ToInt());
                                break;
                            }
                    }

                    //set new position
                    my.agent.Warp(newPosition);

                    return; //EXIT WARP
                }
            #endregion

            // - - - P O S - - -
            #region POS
            case "COORDS": { goto case "POS"; }
            case "POS":
                {
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    Vector3 playerPos = my.transform.position;

                    //show position
                    my.chat.TargetMsgInfo(connectionToClient, "You are located at: " + playerPos.x+" "+playerPos.y+" "+playerPos.z + ".");

                    return; //EXIT POS
                }
                #endregion

        }
    }
}