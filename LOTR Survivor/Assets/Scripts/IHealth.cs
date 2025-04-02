using UnityEngine;

public interface IHealth
{
    int MaxHealth { get; set; }
    int Health { get; set; }
    float FlashDuration { get; set; }
    Color FlashColor { get; set; }

    void OnHealthInitialized();
    void TakeDamage(int damage);
}
