using UnityEngine;

/// <summary>
/// This script is a registry to provide a single access point to multiple
/// enemy-related components
/// </summary>
[DisallowMultipleComponent]     // Prevents component from being added more than once to a GameObject.
public class Enemy : MonoBehaviour
{
    public EnemyMovement Movement;
    public EnemyHealth Health;
}
