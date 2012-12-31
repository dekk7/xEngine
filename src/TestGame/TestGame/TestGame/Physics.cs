using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Box2D.XNA;
using xTile.Tiles;

namespace TestGame
{
    public class Physics : IContactListener
    {
        World world;
        static readonly int PTM = 32;

        public static readonly Object TYPE_GROUND = new Object();
        public static readonly Object TYPE_WATER = new Object();

        public Physics()
        {
            world = new World(new Vector2(0, 50), true);
            world.ContactListener = this;
        }

        public void addCharacterSprite(Sprite sprite)
        {

            CircleShape cs = new CircleShape();
            cs._radius = sprite.Texture2D.Width / PTM;

            FixtureDef fd = new FixtureDef();
            fd.shape = cs;
            fd.restitution = 0.5f;
            fd.friction = 0.5f;
            fd.density = 1.5f;

            BodyDef bd = new BodyDef();
            bd.type = BodyType.Dynamic;
            bd.position = new Vector2(sprite.X / PTM, sprite.Y / PTM);
            bd.userData = sprite;

            Body body = world.CreateBody(bd);
            body.CreateFixture(fd);


        }

        public void addRect(float x, float y, float width, int height, object type)
        {

            PolygonShape ps = new PolygonShape();
            ps.SetAsBox(width / PTM / 2, height / PTM / 2);

            FixtureDef fd = new FixtureDef();
            fd.shape = ps;
            fd.restitution = 0.0f;
            fd.friction = 0.8f;
            fd.density = 1.0f;

            BodyDef bd = new BodyDef();
            bd.type = BodyType.Static;
            bd.position = new Vector2(x / PTM, y / PTM);
            bd.userData = type;

            Body body = world.CreateBody(bd);
            body.CreateFixture(fd);

        }
        public void flickCharacter(float x, float y)
        {
            for (Body b = world.GetBodyList(); b != null; b = b.GetNext())
            {
                if (b.GetUserData() != null)
                {
                    Vector2 p = b.GetPosition();
                    b.ApplyLinearImpulse(new Vector2(x / PTM, y / PTM), b.GetPosition());
                }
            }
        }

        public void update(float dt)
        {

            world.Step(dt / 1000, 10, 3);

            for (Body b = world.GetBodyList(); b != null; b = b.GetNext())
            {

                object userData = b.GetUserData();

                if (userData is Sprite)
                {
                    Sprite sprite = (Sprite)userData;
                    float dx = b.GetPosition().X * PTM;
                    float dy = b.GetPosition().Y * PTM;
                    //sprite.X = dx;
                    //sprite.Y = dy;
                }
            }

        }

        public void BeginContact(Contact contact)
        {

            object obj1 = contact.GetFixtureA().GetBody().GetUserData();
            object obj2 = contact.GetFixtureB().GetBody().GetUserData();

            if (obj1 == TYPE_WATER || obj2 == TYPE_WATER)
            {

                if (obj1 is Sprite)
                {
                    ((Sprite)obj1).Drown = true;
                }
                else if (obj2 is Sprite)
                {
                    ((Sprite)obj2).Drown = true;
                }

            }

        }
        public void EndContact(Contact contact)
        {
        }

        public void PreSolve(Contact contact, ref Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, ref ContactImpulse impulse)
        {
        }
    }
}
