using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Pistol : MonoBehaviour, IWeaponInterface
{

	[SerializeField] Transform firePoint;
	[SerializeField] GameObject bulletPrefab;
	[SerializeField] float Camera_shake_intensity;
	[SerializeField] float Camera_shake_time;
	[SerializeField] float Camera_shake_frequency;
	[Header("accuracy from 0 to 90")]
	[SerializeField] float accuracy;
	[SerializeField] float drawBack;
	[SerializeField] float timeBeforeFireAgain;
	float timeElapsedAfterLastBullet;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Aiming AimingScript;
	internal bool ShootRequested;
	[SerializeField] Animator animator;

	[SerializeField] Player player;

	[SerializeField] string pistolShootSound;


	// Update is called once per frame
	void Update()
	{
		timeElapsedAfterLastBullet += Time.deltaTime;

		if (ShootRequested)
		{
			if (player.PistolAmmoCount > 0)
			{
				Shoot();
			}
			ShootRequested = false;
		}
	}

	public void Shoot()
	{
		if (timeElapsedAfterLastBullet > timeBeforeFireAgain)
		{
			player.PistolAmmoCount -= 1;

			
			animator.Play("Shoot", -1, 0f);
			timeElapsedAfterLastBullet = 0;
			CinemachineShake.CameraInstance.ShakeCamera(Camera_shake_intensity, Camera_shake_time, Camera_shake_frequency);
			GameObject tmp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			rb.AddForce(-AimingScript.direction * drawBack, ForceMode2D.Impulse);
			tmp.transform.Rotate(0, 0f, Random.Range(-90 + accuracy, 90 - accuracy));


			AudioManager.instance.Play(pistolShootSound);
		}

	}
	public void ShootRequestTotrue()
	{
		ShootRequested = true;
	}
}
