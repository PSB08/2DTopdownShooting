using Unity.Behavior;

namespace Code.Scripts.Enemies
{
    [BlackboardEnum]
    public enum EnemyState
    {
        PATROL = 1,
        CHASE = 2,
        ATTACK = 3
    }
}