using System;

using BmFont;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class TextLabel : Sprite {
    Texture2D fontAtlas;
    public String text;

    SpriteRenderer spriteRenderer;

    Dictionary<char, FontChar> characterMap;
    public List<Sprite> sprites { get; private set; }

    public static TextLabel create(SpriteRenderer spriteRenderer, ContentManager contentManager, String fontName) {
        FontFile fontFile;
        Texture2D fontTexture;

        var fontFilePath = Path.Combine(contentManager.RootDirectory, fontName + ".fnt");
        using(var stream = TitleContainer.OpenStream(fontFilePath))
        {
            fontFile = FontLoader.Load(stream);
            fontTexture = contentManager.Load<Texture2D>(fontName + "_0.png");

            stream.Close();
        }
        return new TextLabel(spriteRenderer, fontFile, fontTexture);
    }

    protected TextLabel(SpriteRenderer spriteRenderer, FontFile fontFile, Texture2D texture) : base() {
        this.spriteRenderer = spriteRenderer;
        this.fontAtlas = texture;
        sprites = new List<Sprite>();
        characterMap = new Dictionary<char, FontChar>();

        foreach(var fontCharacter in fontFile.Chars)
        {
            char c = (char) fontCharacter.ID;
            characterMap.Add(c, fontCharacter);
        }
    }

    public void setText(string text)
    {
        foreach(var sprite in sprites) {
            spriteRenderer.remove(sprite);
        }
        sprites.Clear();

        this.text = text;

        int x = (int) this.screenPosition.X;
        int y = (int) this.screenPosition.Y;

        int dx = x;
        int dy = y;

        foreach(char c in text)
        {
            FontChar fc;
            if(characterMap.TryGetValue(c, out fc))
            {
                var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                var sprite = new Sprite();
                sprite.screenPosition = new Vector3(position, this.screenPosition.Z);
                sprite.cropArea = sourceRectangle;
                sprites.Add(sprite);
                spriteRenderer.add(sprite, fontAtlas, sourceRectangle.Location);

                dx += fc.XAdvance;
            }
        }
    }

    public void show() {
        foreach (var sprite in sprites)
            sprite.isVisible = true;
    }

    public void hide() {
        foreach (var sprite in sprites)
            sprite.isVisible = false;
    }

}

