/*
 *Author[Lopez-Sotelo, Jorge]
 * Date Created[09 / 20 / 2026]
 * Last Updated[09 / 20 / 2026]
 * []
 */
using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Parts")]
    [SerializeField] Transform turret;
    [SerializeField] Transform muzzle;
    [SerializeField] EnemyShell shellPrefab;


    [Header("Aiming")]
    [SerializeField] float turretTurnSpeed = 120f;
    [SerializeField] float rangeTolerance =5f;
    [SerializeField] float marginOfError= 3f;
    [SerializeField] bool leadTarget = true;

    [Header("Firing")]
    [SerializeField] float firingRate = 2f;
    [SerializeField] float shootRange = 18f;
    [SerializeField] float windupTime = 0.4f;
    [SerializeField] float shellSpeed = 12f;
    [SerializeField] float shellDamage = 10f; //This can be changed once we set up a health bar for the player



    [Header("Line of Sight")]
    [SerializeField] LayerMask obstacleMask; //Shell will not go through trees, forest, or buildings

    float fireTimer;
    bool windingUp;
    bool aimLocked;
    float aimAngle = 180f;
    Vector3 lastTargetPos;
    Vector3 targetVelocity;

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
            return;
        }

        lastTargetPos = target.position;
        fireTimer = Random.Range(0.3f, firingRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        TrackTargetVelocity();
        AimTurret();

        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f || windingUp) return;

        if (FlatDistance() > shootRange) return;
        if (aimAngle > rangeTolerance) return;
        if (!CanSeeTarget()) return;

        StartCoroutine(ShootRoutine());
    }

    void TrackTargetVelocity()
    {
        if (Time.deltaTime > 0f)
        {
            Vector3 v = (target.position - lastTargetPos) / Time.deltaTime;
            v.y = 0f;
            targetVelocity = Vector3.Lerp(targetVelocity, v, Mathf.Clamp01(10f * Time.deltaTime));
        }
        lastTargetPos = target.position;
    }


    void AimTurret()
    {
        Vector3 aimPoint = target.position;
        if (leadTarget)
        {
            float travelTime = FlatDistance() / shellSpeed;
            aimPoint += targetVelocity * travelTime;
        }
        Vector3 dir = aimPoint - turret.position;
        dir.y = 0f;
        if(dir.sqrMagnitude < 0.01f) return;

        if (!windingUp)
        {
            Quaternion want = Quaternion.LookRotation(dir);
            turret.rotation = Quaternion.RotateTowards(turret.rotation, want, turretTurnSpeed * Time.deltaTime);
        }

        Vector3 turretForward = turret.forward;
        turretForward.y = 0f;
        aimAngle = Vector3.Angle(turretForward, dir);
    }


    bool CanSeeTarget()
    {
        Vector3 from = turret.position + Vector3.up * 0.8f;
        Vector3 to = target.position + Vector3.up * 0.8f;
        return !Physics.Linecast(from, to, obstacleMask, QueryTriggerInteraction.Ignore);
    }

    float FlatDistance()
    {
        Vector3 flat = target.position - transform.position;
        flat.y = 0f;
        return flat.magnitude;
    }

    IEnumerator ShootRoutine()
    {
        windingUp = true;
        yield return new WaitForSeconds(windupTime);

        Fire();

        windingUp = false;
         fireTimer = firingRate * Random.Range(0.85f, 1.15f);
    }

    void Fire()
    {
        float yaw = turret.eulerAngles.y + Random.Range(-marginOfError, marginOfError);
        Vector3 aimDir = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;

        EnemyShell shell = Instantiate(shellPrefab, muzzle.position, Quaternion.FromToRotation(Vector3.up, aimDir));
        shell.speed = shellSpeed;
        shell.damage = shellDamage;
    }
}
