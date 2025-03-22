using Android.Graphics.Drawables;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Color = Android.Graphics.Color;

namespace maui_lab_1.Platforms.Android
{
    public class BorderedEntryHandler : EntryHandler
    {
        public BorderedEntryHandler() : base(EntryHandler.Mapper)
        {
        }

        protected override void ConnectHandler(AppCompatEditText nativeView)
        {
            base.ConnectHandler(nativeView);
            ApplyBorder(nativeView);
        }

        private void ApplyBorder(AppCompatEditText editText)
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetColor(Color.Black);
            gradientDrawable.SetStroke(5, Color.Red);
            gradientDrawable.SetCornerRadius(10);
            editText.Background = gradientDrawable;
            editText.SetTextColor(Color.White);
        }
    }
}
