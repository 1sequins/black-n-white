using System;
using UnityEngine;

public class RotacionCamara : MonoBehaviour
{
	public Transform objeto;
	private Quaternion rotacion;
	private Vector3 posicion;
	public float distancia = 60f;
	public float velocidadX = 400f;
	public float anguloX;
	public float anguloX_actual;
	public float angulo_maximo = 10;
	public bool desdeArriba = false;
	
	private void Update ()
	{
		if (Input.GetMouseButton (0)) {
			anguloX = anguloX + Input.GetAxis ("Mouse X") * velocidadX * 0.02f;
		}
		if (Input.GetMouseButtonUp (1)) {
			desdeArriba = !desdeArriba;
		}

		anguloX_actual = Mathf.Lerp (anguloX_actual, anguloX, Time.deltaTime * 2);

		if (anguloX < 0) {
			if (angulo_maximo > anguloX)
				angulo_maximo = anguloX;
		} else if (anguloX > 0) {
			if (-angulo_maximo < anguloX) 
				angulo_maximo = -anguloX; // Se pone menos ,xke anguloX ya es negativo, asi angulo maximo siempre es positivo
		}

		if (desdeArriba)
			rotacion = Quaternion.Euler (90, anguloX_actual, 0);
		else
			rotacion = Quaternion.Euler (55, anguloX_actual, 0);

		
		posicion = (rotacion * (new Vector3 (0, 0, - distancia))) + objeto.position;
		
		transform.rotation = rotacion;
		
		transform.position = posicion;
	}
}