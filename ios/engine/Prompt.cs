using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Prompt : MarshalByRefObject {
    SpriteRenderer spriteRenderer;
    ContentManager contentManager;

    TextLabel text;
    Sprite blackBox;

    const int blackBoxHeight = 32;

    public Prompt(SpriteRenderer spriteRenderer, ContentManager contentManager) {
        this.contentManager = contentManager;
        this.spriteRenderer = spriteRenderer;
        registerSprites();
    }

    void buildBlackBox(SpriteRenderer spriteRenderer) {
        var blackTexture = new Texture2D(spriteRenderer.graphics.GraphicsDevice, 1, 1);
        blackTexture.SetData(new byte[] { 0, 0, 0, 255 });

        blackBox = spriteRenderer.add(new Sprite(), blackTexture, new Point(1, 1));
        blackBox.setScreenPosition(0, Camera.main.pixelHeight - blackBoxHeight);
        blackBox.setDepth(-8.75f);
        blackBox.transform.localScale = new Vector3(Camera.main.pixelWidth, blackBoxHeight, 1f);
        blackBox.isVisible = false;
    }

    public void Destroy() {
        spriteRenderer.remove(text);
        spriteRenderer.remove(blackBox);
    }

    public void registerSprites() {
        text = TextLabel.create(spriteRenderer, contentManager, "sierra_agi_font_white");
        
        text.isVisible = false;
        text.setScreenPosition(4, Camera.main.pixelHeight - blackBoxHeight + 8);
        text.setDepth(-9.5f);

        buildBlackBox(spriteRenderer);
    }

    public void setText(string action) {
        text.setText(">" + action + "_");
    }

    public void show() {
        blackBox.isVisible = true;
        text.show();
    }

    public void hide() {
        blackBox.isVisible = false;
        text.hide();
    }
}

