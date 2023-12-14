using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class FlamethrowerAttackRadius : MonoBehaviour
{
    public delegate void EnemyEnteredEvent(Enemy enemy);
    public delegate void EnemyExitedEvent(Enemy enemy);

    public EnemyEnteredEvent OnEnemyEnter;
    public EnemyExitedEvent OnEnemyExit;

    private List<Enemy> _enemiesInRadius = new();

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Enemy>(out Enemy enemy)) {
            _enemiesInRadius.Add(enemy);
            OnEnemyEnter?.Invoke(enemy);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<Enemy>(out Enemy enemy)) {
            _enemiesInRadius.Remove(enemy);
            OnEnemyExit?.Invoke(enemy);
        }
    }

    /// <summary>
    /// The OnTriggerExit event won't automatically raise the OnEnemyExit event 
    /// when the object is disabled. It is however necessary to manually unsubscribe
    /// all events when the flamethrower particle system is declaring, which will
    /// occur when the player stops attacking.
    /// </summary>
    void OnDisable() {
        foreach (Enemy enemy in _enemiesInRadius)
            OnEnemyExit?.Invoke(enemy);

        _enemiesInRadius.Clear();
    }
}
