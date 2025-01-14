using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Action<Health> OnDeath;

    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private int _startingHealth = 3;

    //Getters
    public GameObject SplatterPrefab => splatterPrefab;
    public GameObject DeathVfx => deathVfx;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
