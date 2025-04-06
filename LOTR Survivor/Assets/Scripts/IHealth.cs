using UnityEngine;

public interface IHealth
{
    int MaxHealth { get; set; }
    int Health { get; set; }

    void TakeDamage(int damage);
}
