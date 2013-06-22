using System;

using BmFont;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class TextLabel : Sprite {
    Texture2D fontAtlas;
    int lineHeight;
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
        lineHeight = fontFile.Common.LineHeight;
        sprites = new List<Sprite>();
        characterMap = new Dictionary<char, FontChar>();

        foreach(var fontCharacter in fontFile.Chars)
        {
            char c = (char) fontCharacter.ID;
            characterMap.Add(c, fontCharacter);
        }
    }

    void centerText(int width, int height)
    {
        foreach (var sprite in sprites)
        {
            var position = sprite.screenPosition;
            sprite.screenPosition = new Vector3(position.X - width / 2, position.Y - height / 2, position.Z);
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

        int dx = 0;
        int dy = 0;

        int maxWidth = 0;

        foreach(char c in text)
        {
            FontChar fc;
            if(characterMap.TryGetValue(c, out fc))
            {
                var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                var position = new Vector2(x + dx + fc.XOffset, y + dy + fc.YOffset);

                var sprite = new Sprite();
                sprite.screenPosition = new Vector3(position, this.screenPosition.Z);
                sprite.cropArea = sourceRectangle;
                sprites.Add(sprite);
                spriteRenderer.add(sprite, fontAtlas, new Point(sourceRectangle.Width, sourceRectangle.Height));

                dx += fc.XAdvance;

                if (dx > maxWidth) {
                    maxWidth = dx;
                }

                if (c == '\n') {
                    dy += lineHeight;
                    dx = 0;
                }
            }
        }

        int maxHeight = dy + lineHeight;

        centerText(maxWidth, maxHeight);

        this.width = maxWidth;
        this.height = maxHeight;
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

