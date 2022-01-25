using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // References
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private Slider energyBar;
    [SerializeField] private Animator animator;

    // Parameters
    [SerializeField] private float energyRecoveryRate;
    [SerializeField] private float pounceEnergy;
    [SerializeField] private float pounceSpeed = 15;
    [SerializeField] private float pounceDuration = 0.1f;

    private float _pouncing;
    private float _energy;

    private void Start()
    {
        _energy = 1;
        attackHitBox.SetActive(false);
    }

    private void Update()
    {
        _energy += energyRecoveryRate * Time.deltaTime;
        if (_energy > 1)
        {
            _energy = 1;
        }
        
        if (_pouncing <= 0 && _energy > pounceEnergy && Input.GetButtonDown("Jump"))
        {
            _pouncing = pounceDuration;
            _energy -= pounceEnergy;
            attackHitBox.SetActive(true);
        }

        energyBar.value = _energy;

        if (_pouncing > 0)
        {
            _pouncing -= Time.deltaTime;
            if (_pouncing <= 0)
            {
                attackHitBox.SetActive(false);
            }
            else
            {
                navMeshAgent.velocity = transform.forward * pounceSpeed;    
            }
        }
        else
        {
            var v = Input.GetAxisRaw("Vertical");
            var h = Input.GetAxisRaw("Horizontal");
            var direction = new Vector3(h, 0, v).normalized;

            navMeshAgent.velocity = direction * navMeshAgent.speed;
        }
        
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
}