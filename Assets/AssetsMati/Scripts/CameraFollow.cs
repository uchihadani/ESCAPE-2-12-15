using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // El personaje a seguir
    [SerializeField] Transform _target;

    // Velocidad de seguimiento
    [SerializeField] float _smoothSpeed = 5f;

    //Posicion relativa
    Vector3 _offset;

    Vector3 _desiredPosition;
    Vector3 _smoothedPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - _target.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null) return;

        // Posicion deseada con offset
        _desiredPosition = _target.position + _offset;

        // Interpolacion suave 
        _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed * Time.deltaTime * 10f);
        transform.position = _smoothedPosition;
    }
}

