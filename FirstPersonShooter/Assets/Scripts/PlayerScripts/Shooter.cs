using UnityEngine;
using TMPro;

public class Shooter : MonoBehaviour
{
    [Header("�������������� ������")]
    [Tooltip("���� ������")][SerializeField] private float weaponDamage = 10;
    [Tooltip("���������������� � ��������� � �������")][SerializeField] private float fireRate = 1;
    [Tooltip("��������� ��������")][SerializeField] private float range = 15;
    [Tooltip("���� � ������ �������� ������")][SerializeField] private float ejectPower = 250;
    [Tooltip("��� ������")][SerializeField] private bool isAutomatic = false;
    [Tooltip("������� ������ � ��������")][SerializeField] private float gunSpread;
    [Tooltip("����������� ������")][SerializeField] private int magCapacity;
    [Tooltip("������� ���-�� �������� � ������")][SerializeField] private int ammo;

    [Header("�������")]
    [Tooltip("������ ������")] public GameObject casing;
    [Tooltip("������ ���������")] public GameObject hitEffect;
    [Tooltip("������� �� ��������")] public ParticleSystem muzzleParticle;
    [Tooltip("�������� �������� ������")] public string gunAnimName;
    [Tooltip("�������� �������� ����������� ������")] public string gunReloadAnimName;

    public Animator gunAnimator;
    public Transform casingExit;
    public Camera playerCam;
    public TMP_Text ammoText;

    private float nextFire = 0;

    private void Start()
    {
        ammo = magCapacity;
        gunAnimator.speed = fireRate;
    }
    void Update()
    {
        ammoText.text = ammo + "/" + magCapacity;
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                if (ammo > 0)
                {
                    nextFire = Time.time + 1f / fireRate;
                    Shoot();
                    gunAnimator.Play(gunAnimName);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                if (ammo > 0)
                {
                    nextFire = Time.time + 1f / fireRate;
                    Shoot();
                    gunAnimator.Play(gunAnimName);
                }
            }
        }
        if ((ammo == 0 || Input.GetKeyDown(KeyCode.R)) && ammo < magCapacity)
        {
            ammo = 0;
            gunAnimator.Play(gunReloadAnimName);
        }
    }

    void Shoot()
    {
        ammo--;
        muzzleParticle.Play();

        GameObject tempCasing;
        tempCasing = Instantiate(casing, casingExit.position, casingExit.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExit.position - casingExit.right * 0.3f - casingExit.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        Destroy(tempCasing, 5f);

        RaycastHit hit;
        Vector3 spread = new Vector3(Random.Range(-gunSpread / 2, gunSpread / 2), Random.Range(-gunSpread / 2, gunSpread / 2), Random.Range(-gunSpread / 2, gunSpread / 2));
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward + spread, out hit, range))
        {
            Debug.DrawRay(playerCam.transform.position, (playerCam.transform.forward + spread) * range, Color.blue, 10f);

            if (hit.collider.gameObject.GetComponent<HpSystem>() != null)
            {
                HpSystem.TakeDamage(hit.collider.gameObject, weaponDamage);
            }

            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 3f);
        }
    }

    //void TakeDamage(GameObject target, float damage)
    //{
    //    target.GetComponent<HpSystem>().hp -= damage;
    //}

    void Reloading()
    {
        ammo = magCapacity;

        gunAnimator.SetTrigger("ARreloaded");
    }
}