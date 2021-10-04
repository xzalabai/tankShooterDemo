using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject explosionPrefab;      

    private void OnCollisionEnter(Collision collision)
    {
        // If hits an enemy, give him response

        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Target>() != null)
        {
            collision.gameObject.GetComponent<Target>().ReceivedHit();
        }

        Explode();
    }
    void Explode()
    {
        var explosion = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);
    }
}
