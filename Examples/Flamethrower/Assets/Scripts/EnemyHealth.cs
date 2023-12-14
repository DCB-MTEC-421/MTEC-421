using System.Collections;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour, IDamageable, IBurnable
{
    [SerializeField] TextMeshPro HealthText;
    [SerializeField] int _health;
    public int Health { 
        get => _health;
        set {
            _health = value;
            HealthText.SetText(Health.ToString());
        }  
    }

    [SerializeField] bool _isBurning;
    public bool IsBurning { get => _isBurning; set => _isBurning = value; }

    Coroutine _burnCoroutine;

    public event DeathEvent OnDeath;
    public delegate void DeathEvent(Enemy enemy);

    public void TakeDamage(int damage) {
        Health -= damage;

        if (Health <= 0) {
            Health = 0;
            OnDeath?.Invoke(GetComponent<Enemy>());
            StopBurning();
        }
    }

    public void StartBurning(int damagePerSecond) {
        IsBurning = true;
        if (_burnCoroutine != null) {
            StopCoroutine(_burnCoroutine);
        }

        _burnCoroutine = StartCoroutine(Burn(damagePerSecond));
    }

    IEnumerator Burn(int damagePerSecond) {
        float minTimeToDamage = 1.0f / damagePerSecond;

        WaitForSeconds wait = new(minTimeToDamage);
        int damagePerTick = Mathf.CeilToInt(minTimeToDamage);

        TakeDamage(damagePerTick);
        while (IsBurning) {
            yield return wait;
            TakeDamage(damagePerTick);
        }
    }

    public void StopBurning() {
        IsBurning = false;

        if (_burnCoroutine != null) {
            StopCoroutine(_burnCoroutine);
        }
    }
}
