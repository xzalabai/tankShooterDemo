using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosionPrefab;      // Explosion effect prefab

    private void OnEnable()
    {
        transform.position = GetRandomValidPoint();
    }

    // Invoked when Missile hits a target (in Missile.cs)
    public void ReceivedHit()
    {
        // Play Explosion Particle

        var explosion = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);

        gameObject.SetActive(false);
        GameManager.Instance.TargetDestroyed();
    }

    public virtual Vector3 GetRandomValidPoint()
    {
        Collider[] hitColliders;
        while (true)
        {
            // Get random point on arena ground

            Vector3 possibleSpawnPoint = RandomPointInBounds(GameManager.Instance.arena.transform.Find("Floor").GetComponent<MeshCollider>().bounds);

            // Find out, if intersects with wall, player or enemy (if yes, try again)

            hitColliders = Physics.OverlapSphere(possibleSpawnPoint, 3f);

            if (hitColliders.Any(s => !s.CompareTag("Wall") && !s.CompareTag("Player") && !s.CompareTag("Enemy")))
            {
                return possibleSpawnPoint;
            }            
        }
        
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
