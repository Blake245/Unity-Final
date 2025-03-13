using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class RangeEnemyController : MonoBehaviour
{
    //[SerializeField] private Transform[] waypoints;
    [SerializeField, Tooltip("Event raised when enemy dies")]
    Event onDeathEvent;

    //private NavMeshAgent agent;
    //private Transform waypoint;

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //if (waypoints == null || waypoints.Length == 0)
        //{
        //    waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(go => go.transform).ToArray();
        //}
        //waypoint = waypoints[Random.Range(0, waypoints.Length)];

        //agent.SetDestination(waypoint.position);
    }

    void Update()
    {
        //if (agent.remainingDistance < 0.5f)
        //{
        //    waypoint = waypoints[Random.Range(0, waypoints.Length)];
        //    agent.SetDestination(waypoint.position);
        //}
    }

    public void OnDestroyed(DamageInfo damageInfo)
    {
        //if (animator != null) animator.SetTrigger("Death");
        onDeathEvent?.RaiseEvent();
    }
}
