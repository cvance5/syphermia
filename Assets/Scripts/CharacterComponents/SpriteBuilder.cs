using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteBuilder : MonoBehaviour {

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Error!  Too many sprite builders in play!");
        }
    }

    public static Sprite[] BuildSlimeSprite()
    {
        //Create the sprite slots necessary
        Sprite[] slimeSprites = new Sprite[7];
        slimeSprites[0] = Sprite.Create(new Texture2D(1024, 1024), new Rect(0, 0, 1024, 1024), new Vector2(.5f, .5f));

        //Pull the main sprite's pixels
        Color[] combinedPixels = slimeSprites[0].texture.GetPixels();

        Color slimeBodyColor = Random.ColorHSV();

        //Make sure the main sprite starts out completely transparent
        for(int i = 0; i < combinedPixels.Length; i++)
        {
            combinedPixels[i].a = 0;
        }
        //For each slime part...
        for (int i = 0; i < instance.resourceFolders.Length; i++)
        {
            //...count the options and pick one...
            int numOptions = Directory.GetFiles(Application.dataPath + "/Resources/" + instance.resourcePath + instance.resourceFolders[i]).Length / 2;
            int selectedOption = Random.Range(0, numOptions);
            Texture2D newTex = Resources.Load(instance.resourcePath + instance.resourceFolders[i] + selectedOption.ToString()) as Texture2D;

            //...and, as long as it's loaded properly...
            if(newTex != null)
            {
                //...create a new sprite using the texture and pull its pixels...
                slimeSprites[i + 1] = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(.5f, .5f));
                Color[] newPixels = slimeSprites[i + 1].texture.GetPixels();

                //...in order to count through them...
                for (int j = 0; j < newPixels.Length; j++)
                {
                    //...recolor them if they are body pictures and not outline or clear...
                    if (i < 4 && newPixels[j].grayscale > .1f)
                    {
                        newPixels[j] = slimeBodyColor;
                    }
                    //...and apply them if they are visible.
                    if (newPixels[j].a > .6f)
                    {
                        combinedPixels[j] = newPixels[j];
                    }
                }
            }
            else
            {
                Debug.LogWarning("Texture has failed to load: " + instance.resourcePath + instance.resourceFolders[i] + selectedOption);
            }
        }

        //Apply the final product and clear unused resources.
        slimeSprites[0].texture.SetPixels(combinedPixels);
        slimeSprites[0].texture.Apply();
        Resources.UnloadUnusedAssets();

        return slimeSprites;
    }

    private static SpriteBuilder instance;

    private readonly string resourcePath = "UnitSprites/";
    private readonly string[] resourceFolders = new string[] {"Bodies/", "Arms/", "Noses/", "Feet/", "Eyebrows/", "Eyes/" };
}
