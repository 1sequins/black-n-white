using UnityEngine;
using System.Collections;

public class main : MonoBehaviour
{
    public static Cubo[,] mapa;
    public static int tamanoMapa = 5;
    public static int tamanoCubo = 1;
    public Transform objetivoCamara;
    public Transform norte;

    private void Start()
    {
        int cantidadCubosIniciales = 2;
        objetivoCamara.transform.position = new Vector3(tamanoMapa/2, 3f, tamanoMapa/2);
        norte.transform.position = new Vector3(tamanoMapa/2 + tamanoCubo/2, 0, tamanoMapa);
        norte.GetComponent<Renderer>().material.color = Color.red;

        mapa = new Cubo[tamanoMapa, tamanoMapa];
        for (int i = 0; i < tamanoMapa; i++)
        {
            for (int j = 0; j < tamanoMapa; j++)
            {
                crearBase(i, j);
                //				int nivel = Random.Range (0, 5);
                int nivel = 1;
                if (nivel > 0 && cantidadCubosIniciales > 0)
                {
                    Cubo c = new Cubo(i*tamanoCubo, 0, j*tamanoCubo, i, j);
                    c.nivel = nivel;
                    mapa[i, j] = c;
                    cantidadCubosIniciales--;
                }
            }
        }
    }

    private void insertarCubo(bool maligno)
    {
        int i = Random.Range(0, tamanoMapa);
        int j = Random.Range(0, tamanoMapa);
        if (mapa[i, j] == null)
        {
            if (!maligno)
            {
                Cubo c = new Cubo(i*tamanoCubo, 0, j*tamanoCubo, i, j);
                c.maligno = true;
                c.nivel = 1;
                mapa[i, j] = c;
            }
            else
            {
                //				int nivel = Random.Range (1, 5);

                Cubo c = new Cubo(i*tamanoCubo, 0, j*tamanoCubo, i, j);
                c.nivel = 1;
                mapa[i, j] = c;
            }
        }
    }

    private void crearBase(int x, int z)
    {
        var cub = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cub.transform.position = new Vector3(x, -0.01f, z);
        cub.transform.localScale = new Vector3(tamanoCubo - 0.01f, 0.01f, tamanoCubo - 0.01f);
    }

    private void Update()
    {
        bool trancicion = false;
        for (int i = 0; i < tamanoMapa; i++)
        {
            for (int j = 0; j < tamanoMapa; j++)
            {
                if (null != mapa[i, j])
                {
                    if (mapa[i, j].Update())
                    {
                        trancicion = true;
                    }
                    if (mapa[i, j].fueEliminado())
                    {
                        mapa[i, j].destruir();
                        mapa[i, j] = null;
                    }
                }
            }
        }
        if (!trancicion)
        {
            realizarJugada();
        }
    }

    private void moverPorTag(string tag, int x, int y, int z)
    {
        var aObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject objeto in aObjects)
        {
            objeto.transform.Translate(x, y, z);
        }
    }

    private void moverCubo(Cubo.Direccion d)
    {
        print("Mover: " + d);
        switch (d)
        {
            case Cubo.Direccion.arriba:
                for (int x = 0; x < tamanoMapa; x++)
                {
                    for (int z = tamanoMapa - 1; z >= 0; z--)
                    {
                        if (null != mapa[x, z])
                        {
                            mapa[x, z].moverHacia(d);
                        }
                    }
                }
                break;
            case Cubo.Direccion.abajo:
                for (int x = 0; x < tamanoMapa; x++)
                {
                    for (int z = 0; z < tamanoMapa; z++)
                    {
                        if (null != mapa[x, z])
                        {
                            mapa[x, z].moverHacia(d);
                        }
                    }
                }
                break;
            case Cubo.Direccion.derecha:
                for (int x = 0; x < tamanoMapa; x++)
                {
                    for (int z = tamanoMapa - 1; z >= 0; z--)
                    {
                        if (null != mapa[z, x])
                        {
                            mapa[z, x].moverHacia(d);
                        }
                    }
                }
                break;
            case Cubo.Direccion.izquierda:
                for (int x = 0; x < tamanoMapa; x++)
                {
                    for (int z = 0; z < tamanoMapa; z++)
                    {
                        if (null != mapa[z, x])
                        {
                            mapa[z, x].moverHacia(d);
                        }
                    }
                }
                break;
            default:
                break;
        }

        insertarCubo(Random.Range(0, 10)%2 == 0);
    }


    private void realizarJugada()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            moverCubo(Cubo.Direccion.arriba);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            moverCubo(Cubo.Direccion.abajo);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            moverCubo(Cubo.Direccion.derecha);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            moverCubo(Cubo.Direccion.izquierda);
        }
    }
}

//	void rotarPorTag (string tag)
//	{
//		var aObjects = GameObject.FindGameObjectsWithTag (tag);
//		foreach (GameObject objeto in aObjects) {
//			objeto.transform.Rotate (0, Time.deltaTime * 50, 0);
//		}
//	}