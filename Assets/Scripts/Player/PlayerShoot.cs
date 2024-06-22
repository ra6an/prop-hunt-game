using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "PlayerBody";
    public PlayerWeapon weapon;
    [SerializeField]
    public Camera cam;

    [SerializeField]
    private LayerMask mask;

    public bool isShooting = false;
    public bool alreadyShoot = false;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (!isShooting) return;

        if (alreadyShoot) return;

        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                PlayerShootServerRpc(_hit.collider.name);
            }
            if (weapon.singleFire)
            {
                alreadyShoot = true;
            }
        }
    }

    [ServerRpc]
    void PlayerShootServerRpc(string _ID)
    {
        Debug.Log(_ID + " has been shot.");
    }

    public void SetShooting(bool value)
    {
        isShooting = value;
        if(!value)
        {
            alreadyShoot = false;
        }
    }

    //void Shoot ()
    //{
    //    RaycastHit _hit;

    //    if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
    //    {
    //        Debug.Log("We hit " + _hit.collider.name);
    //    }
    //}
}
