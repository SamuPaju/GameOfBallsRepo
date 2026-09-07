using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject player; // Pelaajapallo scenessä

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Jos pelaajan y arvo on suurempi kuin platformin arvo
        // platformin layer muutetaan actiiviseksi
        if (player.transform.position.y - transform.localScale.y / 2 - player.transform.localScale.y / 2 > transform.position.y)
        {
            gameObject.layer = LayerMask.NameToLayer("PlatformActive");
            // Muutetaan platformin väri
            GetComponent<Renderer>().material.color = Color.cyan;
        }
    }
}
