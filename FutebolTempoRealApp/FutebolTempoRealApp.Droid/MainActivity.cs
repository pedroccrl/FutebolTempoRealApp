using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FutebolTempoRealApp.Droid.Adapters;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Android.Gms.Ads;

namespace FutebolTempoRealApp.Droid
{
	[Activity (Label = "FutebolTempoRealApp.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        ViewModel.CentralViewModel ViewModel;
        ListView ListViewCentral;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Model.App.SaveAndLoad = new SaveAndLoad(this);

            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Jogos de Hoje";

            var adView = FindViewById<AdView>(Resource.Id.adView1);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            ViewModel = new ViewModel.CentralViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ListViewCentral = FindViewById<ListView>(Resource.Id.ListViewCentral);
            ListViewCentral.ItemClick += ListViewCentral_ItemClick;
		}

        private void ListViewCentral_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = ViewModel.SectionJogos[e.Position];

            if (item.IsSection) return;

            var partida = item.Item;

            var partidaIntent = new Intent(this, typeof(PartidaPageActivity));
            partidaIntent.PutExtra("Partida", JsonConvert.SerializeObject(partida));

            StartActivity(partidaIntent);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Jogos")
            {
                ListViewCentral.Adapter = new CentralAdapter(this,  ViewModel.SectionJogos);
                FindViewById<ProgressBar>(Resource.Id.CentralProgress).Visibility = ViewStates.Gone;
            }
        }
    }
    
}


