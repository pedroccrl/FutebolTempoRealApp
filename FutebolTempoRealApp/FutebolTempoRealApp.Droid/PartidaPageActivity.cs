using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

using FutebolTempoRealApp.Droid.Adapters;
using Newtonsoft.Json;
using FutebolTempoRealApp.Model.Api;
using Android.Support.V4.Widget;
using Android.Views;
using System;
namespace FutebolTempoRealApp.Droid
{
    [Activity]
    public class PartidaPageActivity : AppCompatActivity
    {
        ViewModel.PartidaViewModel ViewModel;
        Jogo Jogo;
        DrawerLayout DrawerLayout;
        FloatingActionButton FloatActBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PartidaMaterialPage);

            var serializado = Intent.GetStringExtra("Partida");
            Jogo = JsonConvert.DeserializeObject<Model.Api.Jogo>(Intent.GetStringExtra("Partida"));
            ViewModel = new ViewModel.PartidaViewModel(Jogo);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var toolbar = FindViewById<Toolbar>(Resource.Id.PartidaMaterialToolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Partida";

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            FloatActBtn = FindViewById<FloatingActionButton>(Resource.Id.PartidaMaterialFloatingActionButton);
            FloatActBtn.Click += (sender, e) => {
                Snackbar.Make(FloatActBtn, "Here's a snackbar!", Snackbar.LengthLong).SetAction("Action",
                    new ClickListener(v => {
                        Console.WriteLine("Action handler");
                    })).Show();
            };

            var pager = FindViewById<ViewPager>(Resource.Id.PartidaMaterialViewPager);
            var adapter = new PartidaPageAdapter(this, SupportFragmentManager, ViewModel);
            pager.Adapter = adapter;

            var tabLayout = FindViewById<TabLayout>(Resource.Id.PartidaMaterialSlidingTabs);
            tabLayout.SetupWithViewPager(pager);

            for (int i = 0; i < 3; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                tab.SetCustomView(adapter.GetTabView(i));
            }
            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Lances") 
            {
                Snackbar.Make(FloatActBtn, "Novos Lances!", Snackbar.LengthLong).SetAction("Action",
                    new ClickListener(v => {
                        Console.WriteLine("Action handler");
                    })).Show();
            }
        }
    }

    public class ClickListener : Java.Lang.Object, View.IOnClickListener
    {
        public ClickListener(Action<View> handler)
        {
            Handler = handler;
        }

        public Action<View> Handler { get; set; }

        public void OnClick(View v)
        {
            var h = Handler;
            if (h != null)
                h(v);
        }
    }
}