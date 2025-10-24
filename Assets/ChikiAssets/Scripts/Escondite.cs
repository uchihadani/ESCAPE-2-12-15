using UnityEngine;

public class Escondite : MonoBehaviour, IInteractuable
{
    [SerializeField] private Transform puntoDentro;
    [SerializeField] private Transform puntoFuera;
    private bool jugadorDentro = false;

    public void Interactuar(GameObject jugador)
    {
        CharacterController controller = jugador.GetComponent<CharacterController>();
        PlayerMovement movimiento = jugador.GetComponent<PlayerMovement>();
        PlayerState estado = jugador.GetComponent<PlayerState>();

        if (controller != null)
        {
            controller.enabled = false;

            if (!jugadorDentro)
            {
                
                jugador.transform.position = puntoDentro.position;
                if (movimiento != null) movimiento.enabled = false;
                if (estado != null) estado.EntrarEscondite(); 
                jugadorDentro = true;
            }
            else
            {
                
                jugador.transform.position = puntoFuera.position;
                if (movimiento != null) movimiento.enabled = true;
                if (estado != null) estado.SalirEscondite(); 
                jugadorDentro = false;
            }

            controller.enabled = true;
        }
    }
}

