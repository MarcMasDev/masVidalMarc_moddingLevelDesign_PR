using UnityEngine;

public class BoxCollisionSmoke : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeTrail; // Your existing smoke trail
    [SerializeField] private float smokeColumnDuration = 3f;

    void OnCollisionEnter(Collision collision)
    {
        // Get contact point
        ContactPoint contact = collision.contacts[0];
        
        RepositionSmokeTrail(contact.point);
    }

    void RepositionSmokeTrail(Vector3 contactPoint)
    {
        // Detach smoke trail from missile so it stays at impact point
        smokeTrail.transform.position = contactPoint;

        // Start emitting particles at new position
        smokeTrail.Play();

        // Stop emission after duration and clean up
        StartCoroutine(StopSmokeAfterDelay());
    }

    System.Collections.IEnumerator StopSmokeAfterDelay()
    {
        yield return new WaitForSeconds(smokeColumnDuration);

        // Stop emission but let existing particles finish
        smokeTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}