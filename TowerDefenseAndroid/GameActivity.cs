using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace TowerDefenseAndroid
{
    [Activity(Label = "TowerDefenseAndroid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.FullUser
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class GameActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new TowerDefense.TowerDefense();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

