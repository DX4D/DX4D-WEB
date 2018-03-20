using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccessLevel { Player, Seer, GameMaster, Admin }
public partial class Player
{
    public AccessLevel accessLevel = AccessLevel.Player;
}