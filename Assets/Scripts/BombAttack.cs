using Scripts;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BombAttack : MonoBehaviour
{
    public float attackRadius = 10f;
    public Vector2 spawnIntervalRange = new Vector2(0.5f, 3.0f);
    public GameObject bombShadow;
    public Vector2 shadowDurationRange = new Vector2(1.0f, 3.0f);
    private Transform shadow;
    void Start()
    {
        StartCoroutine(SpawnBombShadowsRoutine());
    }

    private void Update()
    {
        if(shadow != null)
        {
            shadow.transform.localScale += Vector3.one * Time.deltaTime;
        }
    }

    private IEnumerator SpawnBombShadowsRoutine()
    {
        yield return new WaitForSeconds(shadowDurationRange.x);
        while (true)
        {
            Vector3 randomPoint = GetRandomPointInRadius();
            GameObject shadowGO = Instantiate(bombShadow, randomPoint, Quaternion.identity);
            shadow = shadowGO.transform;
            shadowGO.SetActive(true);
            float shadowDuration = Random.Range(shadowDurationRange.x, shadowDurationRange.y);
            yield return new WaitForSeconds(shadowDuration);

            GetComponent<BombSpawner>().SpawnBomb(randomPoint, shadow.localScale);
            Destroy(shadowGO);

            float spawnInterval= Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPointInRadius()
    {
        Vector2 randomCircle = Random.insideUnitCircle * attackRadius;
        return new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

   


}
