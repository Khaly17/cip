using System;
using System.ComponentModel;
using System.IO;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using ImageButton = Android.Widget.ImageButton;
using Path = System.IO.Path;
using View = Android.Views.View;
#if __ANDROID_12__
using Android.Animation;
#endif

[assembly: ExportRenderer(typeof(FloatingActionButtonView), typeof(FloatingActionButtonViewRenderer))]
namespace Gefco.CipQuai.Droid.Renderers
{
    public class FloatingActionButtonViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FloatingActionButtonView, FrameLayout>
    {
        private const int MARGIN_DIPS = 16;
        private const int FAB_HEIGHT_NORMAL = 56;
        private const int FAB_HEIGHT_MINI = 40;
        private const int FAB_FRAME_HEIGHT_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_NORMAL;
        private const int FAB_FRAME_WIDTH_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_NORMAL;
        private const int FAB_MINI_FRAME_HEIGHT_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_MINI;
        private const int FAB_MINI_FRAME_WIDTH_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_MINI;
        private readonly Context context;
        private readonly FloatingActionButton fab;

        public FloatingActionButtonViewRenderer(Context context) : base(context)
        {
            this.context = context;
            var d = context.Resources.DisplayMetrics.Density;
            var margin = (int) (MARGIN_DIPS * d); // margin in pixels

            fab = new FloatingActionButton(context);
            var lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            lp.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
            lp.LeftMargin = margin;
            lp.TopMargin = margin;
            lp.BottomMargin = margin;
            lp.RightMargin = margin;
            fab.LayoutParameters = lp;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButtonView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= HandlePropertyChanged;

            if (Element != null)
                Element.PropertyChanged += HandlePropertyChanged;

            Element.Show = Show;
            Element.Hide = Hide;

            SetFabImage(Element.ImageName);
            SetFabSize(Element.Size);

            fab.ColorNormal = Element.ColorNormal.ToAndroid();
            fab.ColorPressed = Element.ColorPressed.ToAndroid();
            fab.ColorRipple = Element.ColorRipple.ToAndroid();
            fab.HasShadow = Element.HasShadow;
            fab.Click += Fab_Click;

            var frame = new FrameLayout(context);
            frame.RemoveAllViews();
            frame.AddView(fab);

            SetNativeControl(frame);
        }

        public void Show(bool animate = true)
        {
            fab.Show(animate);
        }

        public void Hide(bool animate = true)
        {
            fab.Hide(animate);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
                Tracker.UpdateLayout();
            else if (e.PropertyName == FloatingActionButtonView.ColorNormalProperty.PropertyName)
                fab.ColorNormal = Element.ColorNormal.ToAndroid();
            else if (e.PropertyName == FloatingActionButtonView.ColorPressedProperty.PropertyName)
                fab.ColorPressed = Element.ColorPressed.ToAndroid();
            else if (e.PropertyName == FloatingActionButtonView.ColorRippleProperty.PropertyName)
                fab.ColorRipple = Element.ColorRipple.ToAndroid();
            else if (e.PropertyName == FloatingActionButtonView.ImageNameProperty.PropertyName)
                SetFabImage(Element.ImageName);
            else if (e.PropertyName == FloatingActionButtonView.SizeProperty.PropertyName)
                SetFabSize(Element.Size);
            else if (e.PropertyName == FloatingActionButtonView.HasShadowProperty.PropertyName)
                fab.HasShadow = Element.HasShadow;
        }

        private void SetFabImage(string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
                try
                {
                    var drawableNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName);
                    var resources = context.Resources;
                    var imageResourceName = resources.GetIdentifier(drawableNameWithoutExtension, "drawable", context.PackageName);
                    fab.SetImageBitmap(BitmapFactory.DecodeResource(context.Resources, imageResourceName));
                }
                catch (Exception ex)
                {
                    throw new FileNotFoundException("There was no Android Drawable by that name.", ex);
                }
        }

        private void SetFabSize(FloatingActionButtonSize size)
        {
            if (size == FloatingActionButtonSize.Mini)
            {
                fab.Size = FloatingActionButtonSize.Mini;
                Element.WidthRequest = FAB_MINI_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_MINI_FRAME_HEIGHT_WITH_PADDING;
            }
            else
            {
                fab.Size = FloatingActionButtonSize.Normal;
                Element.WidthRequest = FAB_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_FRAME_HEIGHT_WITH_PADDING;
            }
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Element?.Clicked?.Invoke(sender, e);
            Element?.Command?.Execute(Element.CommandParameter);
        }
    }

    public class FloatingActionButton : ImageButton, ViewTreeObserver.IOnPreDrawListener
    {
        private const int TranslateDurationMillis = 200;


        private int _colorDisabled;

        private int _colorNormal;

        private int _colorPressed;

        private int _colorRipple;

        private bool _hasShadow;

#if __ANDROID_12__
        private readonly ITimeInterpolator _interpolator = new AccelerateDecelerateInterpolator();
#endif

        private bool _lastToggleAnimate;
        private bool _marginsSet;

        private int _shadowSize;

        private FloatingActionButtonSize _size = FloatingActionButtonSize.Normal;

        public FloatingActionButton(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public FloatingActionButton(Context context) : this(context, null)
        {
        }

        public FloatingActionButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public FloatingActionButton(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context, attrs);
        }

        /// <summary>
        ///     Gets if FAB is visible
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        ///     Gets or sets the normal color
        /// </summary>
        public int ColorNormal
        {
            get => _colorNormal;
            set
            {
                if (_colorNormal == value)
                    return;

                _colorNormal = value;
                UpdateBackground();
            }
        }

        /// <summary>
        ///     Gets or sets pressed color
        /// </summary>
        public int ColorPressed
        {
            get => _colorPressed;
            set
            {
                if (_colorPressed == value)
                    return;

                _colorPressed = value;
                UpdateBackground();
            }
        }

        /// <summary>
        ///     Gets or sets the normal color
        /// </summary>
        public int ColorDisabled
        {
            get => _colorDisabled;
            set
            {
                if (_colorDisabled == value)
                    return;

                _colorDisabled = value;
                UpdateBackground();
            }
        }

        /// <summary>
        ///     Gets or sets ripple color
        /// </summary>
        public int ColorRipple
        {
            get => _colorRipple;
            set
            {
                if (_colorRipple == value)
                    return;

                _colorRipple = value;
                UpdateBackground();
            }
        }

        /// <summary>
        ///     Gets or sets if it has a shadow
        /// </summary>
        public bool HasShadow
        {
            get => _hasShadow;
            set
            {
                if (_hasShadow == value)
                    return;

                _hasShadow = value;
                UpdateBackground();
            }
        }

        public FloatingActionButtonSize Size
        {
            get => _size;
            set
            {
                if (_size == value)
                    return;

                _size = value;
                UpdateBackground();
            }
        }

        private int MarginBottom
        {
            get
            {
                var layoutParams = LayoutParameters as ViewGroup.MarginLayoutParams;
                if (layoutParams != null)
                    return layoutParams.BottomMargin;
                return 0;
            }
        }


        private bool HasLollipopApi => (int) Build.VERSION.SdkInt >= 21;

        private bool HasJellyBeanApi => (int) Build.VERSION.SdkInt >= 16;


        private bool HasHoneycombApi => (int) Build.VERSION.SdkInt >= 11;

        public bool OnPreDraw()
        {
            var currentVto = ViewTreeObserver;
            if (currentVto.IsAlive)
                currentVto.RemoveOnPreDrawListener(this);
            Toggle(Visible, _lastToggleAnimate, true);
            return true;
        }

        /// <summary>
        ///     Sets the color normal by res id
        /// </summary>
        /// <param name="colorResId"></param>
        public void SetColorNormalResId(int colorResId)
        {
            ColorNormal = ContextCompat.GetColor(MainActivity.Instance, colorResId);
        }

        /// <summary>
        ///     Sets color pressed by res id
        /// </summary>
        /// <param name="colorResId"></param>
        public void SetColorPressedResId(int colorResId)
        {
            ColorPressed = ContextCompat.GetColor(MainActivity.Instance, colorResId);
        }

        /// <summary>
        ///     Sets the color normal by res id
        /// </summary>
        /// <param name="colorResId"></param>
        public void SetColorDisabledResId(int colorResId)
        {
            ColorDisabled = ContextCompat.GetColor(MainActivity.Instance, colorResId);
        }

        /// <summary>
        ///     Sets color ripple by res id
        /// </summary>
        /// <param name="colorResId"></param>
        public void SetColorRippleResId(int colorResId)
        {
            ColorRipple = ContextCompat.GetColor(MainActivity.Instance, colorResId);
        }

        /// <summary>
        ///     Show the FAB
        /// </summary>
        /// <param name="animate">If you want to animate, default true</param>
        public void Show(bool animate = true)
        {
            Toggle(true, animate, false);
        }

        /// <summary>
        ///     Hide the FAB
        /// </summary>
        /// <param name="animate">If you want to animate, default true</param>
        public void Hide(bool animate = true)
        {
            Toggle(false, animate, false);
        }

        private void Toggle(bool visible, bool animate, bool force)
        {
            if (Visible != visible || force)
            {
                Visible = visible;
                _lastToggleAnimate = animate;
                var height = Height;
                if (height == 0 && !force)
                {
                    var vto = ViewTreeObserver;
                    if (vto.IsAlive)
                    {
                        vto.AddOnPreDrawListener(this);
                        return;
                    }
                }
                var translationY = visible ? 0 : height + MarginBottom;
                if (animate)
                {
                    if ((int) Build.VERSION.SdkInt >= 12)
                    {
#if __ANDROID_12__
                        Animate().SetInterpolator(_interpolator).SetDuration(TranslateDurationMillis).TranslationY(translationY);
#endif
                    }
                    else
                    {
                        var oldY = !visible ? 0 : height + MarginBottom;
                        var animation = new TranslateAnimation(0, 0, oldY, translationY)
                        {
                            Duration = TranslateDurationMillis
                        };
                        if (visible)
                            animation.AnimationStart += (sender, e) => { Visibility = ViewStates.Visible; };
                        else
                            animation.AnimationEnd += (sender, e) => { Visibility = ViewStates.Gone; };
                        StartAnimation(animation);
                    }
                }
                else
                {
                    if ((int) Build.VERSION.SdkInt >= 11)
                    {
#if __ANDROID_11__
                        TranslationY = translationY;
#endif
                    }
                    else
                    {
                        var oldY = !visible ? 0 : height + MarginBottom;
                        var animation = new TranslateAnimation(0, 0, oldY, translationY)
                        {
                            Duration = 0
                        };

                        if (visible)
                            animation.AnimationStart += (sender, e) => { Visibility = ViewStates.Visible; };
                        else
                            animation.AnimationEnd += (sender, e) => { Visibility = ViewStates.Gone; };
                        StartAnimation(animation);
                    }
                }

                if (!HasHoneycombApi)
                    Clickable = visible;
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            var theSize = Resources.GetDimensionPixelSize(_size == FloatingActionButtonSize.Normal ? Resource.Dimension.fab_size_normal : Resource.Dimension.fab_size_mini);
            if (_hasShadow && !HasLollipopApi)
            {
                theSize += _shadowSize * 2;
                SetMarginsWithoutShadow();
            }
            SetMeasuredDimension(theSize, theSize);
        }

        private void Init(Context context, IAttributeSet attributeSet)
        {
            Visible = true;
            _colorNormal = ContextCompat.GetColor(MainActivity.Instance, Resource.Color.colorAccent);
            _colorPressed = DarkenColor(_colorNormal);
            _colorRipple = LightenColor(_colorNormal);
            _colorDisabled = ContextCompat.GetColor(MainActivity.Instance, Android.Resource.Color.DarkerGray);
            _size = FloatingActionButtonSize.Normal;
            _hasShadow = true;
            _shadowSize = Resources.GetDimensionPixelSize(Resource.Dimension.fab_shadow_size);
            if (attributeSet != null)
                InitAttributes(context, attributeSet);

            UpdateBackground();
        }

        private void InitAttributes(Context context, IAttributeSet attributeSet)
        {
            var attr = context.ObtainStyledAttributes(attributeSet, Resource.Styleable.FloatingActionButton, 0, 0);
            if (attr == null)
                return;

            try
            {
                _colorNormal = attr.GetColor(Resource.Styleable.FloatingActionButton_fab_colorNormal, ContextCompat.GetColor(MainActivity.Instance, Resource.Color.colorAccent));
                _colorPressed = attr.GetColor(Resource.Styleable.FloatingActionButton_fab_colorPressed, DarkenColor(_colorNormal));
                _colorRipple = attr.GetColor(Resource.Styleable.FloatingActionButton_fab_colorRipple, LightenColor(_colorNormal));
                _colorDisabled = attr.GetColor(Resource.Styleable.FloatingActionButton_fab_colorDisabled, ContextCompat.GetColor(MainActivity.Instance, Android.Resource.Color.DarkerGray));
                _hasShadow = attr.GetBoolean(Resource.Styleable.FloatingActionButton_fab_shadow, true);
                _size = (FloatingActionButtonSize) attr.GetInt(Resource.Styleable.FloatingActionButton_fab_size, 0);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                attr.Recycle();
            }
        }

        private void UpdateBackground()
        {
            var drawable = new StateListDrawable();
            drawable.AddState(new[]
            {
                Android.Resource.Attribute.StatePressed
            }, CreateDrawable(_colorPressed));
            drawable.AddState(new[]
            {
                -Android.Resource.Attribute.StateEnabled
            }, CreateDrawable(_colorDisabled));
            drawable.AddState(new int[]
            {
            }, CreateDrawable(_colorNormal));
            SetBackgroundCompat(drawable);
        }

        private Drawable CreateDrawable(int color)
        {
            var ovalShape = new OvalShape();
            var shapeDrawable = new ShapeDrawable(ovalShape);
            shapeDrawable.Paint.Color = new Color(color);
            if (_hasShadow && !HasLollipopApi)
            {
                var shadowDrawable = ContextCompat.GetDrawable(MainActivity.Instance, _size == FloatingActionButtonSize.Normal ? Resource.Drawable.fab_shadow : Resource.Drawable.fab_shadow_mini);

                var layerDrawable = new LayerDrawable(new[]
                {
                    shadowDrawable,
                    shapeDrawable
                });
                layerDrawable.SetLayerInset(1, _shadowSize, _shadowSize, _shadowSize, _shadowSize);
                return layerDrawable;
            }
            return shapeDrawable;
        }

        private void SetMarginsWithoutShadow()
        {
            if (_marginsSet)
                return;

            if (!(LayoutParameters is ViewGroup.MarginLayoutParams layoutParams))
                return;

            var leftMargin = layoutParams.LeftMargin - _shadowSize;
            var topMargin = layoutParams.TopMargin - _shadowSize;
            var rightMargin = layoutParams.RightMargin - _shadowSize;
            var bottomMargin = layoutParams.BottomMargin - _shadowSize;

            layoutParams.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);
            RequestLayout();
            _marginsSet = true;
        }

        private void SetBackgroundCompat(Drawable drawable)
        {
            if (HasLollipopApi)
            {
#if __ANDROID_21__
                var elevation = 0.0f;
                if (_hasShadow)
                    elevation = Elevation > 0.0f ? Elevation : Resources.GetDimensionPixelSize(Resource.Dimension.fab_elevation_lollipop);

                Elevation = elevation;
                var states = new[]
                {
                    new int[]
                    {
                    }
                };
                var rippleDrawable = new RippleDrawable(new ColorStateList(states, new[]
                {
                    _colorRipple
                }), drawable, null);
                OutlineProvider = new MyOutlineProvider(Resources, _size);
                ClipToOutline = true;
                Background = rippleDrawable;
#endif
            }
            else if (HasJellyBeanApi)
            {
#if __ANDROID_16__
                Background = drawable;
#endif
            }
            else
            {
                SetBackgroundCompat(drawable);
            }
        }

        private static int DarkenColor(int color)
        {
            var hsv = new float[3];
            Color.ColorToHSV(new Color(color), hsv);
            hsv[2] *= 0.9f;
            return Color.HSVToColor(hsv);
        }

        private static int LightenColor(int color)
        {
            var hsv = new float[3];
            Color.ColorToHSV(new Color(color), hsv);
            hsv[2] *= 1.1f;
            return Color.HSVToColor(hsv);
        }

#if __ANDROID_21__
        private class MyOutlineProvider : ViewOutlineProvider
        {
            private readonly FloatingActionButtonSize _fabSize;
            private readonly Resources _res;

            public MyOutlineProvider(Resources res, FloatingActionButtonSize size)
            {
                _res = res;
                _fabSize = size;
            }

            public override void GetOutline(View view, Outline outline)
            {
                var size = _res.GetDimensionPixelSize(_fabSize == FloatingActionButtonSize.Normal ? Resource.Dimension.fab_size_normal : Resource.Dimension.fab_size_mini);
                outline.SetOval(0, 0, size, size);
            }
        }
#endif
    }
}