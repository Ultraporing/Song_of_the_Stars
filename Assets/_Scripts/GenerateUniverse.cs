using UnityEngine;
using System.Collections;
using Bodies.Stars;
using System.Collections.Generic;
using Bodies;
using Assets._Scripts;
using System;
using System.IO;

public class GenerateUniverse : MonoBehaviour
{
    private Galaxy galaxy = null;
    public Sprite galaxyBG = null;
    public Sprite star = null;
    public int amountStars = 4096;

	// Use this for initialization
	void Start ()
    {
        galaxy = gameObject.AddComponent<Galaxy>();
        galaxy.Generate(amountStars);
        GenerateGalaxyFinalImage();
        /*
        StreamWriter f = File.CreateText("Stars.txt");

        for (int i = 0; i < galaxy.Stars.Length; i++)
        {
            f.WriteLine(galaxy.Stars[i].ToString());
        }

        f.Close();*/
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateGalaxyFinalImage()
    {
        SpriteRenderer overlayRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        Texture2D tex = new Texture2D(galaxyBG.texture.width, galaxyBG.texture.height, TextureFormat.ARGB32, false);

        Color[] colArr = new Color[galaxyBG.texture.GetPixels().Length];
        for (int i = 0; i < colArr.Length; i++)
        {
            colArr[i] = Color.clear;
        }
        
        tex.SetPixels(colArr);

        double cmp = Mathf.Pow(16, 2);

        
        int tries = 25;

        for (int s = 0; s < galaxy.Stars.Length; s++)
        {
            Vector2d r = Vector2d.zero;
            
            int isBump = 0;
            
            do
            {
                bool done = true;

                Vector2 r2 = UnityEngine.Random.insideUnitCircle * 1900;
                r = new Vector2d(r2.x, r2.y);
                for (int i = 0; i < galaxy.Stars.Length; i++)
                {
                    Vector2d v2d = new Vector2d(galaxy.Stars[i].Position.x * 32, galaxy.Stars[i].Position.y * 32);
                    double mag = (v2d - r).sqrMagnitude;
                    if (mag <= cmp || r.sqrMagnitude < 64 * 64)
                    {
                        isBump++;
                        done = false;
                        break;
                    }
                }

                if (done)
                {
                    break;
                }
            }
            while (isBump > 0 && isBump <= tries);

            if (isBump <= tries)
            {
                galaxy.Stars[s].Position = new Vector2d(r.x/32.0, r.y/32.0);

                uint rc = 0, gc = 0, bc = 0;
                GameHelper.getRGBfromTemperature(ref rc, ref gc, ref bc, (uint)galaxy.Stars[s].TemperatureKelvin);
                galaxy.Stars[s].StarColor = new Color32((byte)rc, (byte)gc, (byte)bc, 255);

                int pX = (tex.width / 2) + (int)r.x;
                int pY = (tex.height / 2) + (int)r.y;
                tex.SetPixels(pX, pY, star.texture.width, star.texture.height, GameHelper.GetPixelAlphaBlendPut(star.texture.GetPixels(), tex.GetPixels(pX, pY, star.texture.width, star.texture.height), galaxy.Stars[s].StarColor));
                GameHelper.DrawCircle(tex, pX + 8, pY + 8, 8, Color.green);
            }
        }
        
        tex.Apply();

        Sprite gFinal = Sprite.Create(tex, new Rect(0, 0, galaxyBG.texture.width, galaxyBG.texture.height), new Vector2(0.5f, 0.5f), 32);
        gFinal.name = "Final_Galaxy";

        Debug.Log("Stars: " + galaxy.Stars.Length);
        overlayRenderer.sprite = gFinal;
    }

   
}
