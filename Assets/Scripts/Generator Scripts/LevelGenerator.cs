using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;
    [Range(0.0f, 1.0f)]
    public float mapScale;
    public ColorToPrefab[] colorMappings;
	string imgPath;

    float scaleOffset;

	void Awake() {
		imgPath = PlayerPrefs.GetString("img_path");
		map = LoadImage(imgPath);
        scaleOffset = (1 - mapScale);

        GenerateLevel();

	}

	void GenerateLevel() {
		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.height; y++) {
				GenerateTile(x,y);
			}
		}
	}

	void GenerateTile(int x, int y) {
		Color pixelColor = map.GetPixel(x, y);
		if(pixelColor.a == 0) {
			return;
		}

		foreach(ColorToPrefab colorMapping in colorMappings) {
			if(colorMapping.color.Equals(pixelColor)) {
				Vector2 position = new Vector2(x-(int)(map.width/2), y-(int)(map.height/2)) * mapScale + (Vector2)transform.position;
				Transform tile = Instantiate(colorMapping.prefab, position, Quaternion.identity, transform).transform;
                tile.localScale = new Vector3(mapScale, mapScale, mapScale);
			}
		}

	}

	private static Texture2D LoadImage(string filePath)
    {

        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(50, 50, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(bytes);
        texture.Apply();

        return texture;
    }

}
