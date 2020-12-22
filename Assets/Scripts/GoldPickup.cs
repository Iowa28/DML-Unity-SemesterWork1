using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    [SerializeField]
    private int value;

    [SerializeField]
    private GameObject pickupEffect;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().AddGold(value);

            GameObject effect = (GameObject) Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(effect, 3f);

            Destroy(gameObject);
        }
    }
}
