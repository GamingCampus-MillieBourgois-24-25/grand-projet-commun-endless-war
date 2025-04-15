using UnityEngine;
using UnityEngine.VFX;

public class VFXAutoDestroy : MonoBehaviour
{
    private VisualEffect vfx;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (vfx != null && !vfx.HasAnySystemAwake())
        {
            Destroy(gameObject);
        }
    }
}
