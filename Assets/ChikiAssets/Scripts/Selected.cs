using UnityEngine;

public class Selected : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    public float distancia = 1.5f;

    private IInteractuable esconditeActual = null; 

    void Update()
    {
       
        if (esconditeActual != null && Input.GetKeyDown(KeyCode.E))
        {
            esconditeActual.Interactuar(gameObject);
            esconditeActual = null;
            return;
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distancia, mask))
        {
            if (hit.collider.CompareTag("Objeto Interactivo"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    IInteractuable interactuable = hit.collider.GetComponent<IInteractuable>();
                    if (interactuable != null)
                    {
                        interactuable.Interactuar(gameObject);

                        if (interactuable is Escondite)
                        {
                            esconditeActual = interactuable;
                        }
                    }
                }
            }
        }
    }
}

