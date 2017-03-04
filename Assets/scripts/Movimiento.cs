
using System;

public class Movimiento
{

	Cubo.Direccion direccion;
	float desplazamiento;
	bool fin;
	public float constanteParada= 0.05f;

	public Movimiento (float desplazamiento, Cubo.Direccion direccion)
	{
		this.desplazamiento = desplazamiento;
		this.direccion = direccion;
		fin = false;
	}

	public void restart (float desplazamiento, Cubo.Direccion direccion)
	{
		this.desplazamiento = desplazamiento;
		this.direccion = direccion;
		fin = false;
	}

	public bool isFin ()
	{
		return fin;
	}

	public PointF Update (PointF pos)
	{
		if (!fin) {
			pos = Desplazar (pos);
		}
		return pos;
	}

	public PointF Desplazar (PointF pos)
	{
		float avance = desplazamiento * 0.3f;
		desplazamiento -= avance;
		switch (direccion) {
		case Cubo.Direccion.arriba:
			pos.y += avance;
			if (desplazamiento < constanteParada) {
				pos.y += desplazamiento;
				desplazamiento = 0;
				fin = true;
			}
			break;
		case Cubo.Direccion.abajo:
			pos.y -= avance;
			if (desplazamiento < constanteParada) {
				pos.y -= desplazamiento;
				desplazamiento = 0;
				fin = true;
			}
			break;
		case Cubo.Direccion.derecha:
			pos.x += avance;
			if (desplazamiento < constanteParada) {
				pos.x += desplazamiento;
				desplazamiento = 0;
				fin = true;
			}
			break;
		case Cubo.Direccion.izquierda:
			pos.x -= avance;
			if (desplazamiento < constanteParada) {
				pos.x -= desplazamiento;
				desplazamiento = 0;
				fin = true;
			}
			break;
		}
		return pos;
	}

	public float Distancia (PointF p1, PointF p2)
	{
		return (float)Math.Sqrt (Math.Pow (p1.x - p2.x, 2) + (Math.Pow (p1.y - p2.y, 2)));
	}

}

public struct PointF
{
	public float x, y;
	
	public PointF (float px, float py)
	{
		x = px;
		y = py;
	}
	
	public void set (float xx, float yy)
	{
		x = xx;
		x = yy;
	}
}

public interface ObjetoMovil
{
	
	PointF getPosActualF ();
}

