using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

internal static class PrefabSetupTool
{
    private const string ArtFolder = "Assets/WordWall/Art/Environment";
    private const string CharacterPrefabFolder = "Assets/WordWall/Prefabs/Characters";
    private const string EnvironmentPrefabFolder = "Assets/WordWall/Prefabs/Environment";
    private const string GameplayPrefabFolder = "Assets/WordWall/Prefabs/Gameplay";
    private const string PlaceholderSpritePath = ArtFolder + "/PlaceholderSquare.asset";

    [MenuItem("Tools/WordWall/Create Placeholder Prefabs")]
    private static void CreatePlaceholderPrefabs()
    {
        Sprite placeholderSprite = GetOrCreatePlaceholderSprite();
        int createdCount = 0;
        int skippedCount = 0;

        CreatePrefabIfMissing(
            CharacterPrefabFolder + "/Player.prefab",
            () => CreatePlayer(placeholderSprite),
            ref createdCount,
            ref skippedCount);

        CreatePrefabIfMissing(
            CharacterPrefabFolder + "/Cat.prefab",
            () => CreateCat(placeholderSprite),
            ref createdCount,
            ref skippedCount);

        CreatePrefabIfMissing(
            GameplayPrefabFolder + "/LetterBlock.prefab",
            () => CreateLetterBlock(placeholderSprite),
            ref createdCount,
            ref skippedCount);

        CreatePrefabIfMissing(
            EnvironmentPrefabFolder + "/Exit.prefab",
            () => CreateExit(placeholderSprite),
            ref createdCount,
            ref skippedCount);

        int configuredBuilderCount = AssignPrefabsToMazeBuilders();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "WordWall Prefab Setup",
            $"Created {createdCount} prefab(s). Skipped {skippedCount} existing prefab(s). "
                + $"Configured {configuredBuilderCount} MazeBuilder(s). Save the scene.",
            "OK");
    }

    private static Sprite GetOrCreatePlaceholderSprite()
    {
        UnityEngine.Object[] existingAssets = AssetDatabase.LoadAllAssetsAtPath(PlaceholderSpritePath);
        foreach (UnityEngine.Object asset in existingAssets)
        {
            if (asset is Sprite existingSprite)
            {
                return existingSprite;
            }
        }

        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
        {
            name = "PlaceholderSquareTexture",
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
        };
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, 1f, 1f),
            new Vector2(0.5f, 0.5f),
            1f);
        sprite.name = "PlaceholderSquare";

        AssetDatabase.CreateAsset(texture, PlaceholderSpritePath);
        AssetDatabase.AddObjectToAsset(sprite, texture);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(PlaceholderSpritePath);

        foreach (UnityEngine.Object asset in AssetDatabase.LoadAllAssetsAtPath(PlaceholderSpritePath))
        {
            if (asset is Sprite importedSprite)
            {
                return importedSprite;
            }
        }

        throw new InvalidOperationException("Unity could not create the placeholder sprite asset.");
    }

    private static void CreatePrefabIfMissing(
        string prefabPath,
        Func<GameObject> createObject,
        ref int createdCount,
        ref int skippedCount)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
        {
            skippedCount++;
            return;
        }

        GameObject instance = createObject();
        try
        {
            PrefabUtility.SaveAsPrefabAsset(instance, prefabPath, out bool success);
            if (!success)
            {
                throw new InvalidOperationException($"Unity could not save prefab at {prefabPath}.");
            }

            createdCount++;
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(instance);
        }
    }

    private static GameObject CreatePlayer(Sprite sprite)
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = Color.gray;
        renderer.sortingOrder = 1;

        Rigidbody2D rigidbody = player.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
        collider.radius = 0.4f;

        player.AddComponent<PlayerController>();
        return player;
    }

    private static GameObject CreateCat(Sprite sprite)
    {
        GameObject cat = new GameObject("Cat");
        cat.tag = "Cat";

        SpriteRenderer renderer = cat.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = new Color(0.8f, 0.3f, 0.1f);
        renderer.sortingOrder = 1;

        CircleCollider2D collider = cat.AddComponent<CircleCollider2D>();
        collider.radius = 0.4f;
        collider.isTrigger = true;

        cat.AddComponent<CatChaser>();
        return cat;
    }

    private static GameObject CreateLetterBlock(Sprite sprite)
    {
        GameObject block = new GameObject("LetterBlock");

        SpriteRenderer renderer = block.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = new Color(0.9f, 0.8f, 0.2f);

        Rigidbody2D rigidbody = block.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;

        block.AddComponent<BoxCollider2D>();

        LetterBlock letterBlock = block.AddComponent<LetterBlock>();
        letterBlock.letter = 'C';

        GameObject label = new GameObject("Label");
        label.transform.SetParent(block.transform, false);
        label.transform.localPosition = new Vector3(0f, 0f, -1f);

        TextMesh text = label.AddComponent<TextMesh>();
        text.text = "C";
        text.color = Color.black;
        text.characterSize = 0.15f;
        text.fontSize = 40;
        text.anchor = TextAnchor.MiddleCenter;

        return block;
    }

    private static GameObject CreateExit(Sprite sprite)
    {
        GameObject exit = new GameObject("Exit");
        exit.tag = "Exit";

        SpriteRenderer renderer = exit.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = new Color(0.7f, 0.1f, 0.1f);

        exit.AddComponent<BoxCollider2D>();
        return exit;
    }

    private static int AssignPrefabsToMazeBuilders()
    {
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            CharacterPrefabFolder + "/Player.prefab");
        GameObject catPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            CharacterPrefabFolder + "/Cat.prefab");
        GameObject letterBlockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            GameplayPrefabFolder + "/LetterBlock.prefab");
        GameObject exitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            EnvironmentPrefabFolder + "/Exit.prefab");

        MazeBuilder[] builders = UnityEngine.Object.FindObjectsByType<MazeBuilder>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (MazeBuilder builder in builders)
        {
            Undo.RecordObject(builder, "Assign WordWall Prefabs");

            SerializedObject serializedBuilder = new SerializedObject(builder);
            serializedBuilder.FindProperty("playerPrefab").objectReferenceValue = playerPrefab;
            serializedBuilder.FindProperty("catPrefab").objectReferenceValue = catPrefab;
            serializedBuilder.FindProperty("letterBlockPrefab").objectReferenceValue = letterBlockPrefab;
            serializedBuilder.FindProperty("exitPrefab").objectReferenceValue = exitPrefab;
            serializedBuilder.ApplyModifiedProperties();

            EditorUtility.SetDirty(builder);
            EditorSceneManager.MarkSceneDirty(builder.gameObject.scene);
        }

        return builders.Length;
    }
}
