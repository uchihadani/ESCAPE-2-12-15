using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool estaEscondido { get; private set; }

    public void EntrarEscondite()
    {
        estaEscondido = true;
    }

    public void SalirEscondite()
    {
        estaEscondido = false;
    }
}
