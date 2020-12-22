using UnityEngine;
using UnityEngine.Events;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField]
    private int damageToGive = 1;
    [SerializeField]
    private UnityEvent OnHurt;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            FindObjectOfType<HealthManager>().HurtPlayer(damageToGive, hitDirection);
            OnHurt.Invoke();
        }
    }
}
