using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4f;
    public float timeToRotate = 2f;
    public float speedWalk = 1f;
    public float speedRun = 2f;

    public float viewRadius = 60f;
    public float viewAngle = 360f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public Transform[] waypoints;

    private int m_CurrentWaypointIndex;
    private Vector3 playerLastPosition;
    private Vector3 m_PlayerPosition;

    private float m_WaitTime;
    private float m_TimeToRotate;

    private bool m_PlayerInRange;
    private bool m_PlayerNear;
    private bool m_IsPatrol = true;
    private bool m_CaughtPlayer;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        playerLastPosition = Vector3.zero;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        navMeshAgent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speedWalk;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
            Chasing();
        else
            Patrolling();
    }

    void EnvironmentView()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        bool seenPlayer = false;

        for (int i = 0; i < playersInRange.Length; i++)
        {
            Transform player = playersInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
            {
                float distToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {
                    seenPlayer = true;
                    m_PlayerInRange = true;
                    m_PlayerNear = false;
                    m_IsPatrol = false;
                    m_PlayerPosition = player.position;
                    playerLastPosition = player.position;
                    return;
                }
            }
        }

        if (!seenPlayer)
        {
            if (m_PlayerInRange)
            {
                m_PlayerNear = true;
            }

            m_PlayerInRange = false;

            if (!m_PlayerNear)
                m_IsPatrol = true;
        }
    }

    void Chasing()
    {
        if (m_CaughtPlayer) return;

        Move(speedRun);
        navMeshAgent.SetDestination(m_PlayerPosition);

        if (!m_PlayerInRange && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            m_IsPatrol = true;
            m_PlayerNear = true;
            m_WaitTime = startWaitTime;
            m_TimeToRotate = timeToRotate;
        }
    }

    void Patrolling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate > 0)
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
            else
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }

            return;
        }

        if (waypoints.Length == 0) return;

        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0)
            {
                NextPoint();
                Move(speedWalk);
                m_WaitTime = startWaitTime;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);

        if (Vector3.Distance(transform.position, player) <= 0.3f)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                m_IsPatrol = true;
                Move(speedWalk);

                if (waypoints.Length > 0)
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
    }

    void NextPoint()
    {
        if (waypoints.Length == 0) return;
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
}