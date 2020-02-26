using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TestExample.UserControls
{
    public interface IView
    {
        bool IsReady { get; }
        event EventHandler Ready;
        bool IsVisible { get; set; }
    }
    [ContentProperty("Content")]
    public class ViewPresenter : Control
    {
        #region Dependency Properties
        public static readonly DependencyProperty ContentProperty;
        public static readonly DependencyProperty DefaultStoryboardProperty;
        public static readonly DependencyProperty StoryboardProperty;
        public static readonly DependencyProperty StoryboardSelectorProperty;
        public static readonly DependencyProperty OldContentProperty;
        public static readonly DependencyProperty NewContentProperty;
        public static readonly DependencyProperty OldContentTranslateXProperty;
        public static readonly DependencyProperty NewContentTranslateXProperty;
        static ViewPresenter()
        {
            Type ownerType = typeof(ViewPresenter);
            ContentProperty = DependencyProperty.Register("Content", typeof(object), ownerType, new PropertyMetadata(null, RaiseContentChanged));
            DefaultStoryboardProperty = DevExpress.RealtorWorld.Xpf.Helpers.StoryboardProperty.Register("DefaultStoryboard", ownerType, null);
            StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(string), ownerType, new PropertyMetadata(string.Empty));
            StoryboardSelectorProperty = DependencyProperty.Register("StoryboardSelector", typeof(Func<object, string>), ownerType, new PropertyMetadata(null));
            OldContentProperty = DependencyProperty.Register("OldContent", typeof(ContentPresenter), ownerType, new PropertyMetadata(null));
            NewContentProperty = DependencyProperty.Register("NewContent", typeof(ContentPresenter), ownerType, new PropertyMetadata(null));
            OldContentTranslateXProperty = DependencyProperty.Register("OldContentTranslateX", typeof(double), ownerType, new PropertyMetadata(0.0, RaiseOldContentTranslateXChanged));
            NewContentTranslateXProperty = DependencyProperty.Register("NewContentTranslateX", typeof(double), ownerType, new PropertyMetadata(0.0, RaiseNewContentTranslateXChanged));
        }
        static void RaiseContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewPresenter)d).RaiseContentChanged(e.OldValue, e.NewValue);
        }
        static void RaiseOldContentTranslateXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewPresenter)d).RaiseOldContentTranslateXChanged();
        }
        static void RaiseNewContentTranslateXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewPresenter)d).RaiseNewContentTranslateXChanged();
        }
        #endregion
        private bool _animationInProgress;
        private Grid _grid;
        private Storyboard _storyboard;
        private bool _contentChanged;

        public ViewPresenter()
        {
            DefaultStyleKey = typeof(ViewPresenter);
            SizeChanged += OnSizeChanged;
        }
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public Storyboard DefaultStoryboard
        {
            get => (Storyboard)GetValue(DefaultStoryboardProperty);
            set => SetValue(DefaultStoryboardProperty, value);
        }
        public string Storyboard
        {
            get => (string)GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }
        public Func<object, string> StoryboardSelector
        {
            get => (Func<object, string>)GetValue(StoryboardSelectorProperty);
            set { SetValue(StoryboardSelectorProperty, value); }
        }
        public ContentPresenter OldContent
        {
            get => (ContentPresenter)GetValue(OldContentProperty);
            private set { SetValue(OldContentProperty, value); }
        }
        public ContentPresenter NewContent
        {
            get => (ContentPresenter)GetValue(NewContentProperty);
            private set { SetValue(NewContentProperty, value); }
        }
        public double OldContentTranslateX
        {
            get => (double)GetValue(OldContentTranslateXProperty);
            set { SetValue(OldContentTranslateXProperty, value); }
        }
        public double NewContentTranslateX
        {
            get => (double)GetValue(NewContentTranslateXProperty);
            set { SetValue(NewContentTranslateXProperty, value); }
        }
        TranslateTransform OldContentTranslate => ((TransformGroup)OldContent.RenderTransform).Children[1] as TranslateTransform;
        TranslateTransform NewContentTranslate => ((TransformGroup)NewContent.RenderTransform).Children[1] as TranslateTransform;

        protected virtual void SubscribeToReady(object view, EventHandler handler)
        {
            var v = view as IView;
            if (v != null)
                v.Ready += handler;
        }
        protected virtual void UnsubscribeFromReady(object view, EventHandler handler)
        {
            var v = view as IView;
            if (v != null)
                v.Ready -= handler;
        }
        protected virtual bool IsReady(object view)
        {
            var v = view as IView;
            return v?.IsReady ?? true;
        }
        protected virtual void SetIsVisible(object view, bool value)
        {
            var v = view as IView;
            if (v != null)
                v.IsVisible = value;
        }
        void SetStoryboard(object content)
        {
            if (string.IsNullOrEmpty(Storyboard) && StoryboardSelector == null)
            {
                this._storyboard = DefaultStoryboard;
            }
            else
            {
                string name = string.IsNullOrEmpty(Storyboard) ? StoryboardSelector(content) : Storyboard;
                this._storyboard = Resources[name] as Storyboard;
            }
        }
        void RaiseContentChanged(object oldValue, object newValue)
        {
            if (this._animationInProgress)
            {
                this._contentChanged = true;
                return;
            }
            this._animationInProgress = true;
            if (OldContent != null && OldContent.Content != null)
                SetIsVisible(OldContent.Content, false);
            NewContent = new ContentPresenter() { Content = newValue, RenderTransformOrigin = new Point(0.5, 0.5), Opacity = 0.0 };
            Canvas.SetZIndex(NewContent, 5);
            InitTransform(NewContent);
            if (this._grid != null)
                this._grid.Children.Add(NewContent);
            SetStoryboard(newValue);
            if (OldContent == null || this._storyboard == null)
            {
                NewContent.Opacity = 1.0;
                FinishContentChanging();
                return;
            }
            System.Windows.Media.Animation.Storyboard.SetTarget(this._storyboard, this);
            this._storyboard.Completed += OnStoryboardCompleted;
            SubscribeToReady(newValue, OnNewValueReady);
            if (IsReady(newValue))
                OnNewValueReady(newValue, null);
        }
        void OnStoryboardCompleted(object sender, EventArgs e)
        {
            NewContent.Opacity = 1.0;
            this._storyboard.Completed -= OnStoryboardCompleted;
            this._storyboard.Stop();
            this._storyboard = null;
            FinishContentChanging();
        }
        void OnNewValueReady(object sender, EventArgs e)
        {
            UnsubscribeFromReady(sender, OnNewValueReady);
            this._storyboard.Begin();
        }
        void FinishContentChanging()
        {
            if (this._grid != null)
                this._grid.Children.Remove(OldContent);
            OldContent = NewContent;
            NewContent = null;
            Canvas.SetZIndex(OldContent, 10);
            InitTransform(OldContent);
            if (OldContent != null && OldContent.Content != null)
                SetIsVisible(OldContent.Content, true);
            this._animationInProgress = false;
            if (this._contentChanged)
            {
                this._contentChanged = false;
                RaiseContentChanged(null, Content);
            }
        }
        void InitTransform(FrameworkElement fe)
        {
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform());
            transform.Children.Add(new TranslateTransform());
            fe.RenderTransform = transform;
        }
        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry() { Rect = new Rect(0.0, 0.0, ActualWidth, ActualHeight) };
            if (OldContent != null)
                RaiseOldContentTranslateXChanged();
            if (NewContent != null)
                RaiseNewContentTranslateXChanged();
        }
        void RaiseOldContentTranslateXChanged()
        {
            if (!this._animationInProgress) return;
            OldContentTranslate.X = OldContentTranslateX * ActualWidth;
        }
        void RaiseNewContentTranslateXChanged()
        {
            if (!this._animationInProgress) return;
            NewContentTranslate.X = NewContentTranslateX * ActualWidth;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._grid = (Grid)GetTemplateChild("Grid");
            if (OldContent != null)
                this._grid.Children.Add(OldContent);
            if (NewContent != null)
                this._grid.Children.Add(NewContent);
        }
    }
}

namespace DevExpress.RealtorWorld.Xpf.Helpers
{
    public static class StoryboardProperty
    {
        public static DependencyProperty Register(string name, Type ownerType, object defaultValue)
        {
            return Register(name, ownerType, defaultValue, null);
        }
        public static DependencyProperty Register(string name, Type ownerType, object defaultValue, PropertyChangedCallback propertyChangedCallback)
        {
            return DependencyProperty.Register(name, typeof(Storyboard), ownerType, new PropertyMetadata(defaultValue, propertyChangedCallback
                , CoerceStoryboard
            ));
        }
        static object CoerceStoryboard(DependencyObject d, object source)
        {
            var sb = (Storyboard)source;
            return sb?.Clone();
        }
    }
}
