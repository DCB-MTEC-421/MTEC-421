using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] [Range(1, 100)] int _enemiesToSpawn = 10;

    NavMeshTriangulation _triangulation;

    void Awake() {
        _triangulation = NavMesh.CalculateTriangulation();
    }

    void Start() {
        for (int i = 0; i < _enemiesToSpawn ; i++) {
            Enemy enemy = Instantiate(
                _enemyPrefab,
                _triangulation.vertices[Random.Range(0, _triangulation.vertices.Length)],
                Quaternion.identity
            );

            enemy.Movement.Triangulation = _triangulation;
            enemy.Health.OnDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath(Enemy enemy) {
        StartCoroutine(DelayedDestroy(enemy));
        enemy.Movement.StopMoving();
        enemy.Movement.enabled = false;

        // Uncomment for Animator functionality.
        // enemy.GetComponent<Animator>().SetTrigger("Death");
    }

    IEnumerator DelayedDestroy(Enemy enemy) {
        yield return new WaitForSeconds(3);

        enemy.gameObject.SetActive(false);
    }
}
