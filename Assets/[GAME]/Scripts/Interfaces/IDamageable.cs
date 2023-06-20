using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser);
    }
}
