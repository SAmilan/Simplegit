using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] Vector3 startLocation;
    [SerializeField]float stopDistance;
    [SerializeField] Button runToPlayer;
    [SerializeField] Button shoot;
    [SerializeField] Button ragDall;
    [SerializeField]
    private Vector3 m_InitialPosition;
    [SerializeField]
    private Quaternion m_InitialQuateranion;



    // Start is called before the first frame update
    private void Awake()
    {
        m_InitialPosition = transform.position;
        m_InitialQuateranion = transform.rotation;
    }
    void Start()
    {
        //agent.stoppingDistance = 10;
        agent = GetComponent<NavMeshAgent>();
        runToPlayer.onClick.AddListener(RunAtPlayer);
        shoot.onClick.AddListener(ShootAtUser);
        ragDall.onClick.AddListener(AddRagDall);

    }
    private void OnEnable()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("Fire", false);
        animator.SetBool("Aim", false);
        animator.SetBool("Talk", false);
        transform.position = m_InitialPosition;
        transform.rotation = m_InitialQuateranion;

    }
    private void OnDisable()
    {
        transform.position = m_InitialPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsRunning", true);
        }
        else { animator.SetBool("IsRunning", false); }

       
        if(agent.stoppingDistance>= Vector3.Distance(transform.position,player.position))
        {
            
            FacePlayer();
        }

    }

    void FacePlayer()
    {
        Vector3 direction = (player.position- transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3( direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*5f);
    }

    public void RunAtPlayer()
    {
        animator.SetBool("Aim", false);
        agent.SetDestination(player.position);
    }

    public void ShootAtUser()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("Aim", true);
    }

    public void AddRagDall() {
        agent.SetDestination(transform.position);
        agent.enabled = false;
        animator.enabled = false;
    }

}
