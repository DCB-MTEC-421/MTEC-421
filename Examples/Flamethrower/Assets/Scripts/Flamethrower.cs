using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] ParticleSystem _shootingSystem;
    [SerializeField] ParticleSystem _onFireSystemPrefab;
    [SerializeField] FlamethrowerAttackRadius _attackRadius;

    [Space]
    [SerializeField] int _burningDPS = 5;
    [SerializeField] float _burnDuration = 3.0f;

    // ObjectPool is used to instantiate the particle system on the enemy as opposed
    // to instantiating and destroying new particle systems.
    ObjectPool<ParticleSystem> OnFirePool;
    Dictionary<Enemy, ParticleSystem> _enemyParticleSystems = new();

    // Wwise
    [SerializeField] AK.Wwise.Event _startFlamethrower;
    [SerializeField] AK.Wwise.Event _stopFlamethrower;
    bool _isShooting;

    void Awake() {
        OnFirePool = new ObjectPool<ParticleSystem>(CreateOnFireSystem);

        // Subscribe to AttackRadius events.
        _attackRadius.OnEnemyEnter += StartDamagingEnemy;
        _attackRadius.OnEnemyExit += StopDamagingEnemy;
    }

    void Start() {
        _isShooting = false;
    }

    ParticleSystem CreateOnFireSystem() {
        return Instantiate(_onFireSystemPrefab);
    }

    void StartDamagingEnemy(Enemy enemy) {
        if (enemy.TryGetComponent<IBurnable>(out IBurnable burnable)) {
            // Subscribe to OnDeath event.
            enemy.Health.OnDeath += HandleEnemyDeath;

            burnable.StartBurning(_burningDPS);

            // Instantiate small fire particle system over the enemy.
            ParticleSystem onFireSystem = OnFirePool.Get();
            onFireSystem.transform.SetParent(enemy.transform, false);
            onFireSystem.transform.localPosition = new Vector3(0, 1, 0);

            // Retrieve particle system main module to set particle system attributes.
            ParticleSystem.MainModule main = onFireSystem.main;
            main.loop = true;

            // Keep track of instantiated systems.
            _enemyParticleSystems.Add(enemy, onFireSystem);
        }
    }

    void HandleEnemyDeath(Enemy enemy) {
        // Unsubscribe to OnDeath event and remove from dictionary.
        enemy.Health.OnDeath -= HandleEnemyDeath;

        if (_enemyParticleSystems.ContainsKey(enemy)) {
            StartCoroutine(DelayedDisableBurn(enemy, _enemyParticleSystems[enemy], _burnDuration));
            _enemyParticleSystems.Remove(enemy);
        }
    }

    /// <summary>
    /// This function will get called whenever the enemy dies or exits the burning ratio.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="instance"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator DelayedDisableBurn(Enemy enemy, ParticleSystem instance, float duration) {
        ParticleSystem.MainModule main = instance.main;
        main.loop = false;

        yield return new WaitForSeconds(duration);
        instance.gameObject.SetActive(false);

        if (enemy.TryGetComponent<IBurnable>(out IBurnable burnable))
            burnable.StopBurning();
    }

    void StopDamagingEnemy(Enemy enemy) {
        // Unsubscribe from OnDeath event.
        enemy.Health.OnDeath -= HandleEnemyDeath;

        if (_enemyParticleSystems.ContainsKey(enemy)) {
            StartCoroutine(DelayedDisableBurn(enemy, _enemyParticleSystems[enemy], _burnDuration));
            _enemyParticleSystems.Remove(enemy);
        }
    }

    void Update() {
        if (Mouse.current.leftButton.isPressed)
            Shoot();
        else
            StopShooting();
    }

    /// <summary>
    /// Enable the capsule for collision detection and the fire particle system.
    /// </summary>
    void Shoot() {
        _shootingSystem.gameObject.SetActive(true);
        _attackRadius.gameObject.SetActive(true);

        if (!_isShooting) {
            Debug.Log("Shooting!");
            _startFlamethrower.Post(gameObject);
            _isShooting = true;
        }
    }

    /// <summary>
    /// Disable the capsule for collision detection and the fire particle system.
    /// </summary>
    void StopShooting() {
        _attackRadius.gameObject.SetActive(false);
        _shootingSystem.gameObject.SetActive(false);

        if (_isShooting) {
            Debug.Log("Stopping...");
            _stopFlamethrower.Post(gameObject);
            _isShooting = false;
        }
    }
}
