using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Java.Lang;
using FutebolTempoRealApp.ViewModel;
using FutebolTempoRealApp.Droid.Adapters;
using Com.Squareup.Picasso;
using Android.Content;
using Android.Runtime;
using Android.Views.Animations;
using Android.Animation;
using static Android.Animation.Animator;
using Android.Support.V7.Widget;
using Android.Widget;

namespace FutebolTempoRealApp.Droid
{
    public class PartidaPageFragment : Fragment
    {
        PartidasPages Pagina;
        PartidaViewModel ViewModel;
        Context ParentContext;
        public PartidaPageFragment(PartidasPages page, PartidaViewModel viewModel, Context parentContext)
        {
            ViewModel = viewModel;
            Pagina = page;
            ParentContext = parentContext;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            switch (Pagina)
            {
                case PartidasPages.Lances:
                    var view = inflater.Inflate(Resource.Layout.PartidaLances, container, false);

                    var listViewLances = view.FindViewById<ListView>(Resource.Id.PartidaListViewLances);

                    view.FindViewById<TextView>(Resource.Id.PartidaTxtTimeCasa).Text = ViewModel.Jogo.time_casa.nome;
                    view.FindViewById<TextView>(Resource.Id.PartidaTxtPlacar).Text = ViewModel.Jogo.Placar;
                    view.FindViewById<TextView>(Resource.Id.PartidaTxtTimeVisit).Text = ViewModel.Jogo.time_visitante.nome;
                    Picasso.With(inflater.Context).Load(ViewModel.Jogo.time_casa.escudo).Into(view.FindViewById<ImageView>(Resource.Id.PartidaImgEscudoCasa));
                    Picasso.With(inflater.Context).Load(ViewModel.Jogo.time_visitante.escudo).Into(view.FindViewById<ImageView>(Resource.Id.PartidaImgEscudoVisit));
                    view.FindViewById<TextView>(Resource.Id.PartidaTxtPlacarPenalti).Visibility = ViewStates.Gone;

                    if (ViewModel.Lances != null) listViewLances.Adapter = new LanceAdapter(this.GetLayoutInflater(savedInstanceState), ViewModel.Lances);
                    int verticalOffset = 0;
                    bool scrollingUp = false;
                    float TOOLBAR_ELEVATION = 14f;
                    ViewModel.PropertyChanged += ((s0, e0) =>
                     {
                         if (e0.PropertyName == "Lances")
                         {
                             listViewLances.Adapter = new LanceAdapter(this.GetLayoutInflater(savedInstanceState), ViewModel.Lances);
                             Toast.MakeText(inflater.Context, "Novos Lances", ToastLength.Long);

                             listViewLances.Scroll += ((s, e) =>
                             {
                                 if (e.View.GetChildAt(e.FirstVisibleItem)?.Top == null) return;
                                 int dy = e.View.GetChildAt(e.FirstVisibleItem).Top;
                                 verticalOffset += dy;
                                 scrollingUp = dy > 0;
                                 int toolbarYOffset = (int)(dy - tToolbar.TranslationY);
                                 tToolbar.Animate().Cancel();
                                 if (scrollingUp)
                                 {
                                     if (toolbarYOffset < tToolbar.Height)
                                     {
                                         if (verticalOffset > tToolbar.Height)
                                         {
                                             toolbarSetElevation(TOOLBAR_ELEVATION);
                                         }
                                         tToolbar.TranslationY = -toolbarYOffset;
                                     }
                                     else
                                     {
                                         toolbarSetElevation(0);
                                         tToolbar.TranslationY = -tToolbar.Height;
                                     }
                                 }
                                 else
                                 {
                                     if (toolbarYOffset < 0)
                                     {
                                         if (verticalOffset <= 0)
                                         {
                                             toolbarSetElevation(0);
                                         }
                                         tToolbar.TranslationY = 0;
                                     }
                                     else
                                     {
                                         if (verticalOffset > tToolbar.Height)
                                         {
                                             toolbarSetElevation(TOOLBAR_ELEVATION);
                                         }
                                         tToolbar.TranslationY = -toolbarYOffset;
                                     }
                                 }
                             });
                             listViewLances.ScrollStateChanged += ((s, e) =>
                             {
                                 if (e.ScrollState == ScrollState.Idle)
                                 {
                                     if (scrollingUp)
                                     {
                                         if (verticalOffset > tToolbar.Height)
                                         {
                                             toolbarAnimateHide();
                                         }
                                         else
                                         {
                                             toolbarAnimateShow(verticalOffset);
                                         }
                                     }
                                     else
                                     {
                                         if (tToolbar.TranslationY < tToolbar.Height * -0.6 && verticalOffset > tToolbar.Height)
                                         {
                                             toolbarAnimateHide();
                                         }
                                         else
                                         {
                                             toolbarAnimateShow(verticalOffset);
                                         }
                                     }
                                 }
                             });
                         }
                     });

                    

                    tToolbar = ((Android.App.Activity)ParentContext).FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.PartidaToolbar);
                    tToolbar.AnimationStart += (s, e) =>
                    {
                        toolbarSetElevation(verticalOffset == 0 ? 0 : TOOLBAR_ELEVATION);
                    };
                    tToolbar.AnimationEnd += (s, e) =>
                    {
                        toolbarSetElevation(0);
                    };
                    
                    
                    return view;
                case PartidasPages.Midia:
                    break;
                case PartidasPages.Resenha:
                    break;
                default:
                    break;
            }

            return new View(inflater.Context);
        }

        Android.Support.V7.Widget.Toolbar tToolbar;
        private void toolbarAnimateHide()
        {
            tToolbar.Animate()
                .TranslationY(-tToolbar.Height)
                .SetInterpolator(new LinearInterpolator())
                .SetDuration(180);
        }

        private void toolbarAnimateShow(int vOffset)
        {
            tToolbar.Animate()
                .TranslationY(0)
                .SetInterpolator(new LinearInterpolator())
                .SetDuration(180);
        }

        private void toolbarSetElevation(float elevation)
        {
            // setElevation() only works on Lollipop

            if (Build.VERSION_CODES.Lollipop == BuildVersionCodes.Lollipop)
            {
                tToolbar.Elevation = elevation;
            }
        }
    }

    public class ToolbarHidingAnimation : AbsListView.IOnScrollListener, IAnimatorListener
    {
        /// <summary>
        /// Keeps track of the overall vertical offset in the list
        /// </summary>
        int verticalOffset;
        /// <summary>
        /// Determines the scroll UP/DOWN direction
        /// </summary>
        bool scrollingUp;

        Android.Support.V7.Widget.Toolbar tToolbar;
        float TOOLBAR_ELEVATION = 14f;

        public ToolbarHidingAnimation(Android.Support.V7.Widget.Toolbar toolbar)
        {
            this.tToolbar = toolbar;
        }

        public void OnAnimationStart(Animator animation)
        {
            toolbarSetElevation(verticalOffset == 0 ? 0 : TOOLBAR_ELEVATION);
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            int dy = view.GetChildAt(firstVisibleItem).Top;
            verticalOffset += dy;
            scrollingUp = dy > 0;
            int toolbarYOffset = (int)(dy - tToolbar.TranslationY);
            tToolbar.Animate().Cancel();
            if (scrollingUp)
            {
                if (toolbarYOffset < tToolbar.Height)
                {
                    if (verticalOffset > tToolbar.Height)
                    {
                        toolbarSetElevation(TOOLBAR_ELEVATION);
                    }
                    tToolbar.TranslationY = -toolbarYOffset;
                }
                else
                {
                    toolbarSetElevation(0);
                    tToolbar.TranslationY = -tToolbar.Height;
                }
            }
            else
            {
                if (toolbarYOffset < 0)
                {
                    if (verticalOffset <= 0)
                    {
                        toolbarSetElevation(0);
                    }
                    tToolbar.TranslationY = 0;
                }
                else
                {
                    if (verticalOffset > tToolbar.Height)
                    {
                        toolbarSetElevation(TOOLBAR_ELEVATION);
                    }
                    tToolbar.TranslationY = -toolbarYOffset;
                }
            }
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            if (scrollState == ScrollState.Idle)
            {
                if (scrollingUp)
                {
                    if (verticalOffset > tToolbar.Height)
                    {
                        toolbarAnimateHide();
                    }
                    else
                    {
                        toolbarAnimateShow(verticalOffset);
                    }
                }
                else
                {
                    if (tToolbar.TranslationY < tToolbar.Height * -0.6 && verticalOffset > tToolbar.Height)
                    {
                        toolbarAnimateHide();
                    }
                    else
                    {
                        toolbarAnimateShow(verticalOffset);
                    }
                }
            }
        }

        private void toolbarSetElevation(float elevation)
        {
            // setElevation() only works on Lollipop

            if (Build.VERSION_CODES.Lollipop == BuildVersionCodes.Lollipop)
            {
                tToolbar.Elevation = elevation;
            }
        }


        private void toolbarAnimateHide()
        {
            tToolbar.Animate()
                .TranslationY(-tToolbar.Height)
                .SetInterpolator(new LinearInterpolator())
                .SetDuration(180)
                .SetListener(this);
        }

        private void toolbarAnimateShow(int vOffset)
        {
            tToolbar.Animate()
                .TranslationY(0)
                .SetInterpolator(new LinearInterpolator())
                .SetDuration(180)
                .SetListener(this);
        }

        public void OnAnimationEnd(Animator animation)
        {
            toolbarSetElevation(0);
        }

        #region Não usado
        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnAnimationCancel(Animator animation)
        {
        }

        

        public void OnAnimationRepeat(Animator animation)
        {
        }
        #endregion
    }

    public class Teste : AbsListView.IOnScrollChangeListener
    {
        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            throw new NotImplementedException();
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            throw new NotImplementedException();
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            throw new NotImplementedException();
        }
    }

    public enum PartidasPages
    {
        Lances,
        Midia,
        Resenha
    }
}