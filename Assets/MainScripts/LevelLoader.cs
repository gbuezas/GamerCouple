using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color32 color;
    public GameObject prefab;
}

public class LevelLoader : MonoBehaviour {
    
    public Texture2D LevelMap;
    public ColorToPrefab[] colorToPrefab;
    
    // Use this for initialization
	void Start () {
        LoadMap();
	}

    // Busca todos los hijos y los elimina
    void EmptyMap()
    {
        while (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(null);
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    void LoadMap()
    {
        EmptyMap();

        // Obtener los pixeles del mapa
        Color32[] allPixels = LevelMap.GetPixels32();
        int width = LevelMap.width;
        int height = LevelMap.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTileAt(allPixels[(y * width) + x], x, y);
            }
        }
    }

    void SpawnTileAt(Color32 c, int x, int y)
    {
        // Si es transparente lo salteamos
        if (c.a <= 0)
        {
            return;
        }

        // Encontrar el color correcto en nuestro mapa
        foreach (ColorToPrefab ctp in colorToPrefab)
        {
            if (c.Equals(ctp.color))
            {
                // Colocar el prefab en el lugar indicado
                GameObject go = Instantiate(ctp.prefab, new Vector3(x, y, 0), rotation: Quaternion.identity);
                go.transform.SetParent(this.transform);
                return;
            }
        }

        // Si llegamos a este punto quiere decir que el color no se encontro
        Debug.LogError("El siguiente color no pudo ser localizado: " + c.ToString());
    }
}
