using UnityEngine;

[CreateAssetMenu(fileName = "ObjetoData", menuName = "Scriptable Objects/ObjetoData")]
public class ObjetoData : ScriptableObject
{
    public string nombre;
    public GameObject prefab;
    public Sprite icono;
    public Objeto.TipoObjeto tipo;
    public Vector3 escalaOriginal;
}
