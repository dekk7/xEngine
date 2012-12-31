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

using Box2D.XNA.TestBed.Framework;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using System;

public class CharacterCollision : Test
{
    public CharacterCollision()
	{
        // Ground body
		{
			BodyDef bd = new BodyDef();
			Body ground = _world.CreateBody(bd);

			PolygonShape shape = new PolygonShape();
			shape.SetAsEdge(new Vector2(-20.0f, 0.0f), new Vector2(20.0f, 0.0f));
			ground.CreateFixture(shape, 0.0f);
		}

		// Collinear edges
		{
			BodyDef bd = new BodyDef();
			Body ground = _world.CreateBody(bd);

			PolygonShape shape = new PolygonShape();
			shape.SetAsEdge(new Vector2(-8.0f, 1.0f), new Vector2(-6.0f, 1.0f));
			ground.CreateFixture(shape, 0.0f);
			shape.SetAsEdge(new Vector2(-6.0f, 1.0f), new Vector2(-4.0f, 1.0f));
			ground.CreateFixture(shape, 0.0f);
			shape.SetAsEdge(new Vector2(-4.0f, 1.0f), new Vector2(-2.0f, 1.0f));
			ground.CreateFixture(shape, 0.0f);
		}

		// Square tiles
		{
			BodyDef bd = new BodyDef();
			Body ground = _world.CreateBody(bd);

			PolygonShape shape = new PolygonShape();
			shape.SetAsBox(1.0f, 1.0f, new Vector2(4.0f, 3.0f), 0.0f);
			ground.CreateFixture(shape, 0.0f);
			shape.SetAsBox(1.0f, 1.0f, new Vector2(6.0f, 3.0f), 0.0f);
			ground.CreateFixture(shape, 0.0f);
			shape.SetAsBox(1.0f, 1.0f, new Vector2(8.0f, 3.0f), 0.0f);
			ground.CreateFixture(shape, 0.0f);
		}

		// Square made from edges notice how the edges are shrunk to account
		// for the polygon radius. This makes it so the square character does
		// not get snagged. However, ray casts can now go through the cracks.
        for (int i = 0; i < 4; i++)
		{
			BodyDef bd = new BodyDef();
			Body ground = _world.CreateBody(bd);
            ground.SetTransform(new Vector2(-2f * i, 0), 0);

			Vector2[] vs = new Vector2[4];
            vs[0] = new Vector2(-1.0f, 3.0f);
            vs[1] = new Vector2(1.0f, 3.0f);
            vs[2] = new Vector2(1.0f, 5.0f);
            vs[3] = new Vector2(-1.0f, 5.0f);
            LoopShape shape = new LoopShape();
			shape._count = 4;
			shape._vertices = vs;
			ground.CreateFixture(shape, 0.0f);

			//PolygonShape shape = new PolygonShape();
			//float d = 2.0f * Settings.b2_polygonRadius;
			//shape.SetAsEdge(new Vector2(-1.0f + d, 3.0f), new Vector2(1.0f - d, 3.0f));
			//ground.CreateFixture(shape, 0.0f);
			//shape.SetAsEdge(new Vector2(1.0f, 3.0f + d), new Vector2(1.0f, 5.0f - d));
			//ground.CreateFixture(shape, 0.0f);
			//shape.SetAsEdge(new Vector2(1.0f - d, 5.0f), new Vector2(-1.0f + d, 5.0f));
			//ground.CreateFixture(shape, 0.0f);
			//shape.SetAsEdge(new Vector2(-1.0f, 5.0f - d), new Vector2(-1.0f, 3.0f + d));
			//ground.CreateFixture(shape, 0.0f);
		}

		// Square character
		{
			BodyDef bd = new BodyDef();
			bd.position = new Vector2(-3.0f, 5.0f);
			bd.type = BodyType.Dynamic;
			bd.fixedRotation = true;
			bd.allowSleep = false;

			Body body = _world.CreateBody(bd);

			PolygonShape shape = new PolygonShape();
			shape.SetAsBox(0.5f, 0.5f);

			FixtureDef fd = new FixtureDef();
			fd.shape = shape;
			fd.density = 20.0f;
			body.CreateFixture(fd);
		}

#if false
		// Hexagon character
		{
			BodyDef bd = new BodyDef();
			bd.position = new Vector2(-5.0f, 5.0f);
			bd.type = BodyType.Dynamic;
			bd.fixedRotation = true;
			bd.allowSleep = false;

			Body body = _world.CreateBody(bd);

			float angle = 0.0f;
			float delta = (float)Math.PI / 3.0f;
			Vector2[] vertices = new Vector2[6];
			for (int i = 0; i < 6; ++i)
			{
				vertices[i] = new Vector2(0.5f * (float)Math.Cos(angle), 0.5f * (float)Math.Sin(angle));
				angle += delta;
			}

			PolygonShape shape = new PolygonShape();
			shape.Set(vertices, 6);

			FixtureDef fd = new FixtureDef();
			fd.shape = shape;
			fd.density = 20.0f;
			body.CreateFixture(fd);
		}

		// Circle character
		{
			BodyDef bd = new BodyDef();
			bd.position = new Vector2(3.0f, 5.0f);
			bd.type = BodyType.Dynamic;
			bd.fixedRotation = true;
			bd.allowSleep = false;

			Body body = _world.CreateBody(bd);

			CircleShape shape = new CircleShape();
			shape._radius = 0.5f;

			FixtureDef fd = new FixtureDef();
			fd.shape = shape;
			fd.density = 20.0f;
			body.CreateFixture(fd);
		}
#endif
	}

    public override void  Step(Box2D.XNA.TestBed.Framework.Settings settings)
    {
 	    base.Step(settings);
	    _debugDraw.DrawString(5, _textLine, "This tests various character collision shapes");
	    _textLine += 15;
    }

	static internal Test Create()
	{
        return new CharacterCollision();
	}
};
