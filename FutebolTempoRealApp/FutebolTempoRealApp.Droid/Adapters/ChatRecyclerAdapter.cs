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
using ClienteResenha.ViewModel;
using SharedResenha;

namespace FutebolTempoRealApp.Droid.Adapters
{
    public class ChatRecyclerAdapter : RecyclerView.Adapter
    {
        ResenhaViewModel ViewModel;
        Activity Parent;
        public ChatRecyclerAdapter(Activity context, ResenhaViewModel viewModel)
        {
            Parent = context;
            ViewModel = viewModel;
        }

        public Mensagem GetValueAt(int position)
        {
            return ViewModel[position];
        }

        public override int ItemCount
        {
            get
            {
                return ViewModel.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var chatView = holder as ChatViewHolder;
            chatView.Bind(ViewModel[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatView, parent, false);
            return new ChatViewHolder(view);
        }

        class ChatViewHolder : RecyclerView.ViewHolder
        {
            TextView Nome, Mensagem, Data;
            public ChatViewHolder(View view) : base(view)
            {
                Nome = view.FindViewById<TextView>(Resource.Id.ChatTxtUsuario);
                Mensagem = view.FindViewById<TextView>(Resource.Id.ChatTxtMensagem);
                Data = view.FindViewById<TextView>(Resource.Id.ChatTxtData);
            }

            public void Bind(Mensagem mensagem)
            {
                Nome.Text = mensagem.Nome;
                Mensagem.Text = mensagem.Texto;
                Data.Text = mensagem.Data;
            }
        }
    }
}