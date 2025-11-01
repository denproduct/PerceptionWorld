using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace PerceptionWorld.Utils;

public static class Resource
{
    private static readonly Dictionary<string, Texture2D> _textureCache = new();
    private static readonly Dictionary<string, SoundEffect> _soundCache = new();

    // Базовый путь внутри Content (без указания расширения!)
    private const string TexturesPath = "Textures";
    private const string SoundsPath = "Sounds";

    /// <summary>
    /// Получить текстуру по имени (без расширения).
    /// Пример: Resource.Texture("stone")
    /// </summary>
    public static Texture2D Texture(string name)
    {
        if (_textureCache.TryGetValue(name, out var texture))
            return texture;

        texture = LoadTexture(name);
        _textureCache[name] = texture;
        return texture;
    }

    /// <summary>
    /// Получить звук по имени (без расширения).
    /// Пример: Resource.Sound("jump")
    /// </summary>
    public static SoundEffect Sound(string name)
    {
        if (_soundCache.TryGetValue(name, out var sound))
            return sound;

        sound = LoadSound(name);
        _soundCache[name] = sound;
        return sound;
    }

    private static Texture2D LoadTexture(string name)
    {
        try
        {
            var texture = State.Content.Load<Texture2D>(
                Path.Combine(TexturesPath, name)
            );
            LogInfo.Write($"Loaded texture: {name}");
            return texture;
        }
        catch (Exception ex)
        {
            LogInfo.Error($"Failed to load texture: {name}", ex);
            throw;
        }
    }

    private static SoundEffect LoadSound(string name)
    {
        try
        {
            var sound = State.Content.Load<SoundEffect>(
                Path.Combine(SoundsPath, name)
            );
            LogInfo.Write($"Loaded sound: {name}");
            return sound;
        }
        catch (Exception ex)
        {
            LogInfo.Error($"Failed to load sound: {name}", ex);
            throw;
        }
    }

    /// <summary>
    /// Выгрузить конкретную текстуру из кэша.
    /// </summary>
    public static void UnloadTexture(string name)
    {
        if (_textureCache.ContainsKey(name))
        {
            _textureCache[name].Dispose();
            _textureCache.Remove(name);
            LogInfo.Write($"Unloaded texture: {name}");
        }
    }

    /// <summary>
    /// Полностью очистить кэш текстур.
    /// </summary>
    public static void ClearTextures()
    {
        foreach (var texture in _textureCache.Values)
            texture.Dispose();
        _textureCache.Clear();
        LogInfo.Write("All textures unloaded");
    }

    /// <summary>
    /// Аналогично для звуков.
    /// </summary>
    public static void UnloadSound(string name) { /* ... */ }
    public static void ClearSounds() { /* ... */ }
}

