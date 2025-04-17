public interface IPoisonable
{
    void ApplyPoison(float damagePerSecond, float duration);
}

public interface ISlowable
{
    void ApplySlow(float slowPercent, float duration);
}

public interface IStunnable
{
    void ApplyStun(float duration);
}

public interface IBurnable
{
    void ApplyBurn(float duration);
}

public interface IBleedable
{
    void ApplyBleed(float duration);
}
