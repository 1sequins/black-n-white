using UnityEngine;
using System.Collections;

public class Cubo
{
	int xMapa, zMapa;
	float x, altura, z;
	GameObject cub;
	public int nivel = 0;
	float siceX = 0.3f;
	float siceZ = 0.3f;
	float ScalaY = 0.0f;
	public bool maligno;
	public bool borrado;
	public static int nivelLimite = 8; 

	public enum Direccion
	{
		arriba,
		abajo,
		izquierda,
		derecha}
	;
	Movimiento movimiento;

	public Cubo (float x, float y, float z, int xMapa, int zMapa)
	{
        
		this.xMapa = xMapa;
		this.zMapa = zMapa;
		this.x = x;
		this.altura = y;
		this.z = z;
		maligno = false;
		crearCubo (x, y, z);
		cub.GetComponent<Renderer>().material.color = Color.blue;
		actualizarNivel ();
		movimiento = new Movimiento (0, Direccion.abajo);
	}

	public void setNivel (int nivel)
	{
		this.nivel = nivel;
	}

	static float velocidadAumento = 0.1f;

	public bool Update ()
	{
		bool transicion = false;
		if (!movimiento.isFin ()) {
			PointF p = movimiento.Update (new PointF (x, z));
			x = p.x;
			z = p.y;
			transicion = true;
		}
		if (siceX < 1 - velocidadAumento) {
			siceX += velocidadAumento;
			actualizarNivel ();
			transicion = true;
		}
		if (siceZ < 1 - velocidadAumento) {
			siceZ += velocidadAumento;
			actualizarNivel ();
			transicion = true;
		}

		if (ScalaY < 0.9) {
			ScalaY += 0.1f;
			actualizarNivel ();
		} else if (ScalaY > 1f) {
			ScalaY -= 0.1f;
			actualizarNivel ();
		} else {
			ScalaY = 1;
			actualizarNivel ();
		}
		actualizarPosicion ();
		return transicion;
	}

	public void crearCubo (float x, float y, float z)
	{
		cub = Fabrica.obtenerCubo ();
		cub.transform.position = new Vector3 (x, y, z);
		cub.transform.localScale = new Vector3 (siceX, 0.01f, siceZ);
	}

	public void destruir ()
	{
		Fabrica.reciclar (ref this.cub);
		cub = null;
	}

	public void actualizarNivel ()
	{
		actualizarPosicion ();
		cub.transform.localScale = new Vector3 (siceX, ((nivel == 0) ? 0.01f : nivel) * ScalaY, siceZ);
		cambiarColor (nivel);
	}

	public void actualizarPosicion ()
	{
		cub.transform.position = new Vector3 (x, (nivel + altura) * ScalaY / 2, z);
	}

	Color[] colores = {
		Color.blue,
		Color.green,
		Color.cyan,
		Color.gray,
		Color.yellow,
		Color.red
	};

	public bool fueEliminado(){
		return nivel==0&&ScalaY==1;
	}

	public void cambiarColor (int nivel)
	{
		if (maligno)
			cub.GetComponent<Renderer>().material.color = Color.black;
		else
			cub.GetComponent<Renderer>().material.color = colores [nivel % 6];
	}
	 
	public void moverHacia (Direccion d)
	{
		switch (d) {
		case Direccion.arriba:
			movimiento.restart (arriba (), d);
			break;
		case Direccion.abajo:
			movimiento.restart (abajo (), d);
			break;
		case Direccion.izquierda:
			movimiento.restart (izquierda (), d);
			break;
		case Direccion.derecha:
			movimiento.restart (derecha (), d);
			break;
		default:
			break;
		}



	}

	public float arriba ()
	{	
		float desplazamiento = 0;
		int zAnterior = -1;
		for (int nuevaZ = zMapa+1; nuevaZ<main.tamanoMapa; nuevaZ++) {
			zAnterior = zMapa;
			if (!existeCubo (xMapa, nuevaZ)) {
				desplazamiento += main.tamanoCubo;
				zMapa = nuevaZ;
				main.mapa [xMapa, zMapa] = main.mapa [xMapa, zAnterior];
				main.mapa [xMapa, zAnterior] = null;
			} else if ((main.mapa [xMapa, nuevaZ].nivel < nivelLimite) && nivel == main.mapa [xMapa, nuevaZ].nivel) {
				if (main.mapa [xMapa, nuevaZ].maligno == maligno) {
					nivel *= 2;
					ScalaY /= 2;
				} else {
					nivel /= 2;
					ScalaY *= 2;
				}
				desplazamiento += main.tamanoCubo;
				zMapa = nuevaZ;
				main.mapa [xMapa, nuevaZ].destruir ();
				main.mapa [xMapa, nuevaZ] = main.mapa [xMapa, zAnterior];
				main.mapa [xMapa, zAnterior] = null;
				return desplazamiento;
			} else {
				return desplazamiento;
			}
		}
		return desplazamiento;

	}

	public float abajo ()
	{
		float desplazamiento = 0;	
		int zAnterior = -1;
		for (int nuevaZ = zMapa-1; nuevaZ>=0; nuevaZ--) {
			zAnterior = zMapa;
			if (!existeCubo (xMapa, nuevaZ)) {
				desplazamiento += main.tamanoCubo;
				zMapa = nuevaZ;
				main.mapa [xMapa, zMapa] = main.mapa [xMapa, zAnterior];
				main.mapa [xMapa, zAnterior] = null;
			} else if ((main.mapa [xMapa, nuevaZ].nivel < nivelLimite) && nivel == main.mapa [xMapa, nuevaZ].nivel) {
				if (main.mapa [xMapa, nuevaZ].maligno == maligno) {
					nivel *= 2;
					ScalaY /= 2;
				} else {
					nivel /= 2;
					ScalaY *= 2;
				}
				desplazamiento += main.tamanoCubo;
				zMapa = nuevaZ;
				main.mapa [xMapa, nuevaZ].destruir ();
				main.mapa [xMapa, nuevaZ] = main.mapa [xMapa, zAnterior];
				main.mapa [xMapa, zAnterior] = null;
				return desplazamiento;
			} else {
				return desplazamiento;
			}
		}
		return desplazamiento;
	}

	public float derecha ()
	{
		float desplazamiento = 0;	
		int xAnterior = -1;
		for (int nuevaX = xMapa+1; nuevaX<main.tamanoMapa; nuevaX++) {
			xAnterior = xMapa;
			if (!existeCubo (nuevaX, zMapa)) {
				desplazamiento += main.tamanoCubo;
				xMapa = nuevaX;
				main.mapa [xMapa, zMapa] = main.mapa [xAnterior, zMapa];
				main.mapa [xAnterior, zMapa] = null;
			} else if ((main.mapa [nuevaX, zMapa].nivel < nivelLimite) && nivel == main.mapa [nuevaX, zMapa].nivel) {
				if (main.mapa [nuevaX, zMapa].maligno == maligno) {
					nivel *= 2;
					ScalaY /= 2;
				} else {
					nivel /= 2;
					ScalaY *= 2;
				}
				desplazamiento += main.tamanoCubo;
				xMapa = nuevaX;
				main.mapa [nuevaX, zMapa].destruir ();
				main.mapa [nuevaX, zMapa] = main.mapa [xAnterior, zMapa];
				main.mapa [xAnterior, zMapa] = null;
				return desplazamiento;
			} else {
				return desplazamiento;
			}
		}
		return desplazamiento;
	}
	
	public float izquierda ()
	{
		float desplazamiento = 0;	
		int xAnterior = -1;
		for (int nuevaX = xMapa-1; nuevaX>=0; nuevaX--) {
			xAnterior = xMapa;
			if (!existeCubo (nuevaX, zMapa)) {
				desplazamiento += main.tamanoCubo;
				xMapa = nuevaX;
				main.mapa [xMapa, zMapa] = main.mapa [xAnterior, zMapa];
				main.mapa [xAnterior, zMapa] = null;
			} else if ((main.mapa [nuevaX, zMapa].nivel < nivelLimite) && nivel == main.mapa [nuevaX, zMapa].nivel) {
				if (main.mapa [nuevaX, zMapa].maligno == maligno) {
					nivel *= 2;
					ScalaY /= 2;
				} else {
					nivel /= 2;
					ScalaY *= 2;
				}
				desplazamiento += main.tamanoCubo;
				xMapa = nuevaX;
				main.mapa [nuevaX, zMapa].destruir ();
				main.mapa [nuevaX, zMapa] = main.mapa [xAnterior, zMapa];
				main.mapa [xAnterior, zMapa] = null;
				return desplazamiento;
			} else {
				return desplazamiento;
			}
		}
		return desplazamiento;
	}

	bool existeCubo (int x, int y)
	{
		bool existe = main.mapa [x, y] != null;
		main.print ("existeCubo(" + x + "," + y + ") = " + existe);
		return existe;
	}



}


