using Unity.Behavior;

namespace Code.Scripts.Enemies
{
    [BlackboardEnum]
    public enum EnemyState
    {
        CHASE = 1,
        ATTACK = 2,
        HIT = 3,
        DEAD = 4,
        
    }
}