using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

public class SpriteRenderer
{
    ContentManager content;
    public GraphicsDeviceManager graphics;

    ISet<Sprite> sprites = new HashSet<Sprite>();

    public SpriteRenderer(GraphicsDeviceManager graphics, ContentManager content)
    {
        this.graphics = graphics;
        this.content = content;
    }

    string[] pathsForClassName (Type type, string[] textureNames)
    {
        var resourcePrefix = type.ToString();
        string[] texturePaths = new string[textureNames.Length];
        for (int i = 0; i < textureNames.Length; i++) {
            texturePaths [i] = resourcePrefix + "/" + textureNames [i];
        }
        return texturePaths;
    }

    Texture2D[] loadFromPaths (string[] texturePaths)
    {
        Texture2D[] textures = new Texture2D[texturePaths.Length];
        for (int i = 0; i < texturePaths.Length; i++) {
            textures [i] = content.Load<Texture2D> (texturePaths [i]);
            Debug.Log("loaded texture " + texturePaths[i]);
        }
        return textures;
    }

    public Sprite add(Sprite sprite, Type type, params string[] textureNames) {
        return add(sprite, pathsForClassName(type, textureNames));
    }

    public Sprite add(Sprite sprite, params string[] texturePaths) {
        return add(sprite, loadFromPaths(texturePaths));
    }

    public Sprite add(Sprite sprite, Texture2D[] textures) {
        sprite.width = textures[0].Width;
        sprite.height = textures[0].Height;

        sprite.textures = textures;
        sprites.Add(sprite);

        return sprite;
    }

    public Sprite add(Sprite sprite, Texture2D texture, Point dimensions) {
        sprite.width = dimensions.X;
        sprite.height = dimensions.Y;

        sprite.textures = new [] {texture};
        sprites.Add(sprite);

        return sprite;
    }

    public void remove(Sprite sprite) {
        sprites.Remove(sprite);
    }

    public void clear() {
        sprites.Clear();
    }

    public void Draw() {
        foreach(var sprite in sprites.OrderBy<Sprite, float>((sprite) => sprite.screenPosition.Z)) {
            Draw(sprite);
        }
    }

    void Draw(Sprite sprite) {
        if (sprite.isVisible) {
            //Debug.Log("Drawing " + sprite.screenPosition + " " + sprite.textures[0].name);
            Quad quad = sprite.createMesh();

            var quadEffect = new AlphaTestEffect(graphics.GraphicsDevice);

            quadEffect.World = Camera.main.world;
            quadEffect.View = Camera.main.view;
            quadEffect.Projection = Camera.main.projection;
            quadEffect.Texture = sprite.textures[sprite.texture_index];


            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        quad.Vertices, 0, 4,
                        quad.Indexes, 0, 2);
            }

        }

    }
}
