using UnityEngine;

public class AttackFXAutoDestroy : MonoBehaviour
{
    [SerializeField] GameObject parent;
    public void Finish()
    {
        Destroy(parent);
    }

}