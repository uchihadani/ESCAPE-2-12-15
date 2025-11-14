using UnityEngine;

public class DestruirObjeto : MonoBehaviour, IInteractuable
{
    [SerializeField] private string requiredItemId = "Hammer";
    [SerializeField] private AudioClip destruirPared;
    public void Interactuar(GameObject jugador)
    {

        Inventory playerInventory = jugador.GetComponent<Inventory>();

        if (playerInventory != null)
        {
            
            if (playerInventory.HasItem(requiredItemId))
            {

                SoundManager.instance.PlaySound(destruirPared);
                Destroy(gameObject);
            }
          
        }
       
    }
}


