using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PerceptionWorld.Utils;
public class Res
{
    public static Texture2D Stone {  get; private set; }
    public static Texture2D Grass { get; private set; }
    public static Texture2D Dirt { get; private set; }
    public static void Load()
    {
        Stone = LoadTexture("Textures/stone");
        Grass = LoadTexture("Textures/grass");
        Dirt = LoadTexture("Textures/dirt");
    }

    private static Texture2D LoadTexture(string path)
    {
        Texture2D texture;
        try
        {
            texture = State.Content.Load<Texture2D>(path);
            LogInfo.Write($"Loaded texture: {texture.Name}");
            return texture;
        }
        catch (Exception ex)
        {
            LogInfo.Error($"Failed to load texture: ", ex);
        }
        return null;
    }
}

