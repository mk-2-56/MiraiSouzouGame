using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HeightmapFromTexture : MonoBehaviour
{
    [MenuItem("Terrain/Heightmap From Texture")]
    public static void ShowWindow()
    {
        // カスタムウィンドウを表示
        HeightmapInputWindow window = ScriptableObject.CreateInstance<HeightmapInputWindow>();
        window.ShowUtility();
    }
}

public class HeightmapInputWindow : EditorWindow
{
    private string terrainName = "MyTerrainName"; // デフォルトのTerrain名
    private Texture2D heightmap;

    private void OnGUI()
    {
        GUILayout.Label("Heightmap From Texture", EditorStyles.boldLabel);

        // Terrain名の入力フィールド
        terrainName = EditorGUILayout.TextField("Terrain Name:", terrainName);

        // テクスチャの選択
        heightmap = EditorGUILayout.ObjectField("Heightmap Texture:", heightmap, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Apply Heightmap"))
        {
            ApplyHeightmap();
        }
    }

    private void ApplyHeightmap()
    {
        if (heightmap == null)
        {
            EditorUtility.DisplayDialog("No texture selected", "Please select a texture.", "Cancel");
            return;
        }

        // 指定された名前のTerrainを検索
        Terrain myTerrain = GameObject.Find(terrainName)?.GetComponent<Terrain>();
        if (myTerrain == null)
        {
            EditorUtility.DisplayDialog("Terrain not found", $"No Terrain found with the name '{terrainName}'.", "OK");
            return;
        }

        Undo.RegisterCompleteObjectUndo(myTerrain.terrainData, "Heightmap From Texture");

        TerrainData terrain = myTerrain.terrainData;
        int w = heightmap.width;
        int h = heightmap.height;
        int w2 = terrain.heightmapResolution;
        float[,] heightmapData = terrain.GetHeights(0, 0, w2, w2);
        Color[] mapColors = heightmap.GetPixels();
        Color[] map = new Color[w2 * w2];

        if (w2 != w || h != w)
        {
            // Resize using nearest-neighbor scaling if texture has no filtering
            if (heightmap.filterMode == FilterMode.Point)
            {
                float dx = w / w2;
                float dy = h / w2;
                for (int y = 0; y < w2; y++)
                {
                    if (y % 20 == 0)
                    {
                        EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
                    }
                    float thisY = (dy * y) * w;
                    int yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        map[yw + x] = mapColors[(int)(thisY + dx * x)];
                    }
                }
            }
            // Otherwise resize using bilinear filtering
            else
            {
                float ratioX = 1.0f / ((float)w2 / (w - 1));
                float ratioY = 1.0f / ((float)w2 / (h - 1));
                for (int y = 0; y < w2; y++)
                {
                    if (y % 20 == 0)
                    {
                        EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
                    }
                    float yy = Mathf.Floor((float)(y * ratioY));
                    float y1 = yy * w;
                    float y2 = (yy + 1) * w;
                    float yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        float xx = Mathf.Floor((float)(x * ratioX));

                        Color bl = mapColors[(int)(y1 + xx)];
                        Color br = mapColors[(int)(y1 + xx + 1)];
                        Color tl = mapColors[(int)(y2 + xx)];
                        Color tr = mapColors[(int)(y2 + xx + 1)];

                        float xLerp = x * ratioX - xx;
                        map[(int)(yw + x)] = Color.Lerp(Color.Lerp(bl, br, xLerp), Color.Lerp(tl, tr, xLerp), y * ratioY - yy);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
        else
        {
            // Use original if no resize is needed
            map = mapColors;
        }

        // Assign texture data to heightmap
        for (int y = 0; y < w2; y++)
        {
            for (int x = 0; x < w2; x++)
            {
                heightmapData[y, x] = map[y * w2 + x].grayscale;
            }
        }
        terrain.SetHeights(0, 0, heightmapData);
    }
}
