using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace GradientOverlayWithInset.Controls
{
    public class GradientOverlayView : ContentView
    {
        private static readonly Color _defaultStartColor = Color.White.MultiplyAlpha(0.95); // White @ 95% opacity
        private static readonly Color _defaultEndColor = Color.White.MultiplyAlpha(0.0); // White @ 0% opacity

        public static readonly BindableProperty StartColorProperty = BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(GradientOverlayView), _defaultStartColor, Xamarin.Forms.BindingMode.OneWay);
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); OnPropertyChanged(nameof(StartColor)); }
        }

        public static readonly BindableProperty EndColorProperty = BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(GradientOverlayView), _defaultEndColor, Xamarin.Forms.BindingMode.OneWay);
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); OnPropertyChanged(nameof(EndColor)); }
        }

        /// <summary>
        /// If true, the first part of the gradient percentage specified in GradientStartHeavyPercent will be constant instead of gradient using the StartColor property.
        /// This is useful when text or other things overlayed at the top need to be displayed more clearly.
        /// </summary>
        public static readonly BindableProperty HasGradientStartInsetProperty = BindableProperty.Create(nameof(HasGradientStartInset), typeof(bool), typeof(GradientOverlayView), false, Xamarin.Forms.BindingMode.OneWay);
        public bool HasGradientStartInset
        {
            get { return (bool)GetValue(HasGradientStartInsetProperty); }
            set { SetValue(HasGradientStartInsetProperty, value); OnPropertyChanged(nameof(HasGradientStartInset)); }
        }

        /// <summary>
        /// Specifies the heavy gradient percentage deom 0.0 - 1.0, essentially at what point in the diagonal line of the gradient that the start color stops being constant and turns to the gradient. Default is 0.2f (20%).
        /// </summary>
        public static readonly BindableProperty GradientStartInsetPercentProperty = BindableProperty.Create(nameof(GradientStartInsetPercent), typeof(float), typeof(GradientOverlayView), 0.20f, Xamarin.Forms.BindingMode.OneWay);
        public float GradientStartInsetPercent
        {
            get { return (float)GetValue(GradientStartInsetPercentProperty); }
            set { SetValue(GradientStartInsetPercentProperty, value); OnPropertyChanged(nameof(GradientStartInsetPercent)); }
        }

        public GradientOverlayView()
        {
            try
            {
                var canvasView = new SKCanvasView();
                canvasView.PaintSurface += OnCanvasViewPaintSurface;
                canvasView.BackgroundColor = Color.Transparent;
                Content = canvasView;
            }
            catch (Exception ex)
            {
                // Don't crash for a pretty effect
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            try
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                var startPoint = new SKPoint(0, 0);
                var endPoint = new SKPoint(info.Width, info.Height);

                SKColor[] colors;
                SKShader shader;

                if (HasGradientStartInset)
                {
                    colors = new SKColor[] { StartColor.ToSKColor(), StartColor.ToSKColor(), EndColor.ToSKColor() };
                    shader = SKShader.CreateLinearGradient(startPoint, endPoint, colors, new float[] { 0, GradientStartInsetPercent, 1 }, SKShaderTileMode.Clamp);
                }
                else
                {
                    colors = new SKColor[] { StartColor.ToSKColor(), EndColor.ToSKColor() };
                    shader = SKShader.CreateLinearGradient(startPoint, endPoint, colors, null, SKShaderTileMode.Clamp);
                }

                var mainPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Shader = shader
                };

                canvas.DrawRect(new SKRect(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y), mainPaint);

            }
            catch (Exception ex)
            {
                // Don't crash for a pretty effect
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}

