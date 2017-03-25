using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    static class ResourceManager
    {
        public static SpriteFont GameFont { get; private set; }

        public static Texture2D StartButton { get; private set; }
        public static Texture2D UpgradeButton { get; private set; }
        public static Texture2D Enemy { get; private set; }
        public static Texture2D TopBanner { get; private set; }
        public static Texture2D BottomBanner { get; private set; }
        public static Texture2D GenericTower { get; private set; }
        public static Texture2D CannonTower { get; private set; }
        public static Texture2D BatteryTower { get; private set; }
        public static Texture2D BlastTower { get; private set; }
        public static Texture2D Wall { get; private set; }
        public static Texture2D Portal { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D CannonBall { get; private set; }
        public static Texture2D LightningBolt { get; private set; }
        public static Texture2D Blast { get; private set; }
        public static Texture2D Grass { get; private set; }
        public static Texture2D DefaultCursor { get; private set; }

        public static SoundEffect BulletSound { get; private set; }
        public static SoundEffect CannonSound { get; private set; }
        public static SoundEffect LightningSound { get; private set; }
        public static SoundEffect BlastSound { get; private set; }
        public static SoundEffect DamagedSound { get; private set; }
        public static SoundEffect SellSound { get; private set; }
        public static SoundEffect WallSound { get; private set; }
        public static SoundEffect PortalSound { get; private set; }

        internal static void InitializeTextures(ContentManager content)
        {
            GameFont = content.Load<SpriteFont>("text");

            StartButton = content.Load<Texture2D>(@"start");
            UpgradeButton = content.Load<Texture2D>(@"upgrade");
            Enemy = content.Load<Texture2D>(@"enemy");
            TopBanner = content.Load<Texture2D>(@"banner");
            BottomBanner = content.Load<Texture2D>(@"banner2");
            GenericTower = content.Load<Texture2D>(@"generic tower");
            CannonTower = content.Load<Texture2D>(@"cannon tower");
            BatteryTower = content.Load<Texture2D>(@"battery tower");
            BlastTower = content.Load<Texture2D>(@"blast tower");
            Wall = content.Load<Texture2D>(@"wall");
            Portal = content.Load<Texture2D>(@"portal");
            Bullet = content.Load<Texture2D>(@"bullet");
            CannonBall = content.Load<Texture2D>(@"cannon ball");
            LightningBolt = content.Load<Texture2D>(@"lightning bolt");
            Blast = content.Load<Texture2D>(@"blast");
            Grass = content.Load<Texture2D>(@"grass");
            DefaultCursor = content.Load<Texture2D>(@"cursor");

            BulletSound = content.Load<SoundEffect>(@"generic attack");
            CannonSound = content.Load<SoundEffect>(@"cannon attack");
            LightningSound = content.Load<SoundEffect>(@"battery attack");
            BlastSound = content.Load<SoundEffect>(@"blast attack");
            DamagedSound = content.Load<SoundEffect>(@"damaged");
            SellSound = content.Load<SoundEffect>(@"sell");
            WallSound = content.Load<SoundEffect>(@"wallsound");
            PortalSound = content.Load<SoundEffect>(@"portalsound");
        }
    }
}
