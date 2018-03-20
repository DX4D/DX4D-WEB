//- - - - - - - - - - - - - - - - - - 
//            E N T I T Y
// - - - C O M M A N D  L I S T - - -
//- - - - - - - - - - - - - - - - - - 
//KILL
//HEAL
//
//HIDE
//SHOW
//
//SKILL
//
//CHECK (EVAL) (CONSIDER)
//

using UnityEngine;
using UnityEngine.Networking;
public partial class Chat
{
    [Command(channel = Channels.DefaultUnreliable)] // unimportant => unreliable
    internal void CmdEntityCommands(string command)
    {
        //REGISTER COMMANDS
        Chat.RegisterCommand("kill");
        Chat.RegisterCommand("heal");
        Chat.RegisterCommand("hide");
        Chat.RegisterCommand("show");
        Chat.RegisterCommand("skill");
        Chat.RegisterCommand("check");

        //PROCESS COMMANDS
        switch (_activeCommand.ToUpper())
        { 
            // - - - K I L L - - -
            #region KILL
            case "KILL":
            {
                //load player
                Player my = GetComponent<Player>();
                if (my == null) return;

                if (my.target == null)
                {
                    //inform user
                    my.chat.TargetMsgInfo(connectionToClient, "You must target something to kill...");
                }
                else
                {
                    //TODO Don't kill invulnerable stuff

                    //kill target
                    my.target.health = 0;

                    //inform user
                    my.chat.TargetMsgInfo(connectionToClient, "You killed " + my.target.name + ".");
                    //inform target
                    if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You were killed by " + my.name + ".");
                }

                return; //EXIT KILL
            }
            #endregion

            // - - - H E A L - - -
            #region HEAL
            case "HEAL":
                {
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    int amountHealed = 0;

                    if (!(_activeSubject.ToUpper() == "PET"))
                    {
                        amountHealed = _activeSubject.ToInt();

                        if (my.target == null)
                        {
                            //heal yourself
                            if (amountHealed <= 0)
                            {
                                amountHealed = my.healthMax - my.health;
                                my.Revive(); //resurrect
                            }
                            my.health += amountHealed; //heal

                            //inform user
                            my.chat.TargetMsgInfo(connectionToClient, "You healed yourself for " + amountHealed + " health!");
                        }
                        else
                        {
                            //heal your target
                            if (amountHealed <= 0)
                            {
                                amountHealed = my.target.healthMax - my.target.health;
                                my.target.Revive(); //resurrect
                            }
                            my.target.health += amountHealed; //heal

                            //inform user
                            my.chat.TargetMsgInfo(connectionToClient, "You healed " + my.target.name + " for " + amountHealed + " health!");
                            //inform target
                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " healed you! You gained " + amountHealed + " health!");
                        }
                    }
                    else
                    {
                        if (my.activePet != null)
                        {
                            //heal pet
                            amountHealed = my.activePet.healthMax - my.activePet.health;
                            if (amountHealed > 0) { my.activePet.health += amountHealed; }

                            //inform user
                            my.chat.TargetMsgInfo(connectionToClient, "You healed " + my.activePet.name + " for " + amountHealed + " health.");
                        }
                    }


                    return; //EXIT HEAL
                }
            #endregion

            // - - - H I D E - - -
            #region HIDE
            case "HIDE":
            {
                //load player
                Player my = GetComponent<Player>();
                if (my == null) return;

                if (my.target == null)
                {
                    //hide player
                    my.Hide();

                    //inform user
                    my.chat.TargetMsgInfo(connectionToClient, "You are now hidden.");
                }
                else
                {
                    if (my.target is Monster)
                    {
                        Monster m = (Monster)my.target;

                        //turn off respawn
                        m.respawn = false;

                        //inform user
                        my.chat.TargetMsgInfo(connectionToClient, m.name + " will no longer spawn.");
                    }
                    else
                    {
                        //hide target
                        my.target.Hide();

                        //inform user
                        my.chat.TargetMsgInfo(connectionToClient, my.target.name + " is now hidden.");
                        //inform target
                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You are now hidden.");
                    }
                }

                return; //EXIT HIDE
            }
            #endregion

            // - - - S H O W - - -
            #region SHOW
            case "SHOW":
                {
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    if (my.target == null)
                    {
                        //show player
                        my.Show();

                        //inform user
                        my.chat.TargetMsgInfo(connectionToClient, "You have been revealed.");
                    }
                    else
                    {
                        if (my.target is Monster)
                        {
                            Monster m = (Monster)my.target;

                            //turn on respawn
                            m.respawn = true;

                            //inform user
                            my.chat.TargetMsgInfo(connectionToClient, m.name + " will now spawn.");
                        }
                        else
                        {
                            //show target
                            my.target.Show();

                            //inform user
                            my.chat.TargetMsgInfo(connectionToClient, my.target.name + " has been revealed.");
                            //inform target
                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You have been revealed.");
                        }
                    }

                    return; //EXIT SHOW
                }
                #endregion

            // - - - S K I L L - - -
            #region SKILL
            case "SKILL":
                {
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    //get skill id
                    int skillSlotID = _activeSubject.ToInt();
                    skillSlotID--;

                    //use skill
                    int skillID = my.GetSkillIndexByName(player.skillbar[skillSlotID].reference);
                    my.CmdUseSkill(skillID); //use skill X

                    //inform user
                    my.chat.TargetMsgInfo(connectionToClient, "Skill Activated");

                    return; //EXIT SKILL
                }
            #endregion

            // - - - C H E C K - - -
            #region CHECK
            case "EVAL": { goto case "CHECK"; }
            case "CONSIDER": { goto case "CHECK"; }
            case "CHECK":
                {
                    string evalMessage = "\r\n"; //line break
                    //load player
                    Player my = GetComponent<Player>();
                    if (my == null) return;

                    //target name
                    string targetName = my.name;
                    //level gap
                    int levelGap = 0;
                    //health percent
                    int healthPercent = 100;

                    if (my.target == null)
                    {//Target yourself
                        evalMessage += "You examine yourself...";
                        evalMessage += "\r\n"; //line break
                        evalMessage += "You appear to be a level " + my.level + " " + my.className + ".";

                        //get health percent
                        healthPercent = Mathf.RoundToInt(my.health / my.healthMax * 100);
                    }
                    else
                    { //target your target
                        targetName = my.target.name;

                        //inform target
                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " is examining you...");

                        //show name and class
                        evalMessage += targetName + " is a level "+my.target.level+" ";
                        if (my.target is Player) { evalMessage += ((Player)my.target).className; }
                        else { evalMessage += my.target.GetType(); }

                        evalMessage += "\r\n"; //line break

                        //calculate level gap
                        levelGap = my.target.level - my.level;

                        //compare yourself to your target
                        if (levelGap > 7) {
                            evalMessage += targetName + " looks like it would crush you in a single blow...you should run!";
                        } else if (levelGap > 5) {
                            evalMessage += targetName + " looks extremely powerful";
                        } else if (levelGap > 3) {
                            evalMessage += targetName + " looks very strong";
                        } else if (levelGap > 1) {
                            evalMessage += targetName + " looks pretty tough";
                        } else if (levelGap == 0) {
                            evalMessage += targetName + " looks like an even match";
                        } else if (levelGap < -1) {
                            evalMessage += targetName + " looks a little weaker than you";
                        } else if (levelGap < -3) {
                            evalMessage += targetName + " looks extremely weak";
                        } else if (levelGap < -5) {
                            evalMessage += targetName + " looks like it has no chance to survive...pick on someone your own size!";
                        } else {
                            evalMessage += "You are not able to judge " + my.target.name + "'s power.";
                        }

                        //get health percent
                        healthPercent = Mathf.RoundToInt(my.target.health / my.target.healthMax * 100);
                    }

                    //evaluate health level
                    if (healthPercent <= 0)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " does not appear to be alive";
                    }
                    else if (healthPercent <= 10)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " is nearly dead";
                    }
                    else if (healthPercent <= 25)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " is in critical condition";
                    }
                    else if (healthPercent <= 50)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " looks pretty beat up";
                    }
                    else if (healthPercent <= 75)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " is wounded";
                    }
                    else if (healthPercent <= 99)
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " has a few scratches";
                    }
                    else
                    {
                        evalMessage += "\r\n"; //line break
                        evalMessage += targetName + " appears to be in perfect health";
                    }
                    
                    my.chat.TargetMsgInfo(connectionToClient, evalMessage);
                    return; //EXIT CHECK
                }
            #endregion
        }
    }
}