// - - - - - - - - - - - - - - - - -
// - - -   I N V E N T O R Y   - - -
// - - - C O M M A N D  L I S T  - -
// - - - - - - - - - - - - - - - - - 
//
//EQUIP
//
//GET
//GET GOLD
//
//CREATE
//CREATE GOLD
//
//GIVE
//GIVE GOLD
//GIVE ONLINE
//GIVE ONLINE GOLD
//GIVE US
//GIVE US GOLD
//GIVE EVERYONE
//GIVE EVERYONE GOLD
//GIVE ALL
//GIVE ALL GOLD
//
//REMOVE
//REMOVE GOLD
//REMOVE ONLINE
//REMOVE ONLINE GOLD
//REMOVE OUR
//REMOVE OUR GOLD
//REMOVE US
//REMOVE US GOLD
//REMOVE EVERYONE
//REMOVE EVERYONE GOLD
//REMOVE ALL
//REMOVE ALL GOLD
//
//TAKE
//TAKE GOLD
//
//TRADE
//TRADE GOLD
//
//STEAL
//STEAL GOLD
//
//THROW
//THROW GOLD
//
// - - - a l i a s e s - - -
//GREATE = (GET) (MAKE)
//GIVE = (ADD)
//REMOVE = (DELETE) (DEL) (REM)
//

using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public partial class Chat
{
    //This is called on the Reliable channel because Item Transactions are important
    [Command(channel = Channels.DefaultReliable)] //important => reliable
    internal void CmdInventoryCommands(string command)
    {
        //REGISTER COMMANDS
        Chat.RegisterCommand("equip");
        Chat.RegisterCommand("get");
        Chat.RegisterCommand("get gold");
        Chat.RegisterCommand("create");
        Chat.RegisterCommand("create gold");
        Chat.RegisterCommand("give");
        Chat.RegisterCommand("give gold");
        Chat.RegisterCommand("give online");
        Chat.RegisterCommand("give online gold");
        Chat.RegisterCommand("give everyone");
        Chat.RegisterCommand("give everyone gold");
        Chat.RegisterCommand("remove");
        Chat.RegisterCommand("remove gold");
        Chat.RegisterCommand("remove online");
        Chat.RegisterCommand("remove online gold");
        Chat.RegisterCommand("remove all");
        Chat.RegisterCommand("remove all gold");
        Chat.RegisterCommand("take");
        Chat.RegisterCommand("take gold");
        Chat.RegisterCommand("trade");
        Chat.RegisterCommand("trade gold");
        Chat.RegisterCommand("steal");
        Chat.RegisterCommand("steal gold");
        Chat.RegisterCommand("throw");
        Chat.RegisterCommand("throw gold");

        //PROCESS COMMANDS
                            // IMPORTANT INFO
                            ///<summary>
                            ///The itemName is declared here, this value is used by all Item based commands to retrieve items by name.
                            ///This variable is reassigned based on the number of arguments a command has. Call Trim() after setting.
                            ///</summary>
                            ///<example>
                            ///itemName = _activeSubject + " " + _activeProperty + " " + _activeValue; //1 arg commands
                            ///itemName = _activeProperty + " " + _activeValue; //2 arg commands
                            ///itemName = _activeValue; //3 arg commands
                            ///</example>
        string itemName = _activeSubject + " " + _activeProperty + " " + _activeValue;

        //The name of the type of currency your game uses
        string  currencyLabel = "coin";

        //load player
        Player my = GetComponent<Player>();
        if (my == null) return;

        // - - - - - - - - - - -
        //     3  A R G S
        // - - - - - - - - - - -
        itemName = (_activeValue).Trim(); //set item name from command - trim whitespace
        #region Three Argument Commands

        switch (_activeSubject.ToUpper())
        {
            // - - - O N L I N E - - -
            case "US": { goto case "ONLINE"; }
            case "OUR": { goto case "ONLINE"; }
            case "EVERYONEONLINE": { goto case "ONLINE"; }
            case "ONLINE":
                {
                    switch (_activeCommand.ToUpper())
                    {
                        // S T A F F
                        // - - - G I V E - - -
                        #region GIVE
                        case "ADD": { goto case "GIVE"; }
                        case "GIVE":
                            {
                                switch (_activeProperty.ToUpper())
                                {
                                    case "GOLD":
                                        {
                                            //get currency
                                            long amountToGive = _activeValue.ToLong();
                                            string currency = amountToGive + " " + _activeProperty.ToLower() + currencyLabel + "s";

                                            if (amountToGive > 0)
                                            {
                                                //add gold to all online players
                                                int i = 0;
                                                //ONLINE CHARACTERS
                                                foreach (Player p in Player.onlinePlayers.Values)
                                                {
                                                    var currentPlayer = p;

                                                    //add gold from inventory
                                                    currentPlayer.gold += amountToGive;
                                                    currentPlayer.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");
                                                    i++;

                                                    Database.CharacterSave(currentPlayer, true, false);
                                                }

                                                //save character
                                                my.chat.TargetMsgInfo(connectionToClient, "You give " + currency + " to " + i + " players");
                                            }
                                            else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                            return;
                                        }
                                    case "ITEM":
                                        {
                                            //get item
                                            ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                            if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                            Item toGive = new Item(_item);

                                            //add item to all online players
                                            int i = 0;
                                            foreach (Player p in Player.onlinePlayers.Values)
                                            {
                                                //load from online players
                                                var currentPlayer = p;

                                                //add item to pack
                                                if (currentPlayer != null && currentPlayer.InventoryAdd(toGive, 1))
                                                {
                                                    currentPlayer.chat.TargetMsgInfo(connectionToClient, itemName + " added to your backpack");
                                                    my.chat.TargetMsgInfo(connectionToClient, "Added " + itemName + " to " + currentPlayer.name);
                                                    i++;
                                                }

                                                //save character
                                                Database.CharacterSave(currentPlayer, true, false);
                                            }
                                            my.chat.TargetMsgInfo(connectionToClient, "You give " + itemName + " to " + i + " players");
                                            return;
                                        }
                                }
                                break; //exit give
                            }
                        #endregion

                        // - - - R E M O V E - - -
                        #region REMOVE
                        case "REM": { goto case "REMOVE"; }
                        case "DEL": { goto case "REMOVE"; }
                        case "DELETE": { goto case "REMOVE"; }
                        case "REMOVE":
                            {
                                switch (_activeProperty.ToUpper())
                                {
                                    case "GOLD":
                                        {
                                            //get currency
                                            long amountToTake = _activeValue.ToLong();
                                            string currency = amountToTake + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                            if (amountToTake > 0)
                                            {
                                                //remove gold from all online players
                                                foreach (Player p in Player.onlinePlayers.Values)
                                                {
                                                    p.gold -= amountToTake;
                                                    p.chat.TargetMsgInfo(connectionToClient, "You lost " + currency + ".");
                                                }

                                            }
                                            else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                            return;
                                        }
                                    case "ITEM":
                                        {
                                            //remove item
                                            ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                            if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                            Item toRemove = new Item(_item);

                                            //remove item from all online players
                                            foreach (Player p in Player.onlinePlayers.Values)
                                            {
                                                if (p.InventoryRemove(toRemove, 1))
                                                {
                                                    p.chat.TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                                                }
                                            }
                                            return;
                                        }
                                }
                                break; //exit remove
                            }
                            #endregion
                    }
                    break; //EXIT ONLINE
                }

            // - - - E V E R Y O N E - - -
            case "ALL": { goto case "EVERYONE"; }
            case "EVERYONE":
                {
                    // - - - - - - - - - - -
                    //     3  A R G S
                    // - - - - - - - - - - -
                    switch (_activeCommand.ToUpper())
                    {
                        // S T A F F
                        // - - - G I V E  A L L - - -
                        #region GIVE ALL
                        case "ADD": { goto case "GIVE"; }
                        case "GIVE":
                            {//GIVE ALL
                                switch (_activeProperty.ToUpper())
                                {
                                    case "GOLD":
                                        {
                                            //load player classes
                                            List<Player> playerClasses = new List<Player>();
                                            foreach (GameObject prefab in NetworkManager.singleton.spawnPrefabs)
                                            {
                                                Player selectedClass = prefab.GetComponent<Player>();
                                                if (selectedClass != null)
                                                {
                                                    playerClasses.Add(selectedClass);
                                                }
                                            }

                                            //get currency
                                            long amountToGive = _activeValue.ToLong();
                                            string currency = amountToGive + " " + _activeProperty.ToLower() + currencyLabel + "s";

                                            //give items to all players
                                            int i = 0;
                                            if (amountToGive > 0)
                                            {
                                                //ONLINE CHARACTERS
                                                foreach (Player p in Player.onlinePlayers.Values)
                                                {
                                                    var currentPlayer = p;

                                                    //remove gold from inventory
                                                    currentPlayer.gold += amountToGive;
                                                    currentPlayer.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");
                                                    i++;

                                                    Database.CharacterSave(currentPlayer, true, false);
                                                }

                                                //OFFLINE CHARACTERS
                                                foreach (string name in Database.AllCharacters())
                                                {
                                                    if (!Player.onlinePlayers.ContainsKey(name))
                                                    {
                                                        GameObject o = Database.CharacterLoad(name, playerClasses);
                                                        var currentPlayer = o.GetComponent<Player>();

                                                        //remove gold from inventory
                                                        currentPlayer.gold += amountToGive;
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");
                                                        i++;

                                                        Database.CharacterSave(currentPlayer, false, false);
                                                        Destroy(currentPlayer.gameObject);
                                                    }
                                                }
                                            }
                                            else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                            //success message
                                            my.chat.TargetMsgInfo(connectionToClient, "You gave " + currency + " to " + i + " players. Total: " + (amountToGive * i));

                                            return;
                                        }
                                    case "ITEM":
                                        {//GIVE ALL
                                            //load player classes
                                            List<Player> playerClasses = new List<Player>();
                                            foreach (GameObject prefab in NetworkManager.singleton.spawnPrefabs)
                                            {
                                                Player selectedClass = prefab.GetComponent<Player>();
                                                if (selectedClass != null)
                                                {
                                                    playerClasses.Add(selectedClass);
                                                }
                                            }

                                            //get item
                                            ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                            if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                            Item toGive = new Item(_item);


                                            //player lists
                                            //List<Player> onlineCharacters = Player.onlinePlayers.Values.ToList<Player>();
                                            //List<Player> offlineCharacters = new List<Player>();


                                            //give items to all players
                                            int i = 0;
                                            foreach (string name in Database.AllCharacters())
                                            {
                                                //load character
                                                bool isOnline = Player.onlinePlayers.ContainsKey(name);

                                                if (isOnline)
                                                {
                                                    //load from online players
                                                    var currentPlayer = Player.onlinePlayers[name];

                                                    //add to pack
                                                    if (currentPlayer != null && currentPlayer.InventoryAdd(toGive, 1))
                                                    {
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, itemName + " added to your backpack");
                                                        my.chat.TargetMsgInfo(connectionToClient, "Added " + itemName + " to " + currentPlayer.name);
                                                        i++;
                                                    }

                                                    Database.CharacterSave(currentPlayer, isOnline, false);
                                                }
                                                else
                                                {
                                                    //load from database
                                                    GameObject o = Database.CharacterLoad(name, playerClasses);
                                                    var currentPlayer = o.GetComponent<Player>();

                                                    //add to pack
                                                    if (currentPlayer != null && currentPlayer.InventoryAdd(toGive, 1))
                                                    {
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, itemName + " added to your backpack");
                                                        my.chat.TargetMsgInfo(connectionToClient, "Added " + itemName + " to " + currentPlayer.name);
                                                        i++;
                                                    }

                                                    Database.CharacterSave(currentPlayer, isOnline, false);
                                                    Destroy(currentPlayer.gameObject);
                                                }
                                            }
                                            //success message
                                            my.chat.TargetMsgInfo(connectionToClient, "You gave " + itemName + " to " + i + " players");

                                            return;
                                        }
                                    default:
                                        {
                                            //Perform the default action if a Property is not specified
                                            itemName = (_activeProperty + " " + _activeValue).Trim();
                                            if (Utils.IsNullOrWhiteSpace(itemName)) break; //no command args
                                            goto case "ITEM"; //the default action
                                        }
                                }
                                break; //exit give
                            }
                        #endregion

                        // - - - R E M O V E  A L L - - -
                        #region REMOVE ALL
                        case "REM": { goto case "REMOVE"; }
                        case "DEL": { goto case "REMOVE"; }
                        case "DELETE": { goto case "REMOVE"; }
                        case "REMOVE":
                            {//REMOVE ALL
                                switch (_activeProperty.ToUpper())
                                {
                                    case "GOLD":
                                        {
                                            //load player classes
                                            List<Player> playerClasses = new List<Player>();
                                            foreach (GameObject prefab in NetworkManager.singleton.spawnPrefabs)
                                            {
                                                Player selectedClass = prefab.GetComponent<Player>();
                                                if (selectedClass != null)
                                                {
                                                    playerClasses.Add(selectedClass);
                                                }
                                            }

                                            //get currency
                                            long amountToTake = _activeValue.ToLong();
                                            string currency = amountToTake + " " + _activeProperty.ToLower() + currencyLabel + "s";

                                            //remove gold from all online players
                                            int i = 0;
                                            if (amountToTake > 0)
                                            {
                                                //ONLINE CHARACTERS
                                                foreach (Player p in Player.onlinePlayers.Values)
                                                {
                                                    var currentPlayer = p;

                                                    //remove gold from inventory
                                                    long toRemove = amountToTake;
                                                    //only take what they have
                                                    if (toRemove > currentPlayer.gold)
                                                    {
                                                        toRemove = currentPlayer.gold;
                                                    }

                                                    currentPlayer.gold -= toRemove;
                                                    currentPlayer.chat.TargetMsgInfo(connectionToClient, "You lost " + toRemove + " " + _activeProperty + currencyLabel + "s.");
                                                    i++;

                                                    Database.CharacterSave(currentPlayer, true, false);
                                                }

                                                //OFFLINE CHARACTERS
                                                foreach (string name in Database.AllCharacters())
                                                {
                                                    if (!Player.onlinePlayers.ContainsKey(name))
                                                    {
                                                        GameObject o = Database.CharacterLoad(name, playerClasses);
                                                        var currentPlayer = o.GetComponent<Player>();

                                                        long toRemove = amountToTake;

                                                        //only take what they have
                                                        if (toRemove > currentPlayer.gold) { toRemove = currentPlayer.gold; }

                                                        //remove gold from inventory
                                                        currentPlayer.gold -= toRemove;
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, "You lost " + toRemove + " " + _activeProperty + currencyLabel + "s.");
                                                        i++;

                                                        Database.CharacterSave(currentPlayer, false, false);
                                                        Destroy(currentPlayer.gameObject);
                                                    }
                                                }
                                            }
                                            else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                            //success message
                                            my.chat.TargetMsgInfo(connectionToClient, "You took " + currency + " from " + i + " players. Total: " + (amountToTake * i));
                                            return;
                                        }
                                    case "ITEM":
                                        {//REMOVE ALL
                                            //load player classes
                                            List<Player> playerClasses = new List<Player>();
                                            foreach (GameObject prefab in NetworkManager.singleton.spawnPrefabs)
                                            {
                                                Player selectedClass = prefab.GetComponent<Player>();
                                                if (selectedClass != null)
                                                {
                                                    playerClasses.Add(selectedClass);
                                                }
                                            }

                                            //remove item
                                            ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                            if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                            Item toRemove = new Item(_item);

                                            //remove item from all online players
                                            //ONLINE CHARACTERS
                                            foreach (Player p in Player.onlinePlayers.Values)
                                            {
                                                var currentPlayer = p;

                                                //remove item from inventory
                                                if (currentPlayer != null && currentPlayer.InventoryRemove(toRemove, 1))
                                                {
                                                    currentPlayer.chat.TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                                                }

                                                Database.CharacterSave(currentPlayer, true, false);
                                            }

                                            //OFFLINE CHARACTERS
                                            foreach (string name in Database.AllCharacters())
                                            {
                                                if (!Player.onlinePlayers.ContainsKey(name))
                                                {
                                                    GameObject o = Database.CharacterLoad(name, playerClasses);
                                                    var currentPlayer = o.GetComponent<Player>();

                                                    //remove item from inventory
                                                    if (currentPlayer != null && currentPlayer.InventoryRemove(toRemove, 1))
                                                    {
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                                                    }

                                                    Database.CharacterSave(currentPlayer, false, false);
                                                    Destroy(currentPlayer.gameObject);
                                                }
                                            }
                                            return;
                                        }
                                    default:
                                        {
                                            //Perform the default action if a Property is not specified
                                            itemName = (_activeProperty + " " + _activeValue).Trim();
                                            if (Utils.IsNullOrWhiteSpace(itemName)) break; //no command args
                                            goto case "ITEM"; //the default action
                                        }
                                }
                                break; //exit remove
                            }
                        #endregion
                    }
                    break; //EXIT ONLINE
                }
        }
        #endregion

        // - - - - - - - - - - -
        //     2  A R G S
        // - - - - - - - - - - -
        itemName = (_activeProperty + " " + _activeValue).Trim(); //set item name from command - trim whitespace
        #region Two Argument Commands

        switch (_activeSubject.ToUpper())
        {
            // - - - G O L D - - -
            case "GOLD":
                {
                    switch (_activeCommand.ToUpper())
                    {
                        // - - - G E T - - -
                        // - - - C R E A T E - - -
                        #region CREATE
                        case "GET": { goto case "CREATE"; } //There should not be a /get item
                        case "MAKE": { goto case "CREATE"; }
                        case "CREATE":
                            {
                                //get currency
                                long amountToGet = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToGet + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToGet > 0)
                                {
                                    //add gold to player
                                    my.gold += amountToGet;
                                    my.chat.TargetMsgInfo(connectionToClient, "You created " + currency + ".");
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                return;
                            }
                        #endregion

                        // - - - G I V E - - -
                        #region GIVE
                        case "ADD": { goto case "GIVE"; }
                        case "GIVE":
                            {
                                //get currency
                                long amountToGive = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToGive + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToGive > 0)
                                {
                                    if (my.target == null)
                                    {
                                        //add gold to player
                                        my.gold += amountToGive;
                                        my.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");
                                    }
                                    else
                                    {
                                        //add loot to monster
                                        if(my.target is Monster) {
                                            Monster m = my.target as Monster;
                                            m.lootGoldMin += (int)amountToGive;
                                            m.lootGoldMax += (int)amountToGive;
                                            my.chat.TargetMsgInfo(connectionToClient, m.name + " loot set to " + m.lootGoldMin + "-" + m.lootGoldMax + ".");
                                        } else {
                                            //add gold to target
                                            my.target.gold += amountToGive;
                                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + currency + ".");
                                        }
                                    }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                return;
                            }
                        #endregion

                        // - - - R E M O V E - - -
                        #region REMOVE
                        case "REM": { goto case "REMOVE"; }
                        case "DEL": { goto case "REMOVE"; }
                        case "DELETE": { goto case "REMOVE"; }
                        case "REMOVE":
                            {
                                //get currency
                                long amountToRemove = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToRemove + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToRemove > 0)
                                {
                                    if (my.target == null)
                                    {
                                        //don't take more than you have
                                        if (my.gold < amountToRemove) { amountToRemove = my.gold; }

                                        //remove gold from player
                                        my.gold -= amountToRemove;
                                        my.chat.TargetMsgInfo(connectionToClient, "You lost " + currency + ".");
                                    }
                                    else
                                    {
                                        //don't take more than you have
                                        if (my.target.gold < amountToRemove) { amountToRemove = my.gold; }

                                        //remove gold from target
                                        my.target.gold -= amountToRemove;
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You lost " + currency + ".");
                                    }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }

                                return;
                            }
                        #endregion

                        // - - - T A K E - - -
                        #region TAKE
                        case "TAKE":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get currency
                                long amountToTake = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToTake + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToTake > 0)
                                {
                                    if (my.target.gold >= amountToTake)
                                    {
                                        //take gold from target
                                        my.target.gold -= amountToTake;
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " took " + currency + ".");

                                        //put gold into your pack
                                        my.gold += amountToTake;
                                        my.chat.TargetMsgInfo(connectionToClient, "You took " + currency + " from " + my.target.name + ".");
                                    }
                                    else {
                                        //take what they have
                                        if (my.target.gold < amountToTake) { amountToTake = my.target.gold; }

                                        //inform user
                                        my.chat.TargetMsgInfo(connectionToClient, my.target.name + " doesn't have " + currency + " to take, so you take " + amountToTake + " " + _activeSubject + currencyLabel + "s instead.");
                                    }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }
                                return;
                            }
                        #endregion
                            
                        // - - - T R A D E - - -
                        #region TRADE
                        case "TRADE":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get currency
                                long amountToTrade = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToTrade + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToTrade > 0)
                                {
                                    if (my.gold >= amountToTrade)
                                    {
                                        //remove gold from pack
                                        my.gold -= amountToTrade;
                                        my.chat.TargetMsgInfo(connectionToClient, "You gave " + currency + " to " + my.target.name + ".");

                                        //give gold to target
                                        my.target.gold += amountToTrade;
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + currency + ".");
                                    }
                                    else { my.chat.TargetMsgInfo(connectionToClient, "You don't have " + currency + " to give."); }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }
                                return;
                            }
                        #endregion

                        // - - - S T E A L - - -
                        #region STEAL
                        case "STEAL":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get currency
                                long amountToTake = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToTake + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                //steal
                                bool detected = true;

                                if (amountToTake > 0)
                                {
                                    if (my.target.gold >= amountToTake)
                                    {
                                        //take gold from target
                                        my.target.gold -= amountToTake;
                                        if (detected)
                                        {
                                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " stole " + currency + " from you.");
                                        }
                                        else
                                        {
                                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "Your purse feels a little lighter...");
                                        }

                                        //put gold into your pack
                                        my.gold += amountToTake;
                                        my.chat.TargetMsgInfo(connectionToClient, "You stole " + currency + " from " + my.target.name + ".");
                                    }
                                    else
                                    {
                                        //steal what they have
                                        if (my.target.gold < amountToTake) { amountToTake = my.target.gold; }

                                        //inform user
                                        my.chat.TargetMsgInfo(connectionToClient, my.target.name + " doesn't have " + currency + ", so you steal " + amountToTake + " " + _activeSubject + currencyLabel + "s instead.");
                                    }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }
                                return;
                            }
                        #endregion

                        // - - - T H R O W - - -
                        #region THROW
                        case "THROW":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //throw damage multiplier
                                //float throwDamageMultiplier = 1f;

                                //get currency
                                long amountToThrow = _activeProperty.ToLong() + _activeValue.ToLong();
                                string currency = amountToThrow + " " + _activeSubject.ToLower() + " " + currencyLabel + "s";

                                if (amountToThrow > 0)
                                {
                                    if (my.gold >= amountToThrow)
                                    {
                                        //remove gold from your pack
                                        my.gold -= amountToThrow;
                                        my.chat.TargetMsgInfo(connectionToClient, "You threw " + currency + " at " + my.target.name + ".");

                                        //add gold to target's pack
                                        my.target.gold += amountToThrow;
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " throws a handful of " + currencyLabel + "s in your face.");

                                        //deal damage
                                        //my.target.health -= (int)(amountToThrow); // * throwDamageMultiplier
                                        my.DealDamageAt(my.target, (int)amountToThrow);
                                    }
                                    else { my.chat.TargetMsgInfo(connectionToClient, "You don't have " + currency + " to throw."); }
                                }
                                else { my.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }
                                return;
                            }
                        #endregion
                    }
                    break; //EXIT GOLD
                }

            // - - - I T E M - - -
            case "ITEM":
                {
                    switch (_activeCommand.ToUpper())
                    {
                        // - - - G E T - - -
                        #region GET
                        case "GET":
                            {
                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toGet = new Item(_item);

                                //add item to player
                                if (my.InventoryAdd(toGet, 1))
                                {
                                    //equip item
                                    my.CmdUseInventoryItem(my.GetInventoryIndexByName(itemName));

                                    //inform user
                                    my.chat.TargetMsgInfo(connectionToClient, "You pulled " + itemName + " from thin air.");
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be created.");
                                }
                                return;
                            }
                        #endregion

                        // - - - E Q U I P - - -
                        #region EQUIP
                        case "EQUIP":
                            {
                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toEquip = new Item(_item);
                                //ItemSlot slot = new ItemSlot(toEquip, 1);

                                //equip item
                                my.CmdUseInventoryItem(my.GetInventoryIndexByName(toEquip.name));
                                bool wasEquipped = true; //my.GetEquipmentIndexByName(itemName)

                                if (wasEquipped)
                                {
                                    //inform user
                                    my.chat.TargetMsgInfo(connectionToClient, "You equipped " + itemName + ".");
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be equipped.");
                                }

                                return;
                            }
                        #endregion

                        // - - - C R E A T E - - -
                        #region CREATE
                        case "MAKE": { goto case "CREATE"; }
                        case "CREATE":
                            {
                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toGet = new Item(_item);

                                //add item to player
                                if (my.InventoryAdd(toGet, 1))
                                {
                                    my.chat.TargetMsgInfo(connectionToClient,"You created " + itemName + ".");
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be created.");
                                }
                                return;
                            }
                        #endregion

                        // - - - G I V E - - -
                        #region GIVE
                        case "ADD": { goto case "GIVE"; }
                        case "GIVE":
                            {
                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toGive = new Item(_item);

                                if (my.target == null)
                                {
                                    //add item to player
                                    if (my.InventoryAdd(toGive, 1))
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " was added to your backpack");
                                    }
                                    else
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be added to your backpack.");
                                    }
                                }
                                else
                                {
                                    //add item to target
                                    if (my.target.InventoryAdd(toGive, 1))
                                    {
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, itemName + " was added to your backpack");
                                    }
                                    else
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be given to " + my.target.name);
                                    }
                                }
                                return;
                            }
                        #endregion

                        // - - - R E M O V E - - -
                        #region REMOVE
                        case "REM": { goto case "REMOVE"; }
                        case "DEL": { goto case "REMOVE"; }
                        case "DELETE": { goto case "REMOVE"; }
                        case "REMOVE":
                            {
                                //remove item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toRemove = new Item(_item);

                                if (my.target == null)
                                {
                                    //remove item from player
                                    if (my.InventoryRemove(toRemove, 1))
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                                    }
                                    else
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be removed from " + my.name + "'s backpack");
                                    }
                                }
                                else
                                {
                                    //remove item from target
                                    if (my.target.InventoryRemove(toRemove, 1))
                                    {
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                                    }
                                    else
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be removed from " + my.target.name + "'s backpack");
                                    }
                                }
                                return;
                            }
                        #endregion
                            
                        // - - - T R A D E - - -
                        #region TRADE
                        case "TRADE":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toTrade = new Item(_item);

                                //remove from pack
                                if (my.InventoryRemove(toTrade, 1))
                                {
                                    //give to target
                                    if (my.target.InventoryAdd(toTrade, 1))
                                    {
                                        my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + itemName + ".");
                                        my.chat.TargetMsgInfo(connectionToClient, "You gave " + itemName + " to " + my.target.name + ".");
                                    }
                                    else
                                    {
                                        //return to inventory
                                        if (my.target.InventoryAdd(toTrade, 1))
                                        {
                                            my.chat.TargetMsgInfo(connectionToClient, "Trade with " + my.target.name + " canceled. " + itemName + " was returned to your backpack.");
                                        }
                                    }
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be traded to " + my.target.name + ".");
                                }
                                return;
                            }
                        #endregion

                        // - - - T A K E - - -
                        #region TAKE
                        case "TAKE":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toTake = new Item(_item);

                                //remove from target's pack
                                if (my.target.InventoryRemove(toTake, 1))
                                {
                                    //put in your pack
                                    if (my.InventoryAdd(toTake, 1))
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, "You took " + itemName + " from " + my.target.name + ".");
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " took " + itemName + " from you.");
                                    }
                                    else
                                    {
                                        //return to inventory
                                        if (my.target.InventoryAdd(toTake, 1))
                                        {
                                            my.chat.TargetMsgInfo(connectionToClient, "You failed to take " + itemName + " from " + my.target.name + ".");
                                        }
                                    }
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, "You failed to take " + itemName + " from " + my.target.name + ".");
                                }
                                return;
                            }
                        #endregion

                        // - - - S T E A L - - -
                        #region STEAL
                        case "STEAL":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toTake = new Item(_item);

                                bool detected = true;

                                //remove from target's pack
                                if (my.target.InventoryRemove(toTake, 1))
                                {
                                    //put in your pack
                                    if (my.InventoryAdd(toTake, 1))
                                    {
                                        my.chat.TargetMsgInfo(connectionToClient, "You stole " + itemName + " from " + my.target.name + ".");
                                        if (detected)
                                        {
                                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " stole " + itemName + " from you.");
                                        }
                                        else
                                        {
                                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You feel like you are missing something...");
                                        }
                                    }
                                    else
                                    {
                                        //return to inventory
                                        if (my.target.InventoryAdd(toTake, 1))
                                        {
                                            my.chat.TargetMsgInfo(connectionToClient, "You failed to steal " + itemName + " from " + my.target.name + ".");
                                        }
                                    }
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, "You can't steal " + itemName + " from " + my.target.name + ".");
                                }
                                return;
                            }
                        #endregion

                        // - - - T H R O W - - -
                        #region THROW
                        case "THROW":
                            {
                                //we need a target
                                if (my.target == null) return;

                                //get item
                                ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                                if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                                Item toThrow = new Item(_item);

                                //remove from pack
                                if (my.InventoryRemove(toThrow, 1))
                                {
                                    //throw at target
                                    if (my.target.InventoryAdd(toThrow, 1))
                                    {
                                        my.DealDamageAt(my.target, (int)toThrow.sellPrice);
                                        if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + itemName + ".");
                                        my.chat.TargetMsgInfo(connectionToClient, "You threw " + itemName + " at " + my.target.name + ".");
                                    }
                                    else
                                    {
                                        //return to inventory
                                        if (my.target.InventoryAdd(toThrow, 1))
                                        {
                                            my.chat.TargetMsgInfo(connectionToClient, "Throw canceled. " + itemName + " was returned to your backpack.");
                                        }
                                    }
                                }
                                else
                                {
                                    my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be thrown at " + my.target.name + ".");
                                }
                                return;
                            }
                        #endregion
                    }
                    break; //EXIT ITEM
                }
        }
        #endregion

        // - - - - - - - - - - -
        //       1  A R G
        // - - - - - - - - - - -
        itemName = (_activeSubject + " " + _activeProperty + " " + _activeValue).Trim(); //set item name from command - trim whitespace
        #region Single Argument Commands

        switch (_activeCommand.ToUpper())
        {
            // - - - G E T - - -
            #region GET
            case "GET":
                {
                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toGet = new Item(_item);

                    //add item to player
                    if (my.InventoryAdd(toGet, 1))
                    {
                        //equip item
                        my.CmdUseInventoryItem(my.GetInventoryIndexByName(itemName));

                        //inform user
                        my.chat.TargetMsgInfo(connectionToClient, "You pulled " + itemName + " from thin air.");
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be created.");
                    }
                    return;
                }
            #endregion

            // - - - E Q U I P - - -
            #region EQUIP
            case "EQUIP":
                {
                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toEquip = new Item(_item);
                    //ItemSlot slot = new ItemSlot(toEquip, 1);

                    //equip item
                    my.CmdUseInventoryItem(my.GetInventoryIndexByName(toEquip.name));
                    bool wasEquipped = true; //my.GetEquipmentIndexByName(itemName)

                    if (wasEquipped) {
                        //inform user
                        my.chat.TargetMsgInfo(connectionToClient, "You equipped " + itemName + ".");
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be equipped.");
                    }

                    return;
                }
                #endregion

            // - - - C R E A T E - - -
            #region CREATE
            case "MAKE": { goto case "CREATE"; }
            case "CREATE":
                {
                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toGet = new Item(_item);

                    //add item to player
                    if (my.InventoryAdd(toGet, 1))
                    {
                        my.chat.TargetMsgInfo(connectionToClient, "You created " + itemName + ".");
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be created.");
                    }
                    return;
                }
            #endregion

            // - - - G I V E - - -
            #region GIVE
            case "ADD": { goto case "GIVE"; }
            case "GIVE":
                {
                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toGive = new Item(_item);

                    if (my.target == null)
                    {
                        //add item to player
                        if (my.InventoryAdd(toGive, 1))
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " was added to your backpack");
                        }
                        else
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be added to your backpack.");
                        }
                    }
                    else
                    {
                        //add item to target
                        if (my.target.InventoryAdd(toGive, 1))
                        {
                            my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, itemName + " was added to your backpack");
                        }
                        else
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be given to " + my.target.name);
                        }
                    }
                    return;
                }
            #endregion

            // - - - R E M O V E - - -
            #region REMOVE
            case "REM": { goto case "REMOVE"; }
            case "DEL": { goto case "REMOVE"; }
            case "DELETE": { goto case "REMOVE"; }
            case "REMOVE":
                {
                    //remove item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toRemove = new Item(_item);

                    if (my.target == null)
                    {
                        //remove item from player
                        if (my.InventoryRemove(toRemove, 1))
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                        }
                        else
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be removed from " + my.name + "'s backpack");
                        }
                    }
                    else
                    {
                        //remove item from target
                        if (my.target.InventoryRemove(toRemove, 1))
                        {
                            my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, itemName + " was removed from your backpack");
                        }
                        else
                        {
                            my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be removed from " + my.target.name + "'s backpack");
                        }
                    }
                    return;
                }
            #endregion

            // - - - T R A D E - - -
            #region TRADE
            case "TRADE":
                {
                    //we need a target
                    if (my.target == null) return;

                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toTrade = new Item(_item);

                    //remove from pack
                    if (my.InventoryRemove(toTrade, 1))
                    {
                        //give to target
                        if (my.target.InventoryAdd(toTrade, 1))
                        {
                            my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + itemName + ".");
                            my.chat.TargetMsgInfo(connectionToClient, "You gave " + itemName + " to " + my.target.name + ".");
                        }
                        else
                        {
                            //return to inventory
                            if (my.target.InventoryAdd(toTrade, 1))
                            {
                                my.chat.TargetMsgInfo(connectionToClient, "Trade with " + my.target.name + " canceled. " + itemName + " was returned to your backpack.");
                            }
                        }
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be traded to " + my.target.name + ".");
                    }
                    return;
                }
            #endregion

            // - - - T A K E - - -
            #region TAKE
            case "TAKE":
                {
                    //we need a target
                    if (my.target == null) return;

                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toTake = new Item(_item);

                    //remove from target's pack
                    if (my.target.InventoryRemove(toTake, 1))
                    {
                        //put in your pack
                        if (my.InventoryAdd(toTake, 1))
                        {
                            my.chat.TargetMsgInfo(connectionToClient, "You took " + itemName + " from " + my.target.name + ".");
                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " took " + itemName + " from you.");
                        }
                        else
                        {
                            //return to inventory
                            if (my.target.InventoryAdd(toTake, 1))
                            {
                                my.chat.TargetMsgInfo(connectionToClient, "You failed to take " + itemName + " from " + my.target.name + ".");
                            }
                        }
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, "You failed to take " + itemName + " from " + my.target.name + ".");
                    }
                    return;
                }
            #endregion

            // - - - S T E A L - - -
            #region STEAL
            case "STEAL":
                {
                    //we need a target
                    if (my.target == null) return;

                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toTake = new Item(_item);

                    bool detected = true;

                    //remove from target's pack
                    if (my.target.InventoryRemove(toTake, 1))
                    {
                        //put in your pack
                        if (my.InventoryAdd(toTake, 1))
                        {
                            my.chat.TargetMsgInfo(connectionToClient, "You stole " + itemName + " from " + my.target.name + ".");
                            if (detected)
                            {
                                if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " stole " + itemName + " from you.");
                            }
                            else
                            {
                                if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, "You feel like you are missing something...");
                            }
                        }
                        else
                        {
                            //return to inventory
                            if (my.target.InventoryAdd(toTake, 1))
                            {
                                my.chat.TargetMsgInfo(connectionToClient, "You failed to steal " + itemName + " from " + my.target.name + ".");
                            }
                        }
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, "You can't steal " + itemName + " from " + my.target.name + ".");
                    }
                    return;
                }
            #endregion

            // - - - T H R O W - - -
            #region THROW
            case "THROW":
                {
                    //we need a target
                    if (my.target == null) return;

                    //get item
                    ItemTemplate _item = ItemTemplate.dict[itemName.GetStableHashCode()];
                    if (_item == null) { my.chat.TargetMsgInfo(connectionToClient, itemName + " does not exist. Keep in mind that item names are case sensitive."); return; }
                    Item toThrow = new Item(_item);

                    //remove from pack
                    if (my.InventoryRemove(toThrow, 1))
                    {
                        //throw at target
                        if (my.target.InventoryAdd(toThrow, 1))
                        {
                            my.DealDamageAt(my.target, (int)toThrow.sellPrice);
                            if (my.target is Player) my.target.GetComponent<Chat>().TargetMsgInfo(connectionToClient, my.name + " gave you " + itemName + ".");
                            my.chat.TargetMsgInfo(connectionToClient, "You threw " + itemName + " at " + my.target.name + ".");
                        }
                        else
                        {
                            //return to inventory
                            if (my.target.InventoryAdd(toThrow, 1))
                            {
                                my.chat.TargetMsgInfo(connectionToClient, "Throw canceled. " + itemName + " was returned to your backpack.");
                            }
                        }
                    }
                    else
                    {
                        my.chat.TargetMsgInfo(connectionToClient, itemName + " could not be thrown at " + my.target.name + ".");
                    }
                    return;
                }
                #endregion

        }
        #endregion
    }
}


/*
// - - - C R E A T E - - -
#region CREATE
case "GET": { goto case "CREATE"; }
case "MAKE": { goto case "CREATE"; }
case "CREATE":
    {
        if(_ itemName = _activeSubject + " " + _activeProperty + " " + _activeValue;
        break;
    }
#endregion
// - - - T A K E - - -
#region TAKE
case "TAKE":
    {
        itemName = _activeSubject + " " + _activeProperty + " " + _activeValue;
        break;
    }
#endregion
// - - - S T E A L - - -
#region STEAL
case "STEAL":
    {
        itemName = _activeSubject + " " + _activeProperty + " " + _activeValue;
        break;
    }
#endregion
// - - - T H R O W - - -
#region THROW
case "THROW":
    {
        itemName = _activeSubject + " " + _activeProperty + " " + _activeValue;
        break;
    }
    #endregion
*/
/*
bool isOnline = Player.onlinePlayers.ContainsKey(name);
if (isOnline)
{
    //load from online players
    var currentPlayer = Player.onlinePlayers[name];
    if (currentPlayer != null)
    {
        currentPlayer.gold += amountToGive;
        currentPlayer.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");

        Database.CharacterSave(currentPlayer, true, false); //param2 = isOnline
        i++;
    }
}
else
{
    //load from database
    GameObject o = Database.CharacterLoad(name, playerClasses);
    var currentPlayer = o.GetComponent<Player>();
    if (currentPlayer != null)
    {
        currentPlayer.gold += amountToGive;
        currentPlayer.chat.TargetMsgInfo(connectionToClient, "You got " + currency + ".");

        Database.CharacterSave(currentPlayer, false, false); //param2 = isOnline
        Destroy(currentPlayer.gameObject);
        i++;
    }
}*/
/*
                                            foreach (string name in Database.AllCharacters())
                                            {

                                                if (!onlineCharacters.Contains(currentPlayer))
                                                {
                                                    //load character
                                                    GameObject o = Database.CharacterLoad(name, playerClasses);
                                                    offlineCharacters.Add(o.GetComponent<Player>());
                                                }
                                            }

                                                //add item to inventory
                                                if (currentPlayer != null)
                                                {
                                                    
                                                    
                                                }

                                                //save character
                                                Database.CharacterSave(currentPlayer, isOnline, false);
                                                Destroy(currentPlayer.gameObject);
                                            */
/*
                                            if (amountToGive > 0)
                                            {
                                                //get player classes
                                                List<Player> playerClasses = new List<Player>();
                                                foreach( GameObject prefab in NetworkManager.singleton.spawnPrefabs)
                                                {
                                                    Player selectedClass = prefab.GetComponent<Player>();
                                                    if (selectedClass != null)
                                                    {
                                                        playerClasses.Add(selectedClass);
                                                    }
                                                }

                                                //give gold to all players
                                                int i = 0;
                                                Player currentPlayer = null;
                                                foreach ( string name in Database.AllCharacters())
                                                {
                                                    //load character
                                                    GameObject o = Database.CharacterLoad(name, playerClasses);
                                                    currentPlayer = o.GetComponent<Player>();
                                                    
                                                    //add gold
                                                    if(currentPlayer != null)
                                                    {
                                                        currentPlayer.gold += amountToGive;
                                                        currentPlayer.chat.TargetMsgInfo(connectionToClient, currency + " added to your backpack");
                                                        i++;
                                                        me.chat.TargetMsgInfo(connectionToClient, "Added " + currency + " to " + name);
                                                    }
                                                    //save character
                                                    bool isOnline = Player.onlinePlayers.ContainsKey(name);
                                                    Database.CharacterSave(currentPlayer, isOnline, false);
                                                    Destroy(currentPlayer.gameObject);
                                                }

                                                //success message
                                                me.chat.TargetMsgInfo(connectionToClient, "You gave "+currency+" to "+i+" players");
                                            }
                                            else { me.chat.TargetMsgInfo(connectionToClient, "You must specify a value greater than zero"); }
                                            */
