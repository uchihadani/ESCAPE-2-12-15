using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;          // Arrastrá tu AudioSource aquí
    public AudioClip[] pasos;                // Lista de sonidos de pasos
    public float tiempoEntrePasos = 0.5f;    // Tiempo entre cada paso

    private CharacterController controller;
    private float pasoTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Si el jugador se está moviendo
        if (controller.isGrounded && controller.velocity.magnitude > 0.2f)
        {
            pasoTimer -= Time.deltaTime;

            if (pasoTimer <= 0f)
            {
                // Elegir sonido aleatorio
                AudioClip clip = pasos[Random.Range(0, pasos.Length)];
                audioSource.PlayOneShot(clip);

                pasoTimer = tiempoEntrePasos; // reinicia el contador
            }
        }
        else
        {
            pasoTimer = 0f; // resetea cuando no se mueve
        }
    }
}
