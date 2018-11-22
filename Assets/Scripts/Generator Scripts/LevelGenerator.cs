using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;
	public ColorToPrefab[] colorMappings;
	string imgPath;

    public Vector2 mapScale = new Vector2(100,50);

	void Awake() {
		imgPath = PlayerPrefs.GetString("img_path");

        map = ImageLoader.LoadImage(new Vector2(50, 50), imgPath);
        TextureScale.Point(map, (int)mapScale.x, (int)mapScale.y);
    }

	void Start() {
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
				Vector2 position = new Vector2(x-(int)(map.width/2), y-(int)(map.height/2));
				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
			}
		}

	}
}
