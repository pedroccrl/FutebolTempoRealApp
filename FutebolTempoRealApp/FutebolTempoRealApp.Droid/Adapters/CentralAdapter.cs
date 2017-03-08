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
using FutebolTempoRealApp.Model.Api;
using Com.Squareup.Picasso;
using FutebolTempoRealApp.Model;
using Android.Gms.Ads;

namespace FutebolTempoRealApp.Droid.Adapters
{
    public class CentralAdapter : BaseAdapter<ItemSection<Jogo>>
    {
        SectionList<Model.Api.Jogo> Jogos;
        Activity Context;
        
        public CentralAdapter(Activity context, SectionList<Jogo> jogos) : base()
        {
            Jogos = jogos;
            Context = context;
        }

        public override ItemSection<Jogo> this[int position]
        {
            get
            {
                return Jogos[position];
            }
        }
        public override int Count
        {
            get
            {
                return Jogos.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Jogos[position];
            var jogo = item.Item;

            View view = convertView;
            
            if (item.IsSection)
            {
                if (view == null || view?.Id != Resource.Layout.HeaderView) view = Context.LayoutInflater.Inflate(Resource.Layout.HeaderView, null);
                view.FindViewById<TextView>(Resource.Id.HeaderTxtHeader).Text = item.Titulo;
            }
            else
            {
                if (view == null || view?.Id != Resource.Layout.JogoView) view = Context.LayoutInflater.Inflate(Resource.Layout.JogoView, null);
                
                view.FindViewById<TextView>(Resource.Id.JogoTxtSiglaCasa).Text = jogo.time_casa.sigla;
                view.FindViewById<TextView>(Resource.Id.JogoTxtPlacar).Text = jogo.Placar;
                view.FindViewById<TextView>(Resource.Id.JogoTxtSiglaVisit).Text = jogo.time_visitante.sigla;
                view.FindViewById<TextView>(Resource.Id.JogoTxtInfo).Text = jogo.Info;
                view.FindViewById<TextView>(Resource.Id.JogoTxtStatus).Text = jogo.Status;
                Picasso.With(Context).Load(jogo.time_casa.escudo).Into(view.FindViewById<ImageView>(Resource.Id.JogoImgEscudoCasa));
                Picasso.With(Context).Load(jogo.time_visitante.escudo).Into(view.FindViewById<ImageView>(Resource.Id.JogoImgEscudoVisit));
            }

            return view;
        }
        
    }
}