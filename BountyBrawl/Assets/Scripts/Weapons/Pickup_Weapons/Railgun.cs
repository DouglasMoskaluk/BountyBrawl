using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Railgun : MonoBehaviour
{
    [Tooltip("The laser beam being fired")]
    [SerializeField] private Railgun_Bullet beam;
    [Tooltip("The use timer")]
    [SerializeField] private float timer = 2f;
    [SerializeField] private float playerSlowness = 10f;
    [Tooltip("The sprites when player is holding the weapon for each individual player")]
    [SerializeField] Sprite[] holding = new Sprite[4];
    [SerializeField] private float lifeTime = 20f;

    private Sprite sprite; //The starting sprite before pickup
    private SpriteRenderer spriteRenderer;
    private bool firing = true; //Make sure the player can attack

    private bool canUse = true; // If the weapon can currently be used

    private PlayerBody player;
    private bool thrown = false; // If the player has thrown the weapon

    private BoxCollider2D weaponBody;

    private Throw nowThrow;

    private BoxCollider2D box;

    private bool canThrow;
    private bool canShoot;
    private float tempTimer;
    private float tempLifeTime;

    [SerializeField] private Animator animator;
    private bool used; //If the railgun has been used

    private CameraShake cameraShaking;
    [SerializeField] private AudioSource normFire;
    [SerializeField] private AudioSource throwing;


    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cameraShaking = FindObjectOfType<CameraShake>();
        sprite = spriteRenderer.sprite;
        canShoot = true;
        spriteRenderer.sortingLayerName = "Midground";
        beam.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Midground";
        firing = false;
        tempTimer = timer;
        tempLifeTime = lifeTime;
    }

    private void OnEnable()
    {
        animator.SetTrigger("Idle");
        animator.SetBool("Fire1", false);
        animator.SetBool("Reload", false);
        timer = tempTimer;
        lifeTime = tempLifeTime;
        used = false;
    }

    private void Update()
    {
        if (player != null)
        {
            player.maxAmmo = tempTimer;
            player.currAmmo = timer;

            if (player.getFire1() != 0 && timer > 0 && !thrown && canShoot || used && timer > 0)
            {
                animator.SetBool("Fire1", true);
                player.UsingRailgun(true);
                used = true;
            }
            else
            {
                normFire.Stop();
                beam.Deactivation(true);
                player.UsingRailgun(false);
                Reload();

                if (firing == true)
                {
                    canShoot = true;
                    firing = false;
                }
            }
            if (player.getThrow() != 0 && !firing || timer <= 0 && player.getFire1() != 0)
            {
                animator.SetBool("Fire1", false);
                animator.SetBool("Reload", false);
                animator.SetTrigger("Idle");
                StartCoroutine(Throw());
            } //throw gun if player presses the circle button

            //Drops weapon if player dies
            if(player.getHealth() <= 0)
            {
                normFire.Stop();
                cameraShaking.setShake(false);
                player.ExitGlue();
                player.UsingRailgun(false);
                beam.Deactivation(true);
                used = false;
                canShoot = true;
                firing = false;
                animator.SetBool("Fire1", false);
                animator.SetBool("Reload", false);
                animator.SetTrigger("Idle");

                gameObject.GetComponent<SpriteRenderer>().sprite = sprite; //Reset the sprite
                player.ChangeWeapon(false); //Set player back to default weapon
                transform.parent = null; //Unparent the weapon
                spriteRenderer.sortingLayerName = "Midground";
                beam.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Midground";
                thrown = false;
                box.isTrigger = true;
                player = null; //sets the player to null to wait for next player
                canUse = true; //Weapon is back to being pickupable
                weaponBody.isTrigger = true;
            }
        }

        if (firing)
        {
            timer -= Time.deltaTime;
        }

        if (spriteRenderer.sortingLayerName == "Midground")
        {
            if (lifeTime < 0)
            {
                lifeTime = tempLifeTime;
                gameObject.SetActive(false);
            }
            else
            {
                lifeTime -= Time.deltaTime;
            }
        }
    }

    public void DoneCharging()
    {
        normFire.Play();
        beam.Deactivation(false);
        beam.gameObject.SetActive(true);
        firing = true;
        Shoot1();//shoot gun if there is ammo and if player is holding the trigger
    }

    private void Shoot1()
    {
        cameraShaking.setShake(true);
        cameraShaking.StartCoroutine(cameraShaking.Shake(timer, 0.5f));
        beam.gameObject.SetActive(true);
        beam.Fire(player); //Pass the player   
    }

    public void Reload()
    {
        animator.SetBool("Reload", true);
        animator.SetBool("Fire1", false);
    }

    public void FinishReload()
    {
        animator.SetBool("Reload", false);
        animator.SetTrigger("Idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHolder" && canUse)
        {
            PlayerBody checker = collision.transform.parent.GetComponent<PlayerBody>();
            //If player is not using a weapon
            if (!checker.UsingWeapon())
            {
                spriteRenderer.sortingLayerName = "Foreground";
                beam.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

                player = collision.transform.parent.GetComponent<PlayerBody>();
                player.ChangeWeapon(true);
                canUse = false;
                used = false;
                player.setWeaponIndex(1);
                player.Slow(playerSlowness);

                if (player.GetPlayerCharacter() == 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }else if(player.GetPlayerCharacter() == 1)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[1];
                }
                else if (player.GetPlayerCharacter() == 2)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[2];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[3];
                }

                //Move weapon in desired position
                if (collision.transform.right.x >= 0)
                {
                    transform.position = collision.transform.position + collision.transform.right * 0.2f;
                }
                else
                {
                    transform.position = collision.transform.position + collision.transform.right * 0.2f;
                }
                transform.rotation = collision.transform.rotation;
                transform.parent = collision.GetComponentInChildren<CircleCollider2D>().transform;

                canThrow = true;
            }
        }
    }
    IEnumerator Throw()
    {
        if (canThrow)
        {
            normFire.Stop();
            throwing.Play();
            player.ExitGlue();
            player.UsingRailgun(false);
            used = false;
            canThrow = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite; //Reset the sprite
            player.ChangeWeapon(false); //Set player back to default weapon
            transform.parent = null; //Unparent the weapon

            thrown = true;
            box.isTrigger = false;
            Vector2 traj = player.getLastFacing(); //Get the trajectory of the bullet

            yield return nowThrow.Cooldown(traj, player.gameObject); //Wait till the throwing motion is over
            spriteRenderer.sortingLayerName = "Midground";
            beam.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Midground";
            thrown = false;
            box.isTrigger = true;
            player = null; //sets the player to null to wait for next player
            canUse = true; //Weapon is back to being pickupable
            weaponBody.isTrigger = true;

            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }

        } //Called when the weapon is being thrown
    }

    public void SetCanFire(bool can) { canShoot = can; }

    public void SetUsed(bool change) { used = change; }
}