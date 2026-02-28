using System.Collections.Generic;
using UnityEngine;

public class RuneRing : MonoBehaviour
{
    [Header("Visual")]
    public GameObject bladeVisualPrefab;
    public int bladeCount = 0;
    public int maxBladeCount = 5;
    public float orbitRadius = 1.2f;
    public float orbitSpeed = 140f;

    [Header("Damage")]
    public float contactDamage = 8f;
    public float hitRadius = 0.2f;
    public float damageTickInterval = 0.2f;

    [Header("Signature Snap")]
    public float snapInterval = 3f;
    public float minSnapInterval = 1.2f;
    public float snapRange = 6f;
    public float snapDamage = 20f;

    private readonly List<Transform> blades = new();
    private float angleOffset;
    private float damageTick;
    private float snapTimer;
    private int resonanceLevel;

    private void Start()
    {
        RebuildBlades();
        snapTimer = snapInterval;
    }

    private void OnDisable()
    {
        ClearBlades();
    }

    private void Update()
    {
        if (bladeCount <= 0) return;

        RemoveDestroyedBlades();
        if (blades.Count == 0 && bladeVisualPrefab != null)
            RebuildBlades();

        UpdateOrbit();

        damageTick -= Time.deltaTime;
        if (damageTick <= 0f)
        {
            DealContactDamage();
            damageTick = damageTickInterval;
        }

        snapTimer -= Time.deltaTime;
        if (snapTimer <= 0f)
        {
            DoSnapStrike();
            snapTimer = snapInterval;
        }
    }

    private void UpdateOrbit()
    {
        if (blades.Count != bladeCount)
            RebuildBlades();

        angleOffset += orbitSpeed * Time.deltaTime;
        float step = 360f / Mathf.Max(1, bladeCount);

        for (int i = 0; i < blades.Count; i++)
        {
            var blade = blades[i];
            if (blade == null) continue;

            float a = (angleOffset + i * step) * Mathf.Deg2Rad;
            Vector3 local = new(Mathf.Cos(a), Mathf.Sin(a), 0f);
            blade.position = transform.position + local * orbitRadius;
            blade.up = local;
        }
    }

    private void DealContactDamage()
    {
        foreach (var blade in blades)
        {
            if (blade == null) continue;

            var hits = Physics2D.OverlapCircleAll(blade.position, hitRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Enemy")) continue;
                var enemy = h.GetComponent<EnemyStats>();
                if (enemy != null) enemy.TakeDamage(contactDamage);
            }
        }
    }

    private void DoSnapStrike()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            if (e == null) continue;
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist && d <= snapRange)
            {
                bestDist = d;
                best = e.transform;
            }
        }

        if (best == null) return;

        var enemyStats = best.GetComponent<EnemyStats>();
        if (enemyStats != null)
            enemyStats.TakeDamage(snapDamage);
    }

    public void UnlockOrUpgrade()
    {
        if (bladeCount <= 0)
        {
            bladeCount = 1;
            RebuildBlades();
            Debug.Log("Upgrade: Rune Ring unlocked");
            return;
        }

        if (bladeCount < maxBladeCount)
        {
            bladeCount++;
            RebuildBlades();
            Debug.Log($"Upgrade: Rune Ring blades x{bladeCount}");
            return;
        }

        AddResonance();
    }

    public void AddResonance()
    {
        resonanceLevel++;
        orbitSpeed += 15f;
        contactDamage += 1.5f;
        snapDamage += 2.5f;
        snapInterval = Mathf.Max(minSnapInterval, snapInterval - 0.15f);
        Debug.Log($"Upgrade: Resonance {resonanceLevel}");
    }

    private void RebuildBlades()
    {
        ClearBlades();

        if (bladeVisualPrefab == null || bladeCount <= 0) return;

        for (int i = 0; i < bladeCount; i++)
        {
            var go = Instantiate(bladeVisualPrefab, transform.position, Quaternion.identity);
            blades.Add(go.transform);
        }
    }

    private void ClearBlades()
    {
        for (int i = 0; i < blades.Count; i++)
        {
            if (blades[i] != null) Destroy(blades[i].gameObject);
        }
        blades.Clear();
    }

    private void RemoveDestroyedBlades()
    {
        blades.RemoveAll(b => b == null);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, snapRange);
    }
#endif
}
