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
using FutebolTempoRealApp.Model;
using Com.Squareup.Picasso;

namespace FutebolTempoRealApp.Droid.Adapters
{
    class LanceAdapter : BaseAdapter<ItemSection<Lance>>
    {
        SectionList<Lance> Lances;
        Activity Context;
        TextView Titulo, Subtitulo, Jogador, Posicao, Texto, Tempo;
        ImageView Foto, JogadorFoto, JogadorClube;

        public LanceAdapter(Activity context, SectionList<Lance> lances) : base()
        {
            Context = context;
            Lances = lances;
        }

        public override ItemSection<Lance> this[int position]
        {
            get
            {
                return Lances[position];
            }
        }
        public override int Count
        {
            get
            {
                return Lances.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Lances[position];
            var lance = item.Item;

            View view = convertView;

            if (item.IsSection)
            {
                if (view == null || view?.Id != Resource.Layout.HeaderView) view = Context.LayoutInflater.Inflate(Resource.Layout.HeaderView, null);
                view.FindViewById<TextView>(Resource.Id.HeaderTxtHeader).Text = item.Titulo;
            }
            else
            {
                if (view == null || view?.Id != Resource.Layout.LanceView) view = Context.LayoutInflater.Inflate(Resource.Layout.LanceView, null);
                
                Titulo = view.FindViewById<TextView>(Resource.Id.LanceTxtTitulo);
                Subtitulo = view.FindViewById<TextView>(Resource.Id.LanceTxtSubtitulo);
                Jogador = view.FindViewById<TextView>(Resource.Id.LanceTxtNomeJogador);
                Posicao = view.FindViewById<TextView>(Resource.Id.LanceTxtPosicaoJogador);
                Texto = view.FindViewById<TextView>(Resource.Id.LanceTxtTexto);
                Tempo = view.FindViewById<TextView>(Resource.Id.LanceTxtTempo);
                Foto = view.FindViewById<ImageView>(Resource.Id.LanceImgFoto);
                JogadorFoto = view.FindViewById<ImageView>(Resource.Id.LanceImgJogador);
                JogadorClube = view.FindViewById<ImageView>(Resource.Id.LanceImgClube);

                Texto.Text = lance.texto;
                Tempo.Text = lance.Tempo;
                Titulo.Text = lance.titulo;
                Jogador.Text = lance.Jogador.Nome;
                Posicao.Text = lance.Jogador.PosicaoDescricao;
                Picasso.With(Context).Load(lance.Jogador.FotoUrl).Into(JogadorFoto);
                Picasso.With(Context).Load(lance.Jogador.EscudoClubeUrl).Into(JogadorClube);

                if (!string.IsNullOrWhiteSpace(lance.foto_url)) Picasso.With(Context).Load(lance.foto_url).Into(Foto);
                else Foto.Visibility = ViewStates.Gone;
                if (string.IsNullOrWhiteSpace(lance.Jogador.EscudoClubeUrl)) JogadorClube.Visibility = ViewStates.Gone;
                if (string.IsNullOrWhiteSpace(lance.Jogador.FotoUrl)) JogadorFoto.Visibility = ViewStates.Gone;



                VerificaTextViews();
            }

            

            return view;
        }

        void VerificaTextViews()
        {
            if (string.IsNullOrWhiteSpace(Titulo.Text)) Titulo.Visibility = ViewStates.Gone;
            if (string.IsNullOrWhiteSpace(Subtitulo.Text)) Subtitulo.Visibility = ViewStates.Gone;
            if (string.IsNullOrWhiteSpace(Jogador.Text)) Jogador.Visibility = ViewStates.Gone;
            if (string.IsNullOrWhiteSpace(Posicao.Text)) Posicao.Visibility = ViewStates.Gone;
            if (string.IsNullOrWhiteSpace(Texto.Text)) Texto.Visibility = ViewStates.Gone;
            if (string.IsNullOrWhiteSpace(Tempo.Text)) Tempo.Visibility = ViewStates.Gone;
        }
    }
}