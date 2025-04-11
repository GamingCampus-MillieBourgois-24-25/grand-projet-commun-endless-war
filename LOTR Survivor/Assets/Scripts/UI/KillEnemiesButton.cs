using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        if (BombEvent.Instance != null)
            //BombEvent.Instance.KillAllVisibleEnemies();
            BombEvent.Instance.KillAllEnemies();
    }
}
