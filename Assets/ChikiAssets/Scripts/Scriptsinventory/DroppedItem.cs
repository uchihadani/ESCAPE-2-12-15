using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool autoStart;
    [SerializeField] float enabledPickupDelay = 3f;

    [Header("State")]
    public Item item;
    public bool pickedUp = false;

    private bool playerInside = false;
    private Inventory playerInventory;

    void Start()
    {
        if (autoStart && item != null)
        {
            Initialize(item);
        }
    }

    public void Initialize(Item item)
    {
        this.item = item;

        var droppedItem = Instantiate(item.prefab, transform);
        droppedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        StartCoroutine(EnablePickup(enabledPickupDelay));
    }

    IEnumerator EnablePickup(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerInventory = other.GetComponent<Inventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerInventory = null;
        }
    }

    void Update()
    {
        if (playerInside && !pickedUp && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        pickedUp = true;
        playerInventory.AddItemFromWorld(item); 
        Destroy(gameObject);
    }
}