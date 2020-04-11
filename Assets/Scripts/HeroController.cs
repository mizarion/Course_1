using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroController : MonoBehaviour
{
    Animator animator; // reference to the animator component
    NavMeshAgent agent; // reference to the NavMeshAgent

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
        //{
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    agent.SetDestination()
        //}
    }
}
