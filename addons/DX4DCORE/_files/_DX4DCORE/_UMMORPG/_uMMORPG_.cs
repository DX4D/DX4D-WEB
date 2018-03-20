using UnityEngine;

public static partial class UMMO
{
    //ENTITY
    #region IsDead
    public static bool IsDead(Entity target) { return !target.invincible && (target.state == "DEAD" || target.HealthPercent() <= 0f); }
    #endregion

    #region IsAlive
    public static bool IsAlive(Entity target) { return !IsDead(target); }
    #endregion

    #region IsIdle
    public static bool IsIdle(Entity target) { return target.state == "IDLE"; }
    #endregion

    #region IsMoving
    public static bool IsMoving(Entity target) { return target.state == "MOVING"; }
    #endregion

    #region IsCasting
    public static bool IsCasting(Entity target) { return target.state == "CASTING"; }
    #endregion
}