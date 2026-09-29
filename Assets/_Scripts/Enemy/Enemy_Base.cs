using Unity.VisualScripting;

/*
 *Author[Lopez - Sotelo, Jorge]
 * Date Created[9/20/2026]
 * Last Updated[9/20/2026]
 * [This holds the enemy stats and chases the player around the map while keeping a certain distance from the player.]
 */
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    /// <summary>
    /// Drop the player prefab in here so the enemy knows what to chase
    /// </summary>
    [Header("Target")]
    [SerializeField] Transform target;

    /// <summary>
    /// Drop the enemy body prefab or enemy prefab here to active the enemy(whichever works best)
    /// </summary>
    [Header("Base")]
    [SerializeField] Transform hull;

    /// <summary>
    /// Stat list. We can modify these in the editor for light,medium, and heavy tanks. 
    /// </summary>
    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f; //Speed it moves
    [SerializeField] float bodyTurnSpeed = 150f; //Who fast it turns 
    [SerializeField] float acceleration = 8f;   //How long it takes to pick up speed
    [SerializeField] float Distance = 10f;  //Distane between the enemy and the player. 
    [SerializeField] float Repathing = .25f; //How long 

    NavMeshAgent agent;
    float repathTimer;

    public void SetTarget(Transform t) { target = t; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = moveSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = Distance;

        if (hull == null) hull = transform;
        repathTimer = Random.Range(0f, Repathing);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }
        if (target == null)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || !agent.isOnNavMesh) return;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = Repathing;
            agent.SetDestination(target.position);
        }

        Vector3 toSteer = agent.steeringTarget - transform.position;
        toSteer.y = 0f;

        bool arrived = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

        if (arrived || toSteer.sqrMagnitude < 0.04f)
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            hullTurnSpeed(toTarget);
            agent.speed = moveSpeed;

        }
        else 
        {
            hullTurnSpeed(toSteer);
            float align = Vector3.Dot(hull.forward, toSteer.normalized);
            agent.speed = moveSpeed * Mathf.InverseLerp(0.35f, 0.95f, align);

        }

        void hullTurnSpeed(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.0001f) return;
            Quaternion want = Quaternion.LookRotation(direction);
            hull.rotation = Quaternion.RotateTowards(hull.rotation, want, bodyTurnSpeed * Time.deltaTime);
        }
       
    }
}
