using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private Text healthText;

    [Header("Invincibility")]
    [SerializeField]
    private PlayerController thePlayer;

    [SerializeField]
    private float invincibilityLength;
    private float invincibilityCounter;

    [SerializeField]
    private Renderer playerRenderer;
    private float flashCounter;
    [SerializeField]
    private float flashLength = .1f; 

    [Header("Respawn")]
    private bool isRespawning;
    private Vector3 respawnPoint;
    [SerializeField]
    private float respawnLength;

    [Header("Death")]
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private Image blackScreen;
    private bool isFadeToBlack;
    private bool isFadeFromBlack;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private float waitForFade;

    void Start()
    {
        currentHealth = maxHealth;

        healthText.text = "Health: " + maxHealth;

        respawnPoint = thePlayer.transform.position;
    }

    void Update() 
    {
        if (invincibilityCounter > 0f)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0f)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashLength;
            }

            if (invincibilityCounter <= 0f)
            {
                playerRenderer.enabled = true;
            }
        }

        if (isFadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 1f)
            {
                isFadeToBlack =false;
            }
        }

        if (isFadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                isFadeFromBlack =false;
            }
        }
    }

    public void HurtPlayer(int damage, Vector3 direction)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                healthText.text = "Health: 0";

                Respawn();
            } else
            {
                thePlayer.Knockback(direction);

                invincibilityCounter = invincibilityLength;

                playerRenderer.enabled = false;
                flashCounter = flashLength;

                healthText.text = "Health: " + currentHealth;
            }
        }
    }

    private void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine("RespawnGo");
        }
        //healthText.text = "Health: " + maxHealth;
    }

    private IEnumerator RespawnGo()
    {
        isRespawning = true;
        thePlayer.gameObject.SetActive(false);
        GameObject effect = (GameObject) Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        Destroy(effect, 2f);

        yield return new WaitForSeconds(respawnLength);

        isFadeToBlack = true;

        yield return new WaitForSeconds(waitForFade);

        isFadeToBlack = false;
        isFadeFromBlack = true;

        isRespawning = false;

        thePlayer.gameObject.SetActive(true);

        GameObject player = GameObject.Find("Player");
        CharacterController characterController = thePlayer.GetComponent<CharacterController>();
        characterController.enabled = false;
        thePlayer.transform.position = respawnPoint;
        characterController.enabled = true;

        currentHealth = maxHealth;
        healthText.text = "Health: " + maxHealth;

        invincibilityCounter = invincibilityLength;
        playerRenderer.enabled = false;
        flashCounter = flashLength;
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetSpawnPoint(Vector3 newPosition)
    {
        this.respawnPoint = newPosition;
    }
}
