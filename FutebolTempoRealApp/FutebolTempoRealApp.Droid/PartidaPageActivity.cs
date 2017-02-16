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

namespace FutebolTempoRealApp.Droid
{
    [Activity]
    public class PartidaPageActivity : AppCompatActivity
    {
        ViewModel.PartidaViewModel ViewModel;
        Jogo Jogo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PartidaPage);

            var serializado = Intent.GetStringExtra("Partida");
            Jogo = JsonConvert.DeserializeObject<Model.Api.Jogo>(Intent.GetStringExtra("Partida"));
            ViewModel = new ViewModel.PartidaViewModel(Jogo);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var pager = FindViewById<ViewPager>(Resource.Id.PartidaViewPager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.PartidaSlidingTabs);
            var adapter = new PartidaPageAdapter(this, SupportFragmentManager, ViewModel);
            var toolbar = FindViewById<Toolbar>(Resource.Id.PartidaToolbar);

            
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Partida";

            pager.Adapter = adapter;

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
                FindViewById<ContentLoadingProgressBar>(Resource.Id.PartidaPageProgress).Visibility = Android.Views.ViewStates.Gone;

            }
        }
    }
}