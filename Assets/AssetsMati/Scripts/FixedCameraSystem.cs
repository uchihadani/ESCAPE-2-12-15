using System.Collections.Generic;
using UnityEngine;

public class FixedCameraSystem : MonoBehaviour
{
    [Header("Lista de posiciones de cámara")]
    [SerializeField] private List<Transform> cameraPoints = new List<Transform>(); // Puntos donde puede colocarse la cámara
    [SerializeField] private float switchRadius = 8f;  // Rango en el que la cámara cambia
    [SerializeField] private float smoothSpeed = 5f;   // Velocidad de transición

    private Transform player;
    private Transform currentCameraPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Si hay puntos, tomar el primero como inicial
        if (cameraPoints.Count > 0)
        {
            currentCameraPoint = cameraPoints[0];
            MoveCameraInstant(currentCameraPoint);
        }
    }

    void Update()
    {
        Transform nearestPoint = GetNearestPoint();

        // Si el jugador está dentro del rango de un nuevo punto → cambiar de cámara
        if (nearestPoint != currentCameraPoint &&
            Vector3.Distance(player.position, nearestPoint.position) <= switchRadius)
        {
            currentCameraPoint = nearestPoint;
        }

        // Transición suave hacia la cámara activa
        if (currentCameraPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, currentCameraPoint.position, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentCameraPoint.rotation, smoothSpeed * Time.deltaTime);
        }
    }

    // Encuentra el punto de cámara más cercano al jugador
    private Transform GetNearestPoint()
    {
        Transform nearest = cameraPoints[0];
        float minDist = Vector3.Distance(player.position, nearest.position);

        foreach (Transform point in cameraPoints)
        {
            float dist = Vector3.Distance(player.position, point.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = point;
            }
        }

        return nearest;
    }

    // Mueve instantáneamente la cámara (para el inicio)
    private void MoveCameraInstant(Transform point)
    {
        transform.position = point.position;
        transform.rotation = point.rotation;
    }
}
