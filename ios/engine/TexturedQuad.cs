#region File Description
//-----------------------------------------------------------------------------
// Quad.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public struct Quad
{
	public Vector3 Origin;
	public Vector3 UpperLeft;
	public Vector3 LowerLeft;
	public Vector3 UpperRight;
	public Vector3 LowerRight;
	public Vector3 Normal;
	public Vector3 Up;
	public Vector3 Right;

	public VertexPositionNormalTexture[] Vertices;
	public short[] Indexes;
			
	public Quad( Vector3 origin, Vector3 normal, Vector3 up, Vector3 right,
	            float width, float height )
	{
		Vertices = new VertexPositionNormalTexture[4];
		Indexes = new short[6];
		Origin = origin;
		Normal = normal;
		Up = up;
        Right = right;
		
		// Calculate the quad corners
		UpperLeft = Origin;
		UpperRight = Origin + Right * width;
		LowerLeft = UpperLeft - (Up * height);
		LowerRight = UpperRight - (Up * height);

		FillVertices();
	}
	
	private void FillVertices()
	{
		// Fill in texture coordinates to display full texture
		// on quad
		Vector2 textureUpperLeft = new Vector2( 0.0f, 0.0f );
		Vector2 textureUpperRight = new Vector2( 1.0f, 0.0f );
		Vector2 textureLowerLeft = new Vector2( 0.0f, 1.0f );
		Vector2 textureLowerRight = new Vector2( 1.0f, 1.0f );
		
		// Provide a normal for each vertex
		for (int i = 0; i < Vertices.Length; i++)
		{
			Vertices[i].Normal = Normal;
		}
		
		// Set the position and texture coordinate for each
		// vertex
		Vertices[0].Position = LowerLeft;
		Vertices[0].TextureCoordinate = textureLowerLeft;
		Vertices[1].Position = UpperLeft;
		Vertices[1].TextureCoordinate = textureUpperLeft;
		Vertices[2].Position = LowerRight;
		Vertices[2].TextureCoordinate = textureLowerRight;
		Vertices[3].Position = UpperRight;
		Vertices[3].TextureCoordinate = textureUpperRight;
		
		// Set the index buffer for each vertex, using
		// clockwise winding
		Indexes[0] = 0;
		Indexes[1] = 1;
		Indexes[2] = 2;
		Indexes[3] = 2;
		Indexes[4] = 1;
		Indexes[5] = 3;
	}

    // sets UV coordinates to only display the rectangular portion of the texture on the mesh
    public void cropTextureToRectangle(Rectangle cell, int textureWidth, int textureHeight) {
        float left = cell.X / (float) textureWidth;
        float top = cell.Y / (float) textureHeight;
        float right = left + cell.Width / (float) textureWidth;
        float bottom = top + cell.Height / (float) textureHeight;

        Vertices[0].TextureCoordinate = new Vector2(left, bottom);
        Vertices[1].TextureCoordinate = new Vector2(left, top);
        Vertices[2].TextureCoordinate = new Vector2(right, bottom);
        Vertices[3].TextureCoordinate = new Vector2(right, top);
    }
}

