using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineLibrary;
using OpenTK;

namespace GameLibrary
{
    public class BattleScene : IScene
    {
        private List<Texture2D> textures;
        private List<GameObject> objects;

        public void Init()
        {
            textures = new List<Texture2D>();
            objects = new List<GameObject>();

            Texture2D firstTrackTex = Texture2D.LoadTexture(@"../../Resources/Track.bmp");
            Texture2D firstTurretTex = Texture2D.LoadTexture(@"../../Resources/Turret.bmp");
            Texture2D secondTrackTex = Texture2D.LoadTexture(@"../../Resources/Track.bmp");
            Texture2D secondTurretTex = Texture2D.LoadTexture(@"../../Resources/Turret.bmp");
            textures.Add(firstTrackTex);
            textures.Add(firstTurretTex);
            textures.Add(secondTrackTex);
            textures.Add(secondTurretTex);

            firstTrackTex.Color = Color.Green;
            firstTurretTex.Color = Color.Green;
            secondTrackTex.Color = Color.Red;
            secondTurretTex.Color = Color.Red;

            GameObject firstPanzerkampf = new GameObject(firstTrackTex);
            GameObject firstPanzerkampfTurret = new GameObject(firstTurretTex);
            firstPanzerkampf.AddInnerObject(firstPanzerkampfTurret);
            objects.Add(firstPanzerkampf);
            objects.Add(firstPanzerkampfTurret);

            GameObject secondPanzerkampf = new GameObject(secondTrackTex);
            GameObject secondPanzerkampfTurret = new GameObject(secondTurretTex);
            secondPanzerkampf.AddInnerObject(secondPanzerkampfTurret);
            objects.Add(secondPanzerkampf);
            objects.Add(secondPanzerkampfTurret);

            firstPanzerkampf.Scale = new Vector2(5, 5);
            firstPanzerkampfTurret.Scale = new Vector2(5, 5);
            secondPanzerkampf.Scale = new Vector2(-5, 5);
            secondPanzerkampfTurret.Scale = new Vector2(-5, 5);

            firstPanzerkampfTurret.Position = new Vector2(25, -20);
            firstPanzerkampfTurret.RotationPoint = new Vector2(16, 4);
            secondPanzerkampfTurret.Position = new Vector2(-25, -20);
            secondPanzerkampfTurret.RotationPoint = new Vector2(16, 4);

            //firstPanzerkampfTurret.AddScript(new RotationScript());
            //secondPanzerkampfTurret.AddScript(new RotationScript());
        }

        public void Update(TimeSpan delta)
        {
            foreach (GameObject gameObject in objects)
            {
                foreach (IScript script in gameObject.Scripts)
                {
                    script.Update();
                }
            }
        }

        public List<GameObject> GetGameObjects()
        {
            return objects;
        }

        public void Dispose()
        {
            foreach (Texture2D texture in textures)
            {
                texture.Dispose();
            }

            foreach (GameObject gameObject in objects)
            {
                gameObject.Dispose();
            }
        }
    }
}
