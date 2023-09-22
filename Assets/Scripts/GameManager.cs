using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public GameObject enemigo;
  public Vector3 posicion;
  public int numeroEnemigos;
  public float esperaInicial;
  public float esperaEntreEnemigos;
  public float esperaEntreOlas;

  void Start() {  

    //LLamo a la rutina de crear enemigos
    StartCoroutine(crearEnemigos());

  }

  IEnumerator crearEnemigos()
  {
    //Espero un tiempo antes de crear enemigos
    yield return new WaitForSeconds(esperaInicial);

    //Bucle durante toda la vida del juego
    while (true)
    {
      //Bucle de número de enemigos
      for (int i = 0; i < numeroEnemigos; i++)
      {
        //Instancio el enemigo en una posición aleatoria del tablero
        Vector3 posicionEnemigo = new Vector3(Random.Range(-posicion.x, posicion.x), posicion.y, Random.Range(-posicion.z, posicion.z));
        Instantiate(enemigo, posicionEnemigo, Quaternion.identity);

        //Espero un tiempo entre la creación de cada enemigo
        yield return new WaitForSeconds(esperaEntreEnemigos);
      }

      //Espero un tiempo entre oleadas de enemigos
      yield return new WaitForSeconds(esperaEntreOlas);
    }
  }

}