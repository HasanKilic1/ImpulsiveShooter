using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnTimer = 3f;

    private ColorChanger colorChanger;

    private void Awake()
    {
        colorChanger = GetComponent<ColorChanger>();
    }

    private void Start() {
        StartCoroutine(SpawnRoutine());
    }
    
    private IEnumerator SpawnRoutine() {
        while (true)
        {
            colorChanger.SetRandomColor();
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
            enemy.Init(colorChanger.DefaultColor);

            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}
