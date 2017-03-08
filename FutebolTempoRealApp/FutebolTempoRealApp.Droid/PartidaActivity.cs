using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Com.Squareup.Picasso;
using FutebolTempoRealApp.Droid.Adapters;

namespace FutebolTempoRealApp.Droid
{
    [Obsolete("Usar PartidaPageActivity com SlidingTabs")]
    [Activity(Label = "PartidaActivity")]
    public class PartidaActivity : Activity
    {
        Model.Api.Jogo Jogo;
        ViewModel.PartidaViewModel ViewModel;
        ListView ListViewLances;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PartidaLances);

            var serializado = Intent.GetStringExtra("Partida");
            Jogo = JsonConvert.DeserializeObject<Model.Api.Jogo>(Intent.GetStringExtra("Partida"));

            ListViewLances = this.FindViewById<ListView>(Resource.Id.PartidaRecyclerViewLances);

            ViewModel = new ViewModel.PartidaViewModel(Jogo);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;



            FindViewById<TextView>(Resource.Id.PartidaTxtTimeCasa).Text = Jogo.time_casa.nome;
            
            FindViewById<TextView>(Resource.Id.PartidaTxtPlacar).Text = Jogo.Placar;
            FindViewById<TextView>(Resource.Id.PartidaTxtTimeVisit).Text = Jogo.time_visitante.nome;

            //Helpers.ShowImageFromUrl(FindViewById<ImageView>(Resource.Id.PartidaImgEscudoVisit), Jogo.time_visitante.escudo);
            //Helpers.ShowImageFromUrl(FindViewById<ImageView>(Resource.Id.PartidaImgEscudoCasa), Jogo.time_casa.escudo);

            Picasso.With(this).Load(Jogo.time_casa.escudo).Into(FindViewById<ImageView>(Resource.Id.PartidaImgEscudoCasa));
            Picasso.With(this).Load(Jogo.time_visitante.escudo).Into(FindViewById<ImageView>(Resource.Id.PartidaImgEscudoVisit));

            FindViewById<TextView>(Resource.Id.PartidaTxtPlacarPenalti).Visibility = ViewStates.Gone;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Lances")
            {
                //ListViewLances.Adapter = new LanceAdapter(this, ViewModel.Lances);
            }
        }
    }
}