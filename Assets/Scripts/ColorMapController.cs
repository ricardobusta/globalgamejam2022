using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ColorMapController : MonoBehaviour
{
    [SerializeField] private RenderTexture colorMapRt;
    [SerializeField] private Camera cam;
    [SerializeField] private EnemySpawner enemySpawner;
    

    private Texture2D _colorMap;

    private void Awake()
    {
        _colorMap = new Texture2D(colorMapRt.width, colorMapRt.height, colorMapRt.graphicsFormat, TextureCreationFlags.None);
    }

    private void Update()
    {
        ReadColor();

        foreach (var enemy in enemySpawner.EnemyPool)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.SetEnvironmentColor(CheckIfPointIsLight(enemy.transform.position));
            }
        }
    }

    private void ReadColor()
    {
        RenderTexture.active = colorMapRt;

        _colorMap.ReadPixels(new Rect(0, 0, colorMapRt.width, colorMapRt.height), 0, 0);
        _colorMap.Apply();

        RenderTexture.active = null;
    }

    private bool CheckIfPointIsLight(Vector3 p)
    {
        var pos = cam.WorldToScreenPoint(p);
        pos.x = (pos.x + (Screen.height - Screen.width)/2f) / Screen.height;
        pos.y /= Screen.height;

        if (pos.x >= 0 && pos.x < 1 && pos.y >= 0 && pos.y < 1)
        {
            pos.x *= _colorMap.width;
            pos.y *= _colorMap.height;
            
            var color = _colorMap.GetPixel((int)pos.x, (int)pos.y);

            return color.r > 0.5f;
        }
        
        return false; // since enemies on the edge are out of range, this do not matter much.
    }
}
