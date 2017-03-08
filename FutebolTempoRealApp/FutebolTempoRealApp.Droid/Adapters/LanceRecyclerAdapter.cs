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
using Android.Support.V7.Widget;
using FutebolTempoRealApp.Model;
using FutebolTempoRealApp.Model.Api;
using Com.Squareup.Picasso;
using FutebolTempoRealApp.ViewModel;
using Java.Lang;
using Android.Webkit;



namespace FutebolTempoRealApp.Droid.Adapters
{
    public class LanceRecyclerAdapter : RecyclerView.Adapter
    {
        SectionList<Lance> Lances;
        PartidaViewModel ViewModel;
        Activity Parent;
        
        public LanceRecyclerAdapter(Activity context, PartidaViewModel viewModel)
        {
            Parent = context;
            ViewModel = viewModel;
            Lances = ViewModel.Lances;
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Lances")
                {
                    NotifyDataSetChanged();
                }
            };
        }

        public ItemSection<Lance> GetValueAt(int position)
        {
            if (Lances == null) return new ItemSection<Lance>(string.Empty);
            return Lances[position];
        }

        public override int ItemCount
        {
            get
            {
                if (Lances == null) return 0;
                return Lances.Count;
            }
        }

        public override int GetItemViewType(int position)
        {
            if (Lances[position].IsSection) return (int)LanceType.Titulo;
            else return (int)Lances[position].Item.Tipo;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (Lances[position].IsSection)
            {
                var headerView = holder as HeaderViewHolder;
                headerView.BindHeader(Lances[position].Titulo);
            }
            else
            {
                var lanceView = holder as ILanceViewHolder;
                lanceView.BindLance(Lances[position].Item);
            }
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var tipo = (LanceType)viewType;
            switch (tipo)
            {
                case LanceType.Twitter:
                    var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceTwitterView, parent, false);
                    return new LanceTwitterViewHolder(v);
                case LanceType.Titulo:
                    var hv = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HeaderView, parent, false);
                    return new HeaderViewHolder(hv);
                case LanceType.Gol:
                    var lg = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceGolView, parent, false);
                    return new LanceGolViewHolder(lg);
                case LanceType.Cartao:
                    var lc = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceCartaoView, parent, false);
                    return new LanceCartaoViewHolder(lc);
                case LanceType.Simples:
                    var ls = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceSimplesView, parent, false);
                    return new LanceSimplesViewHolder(ls);
                case LanceType.SimplesSemTitulo:
                    var lsst = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceSimplesSemTituloView, parent, false);
                    return new LanceSimplesSemTituloViewHolder(lsst);
                case LanceType.Substituicao:
                    var ls2 = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceSubstituicaoView, parent, false);
                    return new LanceSubstituicaoViewHolder(ls2);
                case LanceType.TwitterEmbed:
                    var lte = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceTwitterEmbedView, parent, false);
                    return new LanceTwitterEmbedViewHolder(lte);
                default:
                    var vw = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LanceView, parent, false);
                    return new LanceViewHolder(vw);
            }
        }
        
    }

    public class LanceViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        public TextView Titulo, Subtitulo, Jogador, Posicao, Texto, Tempo;
        public ImageView Foto, JogadorFoto, JogadorClube;
        Context context;
        public LanceViewHolder(View view) : base(view)
        {
            Titulo = view.FindViewById<TextView>(Resource.Id.LanceTxtTitulo);
            Subtitulo = view.FindViewById<TextView>(Resource.Id.LanceTxtSubtitulo);
            Jogador = view.FindViewById<TextView>(Resource.Id.LanceTxtNomeJogador);
            Posicao = view.FindViewById<TextView>(Resource.Id.LanceTxtPosicaoJogador);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceTxtTexto);
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceTxtTempo);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceImgFoto);
            JogadorFoto = view.FindViewById<ImageView>(Resource.Id.LanceImgJogador);
            JogadorClube = view.FindViewById<ImageView>(Resource.Id.LanceImgClube);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Texto.Text = lance.texto;
            Tempo.Text = lance.Tempo;
            Titulo.Text = lance.titulo;
            Jogador.Text = lance.Jogador.Nome;
            Posicao.Text = lance.Jogador.PosicaoDescricao;

            JogadorFoto.SetImageDrawable(null);
            JogadorClube.SetImageDrawable(null);
            Foto.SetImageDrawable(null);

            Picasso.With(context).Load(lance.Jogador.FotoUrl).Into(JogadorFoto);
            Picasso.With(context).Load(lance.Jogador.EscudoClubeUrl).Into(JogadorClube);

            if (!string.IsNullOrWhiteSpace(lance.FotoUrl)) Picasso.With(context).Load(lance.FotoUrl).Into(Foto);
            //else Foto.Visibility = ViewStates.Gone;
            //if (string.IsNullOrWhiteSpace(lance.Jogador.EscudoClubeUrl)) JogadorClube.Visibility = ViewStates.Gone;
            //if (string.IsNullOrWhiteSpace(lance.Jogador.FotoUrl)) JogadorFoto.Visibility = ViewStates.Gone;

            //VerificaTextViews();
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

    public class HeaderViewHolder : RecyclerView.ViewHolder
    {
        TextView HeaderTextView;
        public HeaderViewHolder(View view) : base(view)
        {
            HeaderTextView = view.FindViewById<TextView>(Resource.Id.HeaderTxtHeader);
        }

        public void BindHeader(string titulo)
        {
            HeaderTextView.Text = titulo;
        }
    }

    public class LanceTwitterViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        ImageView Autor, Foto;
        TextView NomeAutor, TwitterAutor, Texto;
        Context context;
        public LanceTwitterViewHolder(View view) : base(view)
        {
            Autor = view.FindViewById<ImageView>(Resource.Id.LanceTwitterImgAutor);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceTwitterImgFoto);
            NomeAutor = view.FindViewById<TextView>(Resource.Id.LanceTwitterTxtAutorNome);
            TwitterAutor = view.FindViewById<TextView>(Resource.Id.LanceTwitterTxtAutorTwitter);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceTwitterTxtTexto);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            var twitter = lance.LanceTwitter.Lance;
            NomeAutor.Text = twitter.user.name;
            TwitterAutor.Text = twitter.user.Perfil;
            Texto.Text = twitter.text;
            Picasso.With(context).Load(twitter.user.profile_image_url).Into(Autor);
            Picasso.With(context).Load(twitter.FotoUrl).Into(Foto);
        }
    }

    public class LanceGolViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo, NomeJogador, PosicaoJogador, Titulo, Texto;
        ImageView ImgJogador, ImgEscudo, Foto;
        Context context;
        public LanceGolViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceGolTxtTempo);
            NomeJogador = view.FindViewById<TextView>(Resource.Id.LanceGolTxtNomeJogador);
            PosicaoJogador = view.FindViewById<TextView>(Resource.Id.LanceGolTxtPosicaoJogador);
            Titulo = view.FindViewById<TextView>(Resource.Id.LanceGolTxtTitulo);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceGolTxtTexto);
            ImgJogador = view.FindViewById<ImageView>(Resource.Id.LanceGolImgJogador);
            ImgEscudo = view.FindViewById<ImageView>(Resource.Id.LanceGolImgClube);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceGolImgFoto);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            NomeJogador.Text = lance.Jogador.Nome;
            PosicaoJogador.Text = lance.Jogador.PosicaoDescricao;
            Titulo.Text = lance.titulo;
            Texto.Text = lance.texto;
            Picasso.With(context).Load(lance.Jogador.FotoUrl).Into(ImgJogador);
            Picasso.With(context).Load(lance.Jogador.EscudoClubeUrl).Into(ImgEscudo);
            Picasso.With(context).Load(lance.FotoUrl).Into(Foto);
        }
    }

    public class LanceCartaoViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo, NomeJogador, PosicaoJogador, Texto;
        ImageView ImgJogador, ImgEscudo, Foto;
        Context context;
        public LanceCartaoViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceCartaoTxtTempo);
            NomeJogador = view.FindViewById<TextView>(Resource.Id.LanceCartaoTxtNomeJogador);
            PosicaoJogador = view.FindViewById<TextView>(Resource.Id.LanceCartaoTxtPosicaoJogador);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceCartaoTxtTexto);
            ImgJogador = view.FindViewById<ImageView>(Resource.Id.LanceCartaoImgJogador);
            ImgEscudo = view.FindViewById<ImageView>(Resource.Id.LanceCartaoImgClube);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceCartaoImgFoto);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            NomeJogador.Text = lance.Jogador.Nome;
            PosicaoJogador.Text = lance.Jogador.PosicaoDescricao;
            Texto.Text = lance.texto;
            Picasso.With(context).Load(lance.Jogador.FotoUrl).Into(ImgJogador);
            Picasso.With(context).Load(lance.Jogador.EscudoClubeUrl).Into(ImgEscudo);
            Picasso.With(context).Load(lance.FotoUrl).Into(Foto);
        }
    }

    public class LanceSimplesViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo, Titulo, Texto;
        ImageView Foto;
        Context context;
        public LanceSimplesViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceSimplesTxtTempo);
            Titulo = view.FindViewById<TextView>(Resource.Id.LanceSimplesTxtTitulo);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceSimplesTxtTexto);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceSimplesImgFoto);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            Titulo.Text = lance.titulo;
            Texto.Text = lance.texto;
            Picasso.With(context).Load(lance.FotoUrl).Into(Foto);
        }
    }

    public class LanceSimplesSemTituloViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo, Texto;
        ImageView Foto;
        Context context;
        public LanceSimplesSemTituloViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceSimplesSemTituloTxtTempo);
            Texto = view.FindViewById<TextView>(Resource.Id.LanceSimplesSemTituloTxtTexto);
            Foto = view.FindViewById<ImageView>(Resource.Id.LanceSimplesSemTituloImgFoto);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            Texto.Text = lance.texto;
            Picasso.With(context).Load(lance.FotoUrl).Into(Foto);
        }
    }

    public class LanceTwitterEmbedViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo;
        WebView Web;
        Context context;
        public LanceTwitterEmbedViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceTwitterEmbedTxtTempo);
            Web = view.FindViewById<WebView>(Resource.Id.LanceTwitterEmbedWeb);
            Web.SetWebChromeClient(new WebChromeClient());
            Web.SetWebViewClient(new WebViewClient());
            Web.Settings.JavaScriptEnabled = true;
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            
            var html = $"<html><body>{lance.texto}</body><script async src=\"http://platform.twitter.com/widgets.js\" charset=\"utf-8\"></script></html>";
            Web.LoadDataWithBaseURL("https://twitter.com", html, "text/html", "UTF-8", "");
        }
    }

    public class LanceSubstituicaoViewHolder : RecyclerView.ViewHolder, ILanceViewHolder
    {
        TextView Tempo, SaiNomeJogador, SaiPosicaoJogador, EntraNomeJogador, EntraPosicaoJogador;
        ImageView SaiImgJogador, SaiImgEscudo, EntraImgJogador, EntraImgEscudo;
        Context context;
        public LanceSubstituicaoViewHolder(View view) : base(view)
        {
            Tempo = view.FindViewById<TextView>(Resource.Id.LanceSubstituicaoTxtTempo);

            SaiNomeJogador = view.FindViewById<TextView>(Resource.Id.LanceSubstituicaoSaiTxtNomeJogador);
            SaiPosicaoJogador = view.FindViewById<TextView>(Resource.Id.LanceSubstituicaoSaiTxtPosicaoJogador);
            SaiImgJogador = view.FindViewById<ImageView>(Resource.Id.LanceSubstituicaoSaiImgJogador);
            EntraNomeJogador = view.FindViewById<TextView>(Resource.Id.LanceSubstituicaoEntraTxtNomeJogador);
            EntraPosicaoJogador = view.FindViewById<TextView>(Resource.Id.LanceSubstituicaoEntraTxtPosicaoJogador);
            EntraImgJogador = view.FindViewById<ImageView>(Resource.Id.LanceSubstituicaoEntraImgJogador);

            EntraImgEscudo = view.FindViewById<ImageView>(Resource.Id.LanceSubstituicaoEntraImgClube);
            SaiImgEscudo = view.FindViewById<ImageView>(Resource.Id.LanceSubstituicaoSaiImgClube);
            context = view.Context;
        }

        public void BindLance(Lance lance)
        {
            Tempo.Text = lance.Tempo;
            SaiNomeJogador.Text = lance.substituicao.nome;
            SaiPosicaoJogador.Text = lance.substituicao.posicao_descricao;
            EntraNomeJogador.Text = lance.substituicao.substituido_por.nome;
            EntraPosicaoJogador.Text = lance.substituicao.substituido_por.posicao_descricao;
            Picasso.With(context).Load(lance.Jogador.EscudoClubeUrl).Into(EntraImgEscudo);
            Picasso.With(context).Load(lance.Jogador.EscudoClubeUrl).Into(SaiImgEscudo);
            Picasso.With(context).Load(lance.substituicao.foto).Into(SaiImgJogador);
            Picasso.With(context).Load(lance.substituicao.substituido_por.foto).Into(EntraImgJogador);
        }
    }

    public class EmptyCollectionViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView;
        public EmptyCollectionViewHolder(View view) : base(view)
        {
            TextView = view.FindViewById<TextView>(Resource.Id.EmptyCollectionText);
        }
    }

    public interface ILanceViewHolder
    {
        void BindLance(Lance lance);
    }

    
}