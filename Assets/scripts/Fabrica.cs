using UnityEngine;
using System.Collections;

public class Fabrica
{
	public static ArrayList listaCubos = new ArrayList ();

	public static GameObject obtenerCubo ()
	{
		GameObject cubo = null;
		if (listaCubos.Count > 0) {
			cubo = (GameObject)listaCubos [0];
			listaCubos.RemoveAt (0);
		} else {
			main.print ("Cubo creado");
            
			cubo = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}
//		cubo.SetActive (true);
		return cubo;
	}

	public static void reciclar (ref GameObject cubo)
	{
		cubo.transform.localScale = new Vector3 (0, 0, 0);
		listaCubos.Add (cubo);

//		cubo.SetActive (false);
	}



}


