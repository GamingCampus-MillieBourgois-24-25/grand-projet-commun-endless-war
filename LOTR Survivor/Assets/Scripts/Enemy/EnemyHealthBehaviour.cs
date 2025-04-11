using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Enemy Data")]
    [SerializeField] public EnemySO enemyData;

    [Header("XP Pickup")]
    [SerializeField] public GameObject xpPrefab;

    [Header("Health Pickup")]
    [SerializeField] public GameObject healthPrefab;

    private int health;
    private Renderer objectRenderer;
    private Color originalColor;

    private static int killCounter = 0;
    private static int killsForHealthPickup = 15;

    public int MaxHealth { get => enemyData != null ? (int)enemyData.maxHealth : 100; set => enemyData.maxHealth = value; }
    public int Health { get => health; set => health = value; }
    public float FlashDuration { get => enemyData != null ? enemyData.flashDuration : 0.1f; set => enemyData.flashDuration = value; }
    public Color FlashColor { get => enemyData != null ? enemyData.flashColor : Color.red; set => enemyData.flashColor = value; }

    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy Data non assigné sur " + gameObject.name);
            return;
        }

        Health = MaxHealth;
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        OnHealthInitialized();
    }

    public void OnHealthInitialized() { }

    public void Initialize(EnemySO enemySO)
    {
        this.enemyData = enemySO;
        Health = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = FlashColor;
            yield return new WaitForSeconds(FlashDuration);
            objectRenderer.material.color = originalColor;
        }
    }

    private void DestroyEnemy()
    {
        killCounter++;

        if (xpPrefab != null)
        {
            if (ObjectPool.Instance != null)
            {
                GameObject xp = ObjectPool.Instance.Spawn(xpPrefab, transform.position, Quaternion.identity);
            }
        }
        if (killCounter % killsForHealthPickup == 0 && healthPrefab != null)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }


        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, enemyData.prefab);
        }
        else
        {
            Debug.Log("ObjectPool Instance is not present in the scene!");
            Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        Debug.Log($"{gameObject.name} -> OnEnable()");
        if (BombEvent.Instance != null)
        {
            //BombEvent.Instance.OnKillAllVisibleEnemies += KillIfVisible;
            BombEvent.Instance.OnKillAllEnemies += KillFromEvent;
        }
    }

    void OnDisable()
    {
        if (BombEvent.Instance != null)
            //BombEvent.Instance.OnKillAllVisibleEnemies -= KillIfVisible;
            BombEvent.Instance.OnKillAllEnemies += KillFromEvent;
    }

    private void KillFromEvent()
    {
        DestroyEnemy();
    }

    //Système non fonctionnel pour kill que les ennemies présent sur l'écran
    /*private void KillIfVisible()
    {
        if (IsVisibleByMainCamera())
            DestroyEnemy();
    }

    private bool IsVisibleByMainCamera()
    {
        if(Camera.main == null)
        {
            Debug.LogWarning("Camera.main est null !");
            return false;
        }
            

        var renderer = GetComponent<Renderer>();
        if(renderer == null)
        {
            Debug.LogWarning("Pas de Renderer sur " + gameObject.name);
            return false;
        }

        if(!renderer.isVisible)
        {
            Debug.Log($"{gameObject.name} : Renderer pas visible (hors champ)");
            return false;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool isVisible = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        Debug.Log($"{gameObject.name} visible dans le frustum ? {isVisible}");
        return isVisible;
    }*/
}