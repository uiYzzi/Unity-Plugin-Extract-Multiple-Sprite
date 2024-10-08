﻿/*Author: toanstt or nmtoan91
 * Homepage: https://github.com/nmtoan91/Unity-Plugin-Extract-Multiple-Sprite
 * STGAME
 * 2014
 */

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class MultipleSpritesSplit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [MenuItem("Assets/STGame/SpriteTool/Slip This Atlas", false, 0)]
    public static void SlipAtlas()
    {
       
        SlipAtlas(false);
    }
    [MenuItem("Assets/STGame/SpriteTool/Slip This Atlas (Folder,Prefix)", false, 0)]
    public static void SlipAtlasFolderPrefix()
    {
        SlipAtlas(true, true);
    }
    [MenuItem("Assets/STGame/SpriteTool/Slip This Atlas (Folder,Original Name)", false, 0)]
    public static void SlipAtlasFolderOriginalName()
    {
        SlipAtlas(true, false, true);
    }
    [MenuItem("Assets/STGame/SpriteTool/Slip This Atlas (Folder,No Prefix)", false, 0)]
    public static void SlipAtlasFolder()
    {
        SlipAtlas(true);
    }
   
    public static void SlipAtlas(bool is_folder = false, bool is_prefix = false, bool is_OriginalName = false)
    {
        Debug.Log("toan_stt's sprites spliter tool");

        List<Object> selectedObjects = new(Selection.objects);

        foreach (Object file in selectedObjects)
        {
            string rootPath;
            rootPath = AssetDatabase.GetAssetPath(file);
            string fullPath = Path.GetFullPath(AssetDatabase.GetAssetPath(file));
            //Debug.Log(fullPath + "\n" +Selection.activeObject.name);

            string folder = GetMyFolder(fullPath, file.name);
            Debug.Log(folder);

            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath);

            string folder_new = folder;
            if (is_folder)
            {
                folder_new = folder + file.name + "/";
                System.IO.Directory.CreateDirectory(folder_new);
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                string name = i + ".png";
                if (is_prefix || !is_folder) name = file.name + "_" + i + ".png";

                string new_name = folder_new + name;
                if (is_folder) name = folder_new + "/" + i + ".png";

                if (is_OriginalName) new_name = folder_new + "/" + sprites[i].name + ".png";

                Debug.Log("Extracted: " + new_name.Replace(folder, ""));
                Sprite s = (Sprite)sprites[i];

                Texture2D tex2d = ConvertFromSprite(s);
                System.IO.File.WriteAllBytes(new_name, tex2d.EncodeToPNG());
            }
        }

        AssetDatabase.Refresh();
    }
    
    static public string GetMyFolder(string full, string name)
    {
        string s = full.Replace(".png", "");
        s = s.Substring(0, s.Length - name.Length);
        return s;
    }
    static Texture2D ConvertFromSprite(Sprite sprite)
    {
        
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        //Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y,(int)sprite.textureRect.width,(int)sprite.textureRect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
        // Color[] pixels = sprite.texture.GetPixels(0,0, (int)sprite.rect.width, (int)sprite.rect.height);
        //Debug.Log(sprite.textureRect.x + " " + sprite.textureRect.y + " " + sprite.textureRect.width + " "+sprite.textureRect.height+ "  ("+ sprite.textureRectOffset+")");
        //Debug.Log(sprite.rect.x + " " + sprite.rect.y );
        //Debug.Log(pixels.Length);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

}
#endif