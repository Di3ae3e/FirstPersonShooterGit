using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Параметры")]
    public float shootingDistance;//дистаниця от игрока на которой враг начнет стрелять
    public float retreatDistance;//дистанция от игрока на которой враг начнет отсупать от игрока
    [Header("Параметры_стрельбы")]
    public float spread;
    public int magCapacity = 10;
    private int ammo;
    [SerializeField] private float reloadingTime;
    public float startReloadingTime = 5;
    public float fireRate = 1;
    public float range = 15;
    public float weaponDamage = 5;
    private float nextFire = 0;

    public Transform shootPoint;//точка выстрела
    public Transform arm;//пустой объект для поворота врага и оружия
    private Transform player;
    [Header("Состояния")]
    public bool isApproach = false;
    public bool isShooting = false;
    public bool isRetreat = false;
    public bool isChangesPosition = false;

    private void Start()
    {
        ammo = magCapacity;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        reloadingTime = 0;
    }
    public void Approaching()//враг приближается
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        agent.SetDestination(player.position);
    }
    public void Shooting()
    {
        if (ammo > 0)
        {
            agent.SetDestination(this.transform.position);
            //transform.position = this.transform.position;
            Vector3 shotSpread = new Vector3(Random.Range(-spread / 2, spread / 2), Random.Range(-spread / 2, spread / 2), Random.Range(-spread / 2, spread / 2));

            if (Time.time > nextFire)
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward + shotSpread, out hit, range))
                {
                    if (hit.collider.gameObject.GetComponent<HpSystem>() != null)
                    {
                        HpSystem.TakeDamage(hit.collider.gameObject, weaponDamage);
                        Debug.Log("target hit");
                    }
                    nextFire = Time.time + 1f / fireRate;
                    Debug.Log(ammo + " left");
                    ammo--;
                }
            }
        }
    }
    public void Retreat()
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        agent.SetDestination(-player.position);
    }
    public void ChangingPosition()
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.position + new Vector3(0, Random.Range(-2, 2)), -speed / 2 * Time.deltaTime);
        //agent.SetDestination(-player.position + new Vector3(0, Random.Range(-2, 2)));
    }
    private void FixedUpdate()
    {
        //target tracking
        arm.transform.LookAt(player.transform.position);
        //перезарядка
        if (ammo == 0)
        {
            reloadingTime += Time.deltaTime;
            if (reloadingTime >= startReloadingTime)
            {
                ammo = magCapacity;
                reloadingTime = 0;
            }
        }

        //отсупление
        if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            isRetreat = true;
            isShooting = false;
            isApproach = false;
            isChangesPosition = false;
        }

        //смена позиции при перезарядке
        else if (ammo == 0 && !isApproach)
        {
            isChangesPosition = true;
            isShooting = false;
            isApproach = false;
            isRetreat = false;
        }

        //движение в сторону игрока
        else if (Vector3.Distance(transform.position, player.position) > shootingDistance && !isShooting)
        {
            isApproach = true;
            isShooting = false;
            isChangesPosition = false;
            isRetreat = false;
        }

        //пребывание на нужной для стрельбы дистанции
        else if (Vector3.Distance(transform.position, player.position) < shootingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            isShooting = true;
            isApproach = false;
            isRetreat = false;
            isChangesPosition = false;
        }

        if (isApproach)
            Approaching();
        else if (isShooting)
            Shooting();
        else if (isRetreat)
            Retreat();
        //else if (isChangesPosition)
        //    ChangingPosition();
    }
}