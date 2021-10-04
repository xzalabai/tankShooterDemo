using System.Linq;
using UnityEngine;

// Turret has an own script (in case of future reusability, e.g. with multiple turrets on tanks, towers with turrets ...)
public class TankTurret : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sets rotation speed of turret")]
    private float turretRotationSpeed;                          
    
    [SerializeField]
    [Tooltip("Speed of missiles")]
    private float missileSpeed;                                 
    
    [SerializeField]
    [Tooltip("Period between 2 shots")]
    private float shootPeriod;                                  
    private bool canShoot;
    private float shootPeriodCountdown;
    
    public GameObject missileSpawnPosition;             
    public GameObject classicMissilePrefab;             
    public GameObject homingMissilePrefab;         
    
    private void Update()
    {
        if (canShoot && Input.GetButtonUp("Fire1"))
        {
            canShoot = false;
            SendFastMissile();
        }    

        if (canShoot && Input.GetButtonUp("Fire2"))
        {
            canShoot = false;
            SendHomingMissile();
        }

        // Player can shoot only after every N milisconds

        if (!canShoot)
        {
            if (shootPeriodCountdown > shootPeriod)
            {
                canShoot = true;
                shootPeriodCountdown = 0f;
            }
            else
                shootPeriodCountdown += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Get mouse position and calculate rotation

        Vector3 lookPosition = GetMousePositionToWorld();
        Quaternion rotation = Quaternion.LookRotation(lookPosition - transform.position);
        transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation, Time.deltaTime * turretRotationSpeed);
    }

    private void SendFastMissile()
    {
        GameObject missile = Instantiate(classicMissilePrefab, missileSpawnPosition.transform.position, transform.rotation);
        missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * missileSpeed, ForceMode.Impulse);
    }

    private void SendHomingMissile()
    {
        // Find nearest target, instantiate missile and set enemy target

        GameObject nearestTarget = GetTargetNearClick();

        HomingMissile homingMissile = Instantiate(homingMissilePrefab, missileSpawnPosition.transform.position, transform.rotation).GetComponent<HomingMissile>();
        homingMissile.GetComponent<Rigidbody>().AddForce(homingMissile.transform.forward * missileSpeed, ForceMode.Force);
        
        homingMissile.SetTarget(nearestTarget);
    }

    private Vector3 GetMousePositionToWorld()
    {
        // Calculate mouse position to plane via Raycast (height of objects is not relevant, because we are only rotating)

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            return new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);
        }

        return Vector3.zero;
    }

    GameObject GetTargetNearClick()
    {
        // Calculate radius around click and get target

        Vector3 mouseClick = GetMousePositionToWorld();
        Collider[] hitColliders = Physics.OverlapSphere(mouseClick, 5f);
        GameObject target = hitColliders.Where(x => x.CompareTag("Enemy")).FirstOrDefault()?.gameObject;
        
        return target;
    }
}
