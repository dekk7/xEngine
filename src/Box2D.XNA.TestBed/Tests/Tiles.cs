/*
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/


/// This stress tests the dynamic tree broad-phase. This also shows that tile
/// based collision is _not_ smooth due to Box2D not knowing about adjacency.
/// 
using Box2D.XNA.TestBed.Framework;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using System;

public class Tiles : Test
{
    public static int e_count = 20;

	public Tiles()
	{
		{
			float a = 0.5f;
			BodyDef bd = new BodyDef();
			bd.position.Y = -a;
			Body ground = _world.CreateBody(bd);

#if true
			int N = 200;
			int M = 10;
			Vector2 position = new Vector2();
			position.Y = 0.0f;
			for (int j = 0; j < M; ++j)
			{
				position.X = -N * a;
				for (int i = 0; i < N; ++i)
				{
					PolygonShape shape = new PolygonShape();
					shape.SetAsBox(a, a, position, 0.0f);
					ground.CreateFixture(shape, 0.0f);
					position.X += 2.0f * a;
				}
				position.Y -= 2.0f * a;
			}
#else
			int N = 200;
			int M = 10;
			Vector2 position;
			position.x = -N * a;
			for (int i = 0; i < N; ++i)
			{
				position.y = 0.0f;
				for (int j = 0; j < M; ++j)
				{
					PolygonShape shape = new PolygonShape();
					shape.SetAsBox(a, a, position, 0.0f);
					ground.CreateFixture(shape, 0.0f);
					position.y -= 2.0f * a;
				}
				position.x += 2.0f * a;
			}
#endif
		}

		{
			float a = 0.5f;
			PolygonShape shape = new PolygonShape();
			shape.SetAsBox(a, a);

			Vector2 x = new Vector2(-7.0f, 0.75f);
			Vector2 y;
			Vector2 deltaX = new Vector2(0.5625f, 1.25f);
			Vector2 deltaY = new Vector2(1.125f, 0.0f);

			for (int i = 0; i < e_count; ++i)
			{
				y = x;

				for (int j = i; j < e_count; ++j)
				{
					BodyDef bd = new BodyDef();
					bd.type = BodyType.Dynamic;
					bd.position = y;
					Body body = _world.CreateBody(bd);
					body.CreateFixture(shape, 5.0f);

					y += deltaY;
				}

				x += deltaX;
			}
		}
	}

	public override void Step(Box2D.XNA.TestBed.Framework.Settings settings)
	{
		ContactManager cm = _world.GetContactManager();
		int height = cm._broadPhase.ComputeHeight();
		int leafCount = cm._broadPhase.ProxyCount;
		int minimumNodeCount = 2 * leafCount - 1;
		float minimumHeight = (float)(Math.Ceiling(Math.Log((double)minimumNodeCount) / Math.Log(2.0)));
		_debugDraw.DrawString(5, _textLine, string.Format("dynamic tree height = {0}, min = {1}", height, minimumHeight));
		_textLine += 15;

		base.Step(settings);
	}

	static internal Test Create()
	{
		return new Tiles();
	}
};
