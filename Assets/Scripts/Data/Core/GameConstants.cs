using System;

public static class GameConstants
{
    public static string oneWayPlatform = "OneWayPlatform";
    public static string enemyBullet = "EnemyBullet";
    public static string playerBullet = "PlayerBullet";
    public static string gateEnd = "GateEnd";
}

public static class GameEvent
{
    public static Action OnPlayerDead;
    public static Action<int> OnPlayerIsShot;
    public static Action OnBossKilled;
    public static Action<CharacterColor, bool> OnEnemyKill;

    public static Action OnCompleteObjective;
    public static Action OnCompleteLevel;
    public static Action OnLevelStart;
    public static Action OnDisplayStartGate;

}
