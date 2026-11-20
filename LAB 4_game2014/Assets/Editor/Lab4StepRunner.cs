using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public static class Lab4StepRunner
{
    private const string Step1DoneKey = "GAME2014_Lab4_Step1_Done";

    [InitializeOnLoadMethod]
    private static void OnLoad()
    {
        EditorApplication.delayCall += () =>
        {
            if (!SessionState.GetBool(Step1DoneKey, false))
            {
                SessionState.SetBool(Step1DoneKey, true);
                RunStep1();
            }
        };
    }

    [MenuItem("Lab 4/Step 1: Configure 2D Scene & Slice Sprites")]
    public static void RunStep1()
    {
        // 1. Set project Editor behavior mode to 2D
        EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode2D;

        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.in2DMode = true;
            SceneView.lastActiveSceneView.Repaint();
        }

        // 2. Configure Main Camera for 2D Orthographic and remove 3D Directional Light
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.orthographic = true;
            mainCam.orthographicSize = 5f;
            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = new Color(0.45f, 0.75f, 0.95f, 1f);
            mainCam.transform.position = new Vector3(0f, 0f, -10f);
            mainCam.transform.rotation = Quaternion.identity;
            EditorUtility.SetDirty(mainCam);
            EditorUtility.SetDirty(mainCam.transform);
        }

        GameObject dirLight = GameObject.Find("Directional Light");
        if (dirLight != null)
        {
            Object.DestroyImmediate(dirLight);
        }

        RenderSettings.skybox = null;

        // 3. Configure single sprites (Foxy animations, props, backgrounds) as 2D Sprites (16 PPU, Point filter)
        string[] singleSpritePaths =
        {
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/idle/sprites/f-01.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/idle/sprites/f-02.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/idle/sprites/f-03.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/idle/sprites/f-04.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-01.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-02.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-03.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-04.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-05.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/run/sprites/f-06.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/jump/sprites/f-01.png",
            "Assets/imported-assets/Sunny-land-files/Assets/Characters/Foxy/jump/sprites/f-02.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Props/crate.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Props/big-crate.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Props/sign.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Props/crank-down.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Props/crank-up.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Background/back.png",
            "Assets/imported-assets/Sunny-land-files/Assets/environment/Background/middle.png"
        };

        foreach (string path in singleSpritePaths)
        {
            ConfigureSingleSprite(path, 16f);
        }

        // 4. Configure and slice tileset.png (400x368 -> 25x23 grid of 16x16 tiles) using ISpriteEditorDataProvider
        SliceTileset("Assets/imported-assets/Sunny-land-files/Assets/environment/tileset.png", 16, 16, 16f);

        // 5. Save scene and assets
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Lab 4] Step 1 complete: 2D Orthographic Scene configured & Sunny Land sprites sliced (16 PPU, Point Filter).");
    }

    private static void ConfigureSingleSprite(string assetPath, float ppu)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) return;

        bool changed = false;
        if (importer.textureType != TextureImporterType.Sprite) { importer.textureType = TextureImporterType.Sprite; changed = true; }
        if (importer.spriteImportMode != SpriteImportMode.Single) { importer.spriteImportMode = SpriteImportMode.Single; changed = true; }
        if (!Mathf.Approximately(importer.spritePixelsPerUnit, ppu)) { importer.spritePixelsPerUnit = ppu; changed = true; }
        if (importer.filterMode != FilterMode.Point) { importer.filterMode = FilterMode.Point; changed = true; }
        if (importer.textureCompression != TextureImporterCompression.Uncompressed) { importer.textureCompression = TextureImporterCompression.Uncompressed; changed = true; }
        if (importer.mipmapEnabled) { importer.mipmapEnabled = false; changed = true; }

        if (changed)
        {
            importer.SaveAndReimport();
        }
    }

    private static void SliceTileset(string assetPath, int cellWidth, int cellHeight, float ppu)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) return;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = ppu;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.isReadable = true;
        importer.SaveAndReimport();

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (tex == null) return;

        SpriteDataProviderFactories factory = new SpriteDataProviderFactories();
        factory.Init();
        ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
        if (dataProvider == null) return;
        dataProvider.InitSpriteEditorDataProvider();

        // Keep existing GUIDs if already sliced so references stay stable
        Dictionary<string, GUID> existingGuids = new Dictionary<string, GUID>();
        SpriteRect[] oldRects = dataProvider.GetSpriteRects();
        if (oldRects != null)
        {
            foreach (SpriteRect sr in oldRects)
            {
                if (!string.IsNullOrEmpty(sr.name) && !existingGuids.ContainsKey(sr.name))
                {
                    existingGuids[sr.name] = sr.spriteID;
                }
            }
        }

        int cols = tex.width / cellWidth;
        int rows = tex.height / cellHeight;
        List<SpriteRect> spriteRects = new List<SpriteRect>();
        List<SpriteNameFileIdPair> nameFileIdPairs = new List<SpriteNameFileIdPair>();

        for (int rowFromTop = 0; rowFromTop < rows; rowFromTop++)
        {
            int y = tex.height - (rowFromTop + 1) * cellHeight;
            for (int col = 0; col < cols; col++)
            {
                int x = col * cellWidth;
                if (!HasOpaquePixel(tex, x, y, cellWidth, cellHeight))
                    continue;

                string spriteName = $"tile_r{rowFromTop}_c{col}";
                GUID guid = existingGuids.TryGetValue(spriteName, out GUID existing) ? existing : GUID.Generate();

                SpriteRect sr = new SpriteRect
                {
                    name = spriteName,
                    spriteID = guid,
                    rect = new Rect(x, y, cellWidth, cellHeight),
                    alignment = SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f),
                    border = Vector4.zero
                };
                spriteRects.Add(sr);
                nameFileIdPairs.Add(new SpriteNameFileIdPair(spriteName, guid));
            }
        }

        dataProvider.SetSpriteRects(spriteRects.ToArray());
        ISpriteNameFileIdDataProvider nameFileIdProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
        if (nameFileIdProvider != null)
        {
            nameFileIdProvider.SetNameFileIdPairs(nameFileIdPairs);
        }

        dataProvider.Apply();
        importer.SaveAndReimport();
    }

    private static bool HasOpaquePixel(Texture2D tex, int startX, int startY, int w, int h)
    {
        Color[] pixels = tex.GetPixels(startX, startY, w, h);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a > 0.01f) return true;
        }
        return false;
    }
}
