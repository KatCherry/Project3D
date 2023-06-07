using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimCam;
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorController animator1;
    [SerializeField] private AnimatorController animator2;
    [SerializeField] private GameObject Rifle;
    [SerializeField] private GameObject Pistol;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private ThirdPersonController thirdPersonController;

    [SerializeField] private Transform onFireObject;

    [SerializeField] private float fireSpeed = 0.5f;
    [SerializeField] private float fireAccurate = 0.5f;
    [SerializeField] private GameObject RifleStartBullet;

    private float fireReloading = 0;



    private void Update()
    {
        var mouseWorldPosition = Vector3.zero;
        var hitPoint = Vector3.zero;
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            hitPoint = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (_input.isAim)
        {
            _aimCam.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            _aimCam.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            thirdPersonController.SetSensitivity(normalSensitivity);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 13f));
        }

        if (_input.isFire)
        {
            if (fireReloading <= 0)
            {
                fireReloading = fireSpeed;

                if (Random.Range(0, 1f) > fireAccurate)
                {
                    var obj = Instantiate(onFireObject, hitPoint, Quaternion.identity);

                    //var obj = Instantiate(bullet, RifleStartBullet, Quaternion.identity);
                    //var bulletObj = obj.GetComponent<Bullet>();
                    //bulletObj.SetEndPos(hitPoint);
                    //bulletObj.StartFly();
                }
            }
        }

        fireReloading -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    public void SwitchWeapon()
    {
        //  if (pistol)
        {
            animator.runtimeAnimatorController = animator1;
            Pistol.SetActive(true);
            Rifle.SetActive(false);
        }
    }
}
