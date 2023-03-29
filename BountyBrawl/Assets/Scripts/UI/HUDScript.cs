using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScript : MonoBehaviour
{
    int characterNum;
    // int weaponNum;
    float playerHealth;
    int playerLives;
    float playerMoney;
    float sliderHealth;

    [SerializeField] private int characterIndex;
    //[SerializeField] private GameObject playerCharacter;
    private GameObject playerCharacter;

    // 0 is for melee
    //[SerializeField] private GameObject hammer;
    //[SerializeField] private GameObject fist;
    //[SerializeField] private GameObject katana;
    //[SerializeField] private GameObject knife;

    // [SerializeField] private GameObject pistol; //1
    // [SerializeField] private GameObject crossbow;
    // [SerializeField] private GameObject railgun;
    // [SerializeField] private GameObject rifle;
    // [SerializeField] private GameObject shotgun; //2
    // [SerializeField] private GameObject shuriken; //3

    [SerializeField] private GameObject sandstorm; //0
    [SerializeField] private GameObject nagakome; //1
    [SerializeField] private GameObject greed; //2
    [SerializeField] private GameObject emerald; //3

    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject health;

    [SerializeField] private float sliderHeight = 5f; //How high the health will float on the player
    [SerializeField] private float sliderSize = 0.3f;
    [SerializeField] private float sliderXOffset = 60f;

    [SerializeField] private Slider ammoSlider;
    [SerializeField] private GameObject ammo;

    public TMP_Text moneyText;

    private bool check;
    private bool found;

    // Start is called before the first frame update
    void Start()
    {
        check = true; //Tell to check for players
        found = false; //When players are found stop checking and don't set player stuff

    }

    private void LateUpdate()
    {
        if (check && !found)
        {
            if(FindObjectsOfType<PlayerBody>() != null)
            {
                PlayerBody[] players = FindObjectsOfType<PlayerBody>();

                foreach(PlayerBody p in players)
                {
                    if (p.GetPlayerIndex() == characterIndex)
                    {
                        characterNum = p.getCharacter();
                        playerCharacter = p.gameObject;

                        if (characterNum == 0)
                        {
                            sandstorm.SetActive(true);
                            //fist.SetActive(true);
                        }
                        if (characterNum == 1)
                        {
                            nagakome.SetActive(true);
                            //katana.SetActive(true);
                        }
                        if (characterNum == 2)
                        {
                            greed.SetActive(true);
                            //knife.SetActive(true);
                        }
                        if (characterNum == 3)
                        {
                            emerald.SetActive(true);
                            //hammer.SetActive(true);
                        }

                        if (health != null && ammo != null)
                        {
                            health.transform.localScale *= sliderSize;
                            ammo.transform.localScale *= sliderSize;
                        }
                        check = false;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    gameObject.SetActive(false);
                }

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (found)
        {
            //weaponNum = playerCharacter.GetComponent<PlayerBody>().getWeaponIndex();
            playerHealth = playerCharacter.GetComponent<PlayerBody>().getHealth();
            playerLives = playerCharacter.GetComponent<PlayerBody>().getLives();
            playerMoney = playerCharacter.GetComponent<PlayerBody>().getMoney();
            //WeaponIcon();
            Hearts();
            HealthBar();
            Money();

            if (health != null && ammo != null)
            {
                if (playerCharacter.activeSelf)
                {

                    ammoSlider.maxValue = playerCharacter.GetComponent<PlayerBody>().maxAmmo;
                    ammoSlider.value = playerCharacter.GetComponent<PlayerBody>().currAmmo;

                    Vector2 playerPos = playerCharacter.transform.position;

                    health.transform.position = Camera.main.WorldToScreenPoint(new Vector2(playerPos.x, playerPos.y + sliderHeight));
                    health.transform.position -= new Vector3(sliderXOffset, 0, 0);
                    ammo.transform.position = Camera.main.WorldToScreenPoint(new Vector2(playerPos.x, playerPos.y + sliderHeight));
                    ammo.transform.position += new Vector3(sliderXOffset, 0, 0);
                }
                else
                {
                    health.SetActive(false);
                    ammo.SetActive(false);
                }
            }
        }
    }

    private void Money()
    {
        moneyText.text = playerMoney.ToString();
    }

    private void HealthBar()
    {
        sliderHealth = playerHealth;
        healthSlider.value = sliderHealth;
        //if(playerHealth == 0)
        //{
        //playerLives--;

        //}
    }


    private void Hearts()
    {
        if (playerLives == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
        }
        if (playerLives == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
        }
        if (playerLives == 1)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
        }
        if (playerLives == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);

        }
    }
}

   /* private void WeaponIcon()
    {
       if(weaponNum == 0)
        {
            DisableAll();
            if (characterNum == 0)
            {
                fist.SetActive(true);
            }
            if (characterNum == 1)
            {
                katana.SetActive(true);
            }
            if (characterNum == 2)
            {
                knife.SetActive(true);
            }
            if (characterNum == 3)
            {
                hammer.SetActive(true);
            }
        }
       if(weaponNum == 1)
        {
            DisableAll();
            pistol.SetActive(true);
        }
       if (weaponNum == 2)
        {
            DisableAll();
            shotgun.SetActive(true);
        }
       if (weaponNum == 3)
        {
            DisableAll();
            shuriken.SetActive(true);
        }
    }

    private void DisableAll()
    {
        fist.SetActive(false);
        katana.SetActive(false);
        knife.SetActive(false);
        hammer.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        shuriken.SetActive(false);
        crossbow.SetActive(false);
        railgun.SetActive(false);
        rifle.SetActive(false);
    }

}
   */
