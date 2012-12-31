﻿/*
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

public class EdgeTest : Test
{
	public EdgeTest()
	{
		{
			BodyDef bd = new BodyDef();
			Body ground = _world.CreateBody(bd);

			Vector2 v1 = new Vector2(-10.0f, 0.0f);
            Vector2 v2 = new Vector2(-7.0f, -1.0f); 
            Vector2 v3 = new Vector2(-4.0f, 0.0f);
			Vector2 v4 = new Vector2(0.0f, 0.0f);
            Vector2 v5 = new Vector2(4.0f, 0.0f);
            Vector2 v6 = new Vector2(7.0f, 1.0f);
            Vector2 v7 = new Vector2(10.0f, 0.0f);

            EdgeShape shape = new EdgeShape();

			shape.Set(v1, v2);
			//shape._index1 = 0;
			//shape._index2 = 1;
			shape._hasVertex3 = true;
			shape._vertex3 = v3;
			ground.CreateFixture(shape, 0.0f);

			shape.Set(v2, v3);
			//shape._index1 = 1;
			//shape._index2 = 2;
			shape._hasVertex0 = true;
			shape._hasVertex3 = true;
			shape._vertex0 = v1;
			shape._vertex3 = v4;
			ground.CreateFixture(shape, 0.0f);

			shape.Set(v3, v4);
			//shape._index1 = 2;
			//shape._index2 = 3;
			shape._hasVertex0 = true;
			shape._hasVertex3 = true;
			shape._vertex0 = v2;
			shape._vertex3 = v5;
			ground.CreateFixture(shape, 0.0f);

			shape.Set(v4, v5);
			//shape._index1 = 3;
			//shape._index2 = 4;
			shape._hasVertex0 = true;
			shape._hasVertex3 = true;
			shape._vertex0 = v3;
			shape._vertex3 = v6;
			ground.CreateFixture(shape, 0.0f);

			shape.Set(v5, v6);
			//shape._index1 = 4;
			//shape._index2 = 5;
			shape._hasVertex0 = true;
			shape._hasVertex3 = true;
			shape._vertex0 = v4;
			shape._vertex3 = v7;
			ground.CreateFixture(shape, 0.0f);

			shape.Set(v6, v7);
			//shape._index1 = 5;
			//shape._index2 = 6;
			shape._hasVertex0 = true;
			shape._vertex0 = v5;
			ground.CreateFixture(shape, 0.0f);
		}

		{
			BodyDef bd = new BodyDef();
			bd.type = BodyType.Dynamic;
			bd.position = new Vector2(-0.5f, 0.5f);
			bd.allowSleep = false;
			Body body = _world.CreateBody(bd);

			CircleShape shape = new CircleShape();
			shape._radius = 0.5f;

			body.CreateFixture(shape, 1.0f);
		}

		{
			BodyDef bd = new BodyDef();
			bd.type = BodyType.Dynamic;
			bd.position = new Vector2(0.5f, 0.5f);
			bd.allowSleep = false;
			Body body = _world.CreateBody(bd);

			PolygonShape shape = new PolygonShape();
			shape.SetAsBox(0.5f, 0.5f);

			body.CreateFixture(shape, 1.0f);
		}
	}

	static internal Test Create()
	{
		return new EdgeTest();
	}
};
