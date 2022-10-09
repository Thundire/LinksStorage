using System.Windows.Input;

namespace LinksStorage.Controls;

public partial class IconButton : ContentView
{
    public IconButton() => InitializeComponent();

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(IconButton),
        default(ICommand));
    public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(IconButton),
        default(object));
    public object CommandParameter { get => (object)GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

    public static readonly BindableProperty GlyphProperty = BindableProperty.Create(
        nameof(Glyph),
        typeof(ImageSource),
        typeof(IconButton),
        default(ImageSource));
    public ImageSource Glyph { get => (ImageSource)GetValue(GlyphProperty); set => SetValue(GlyphProperty, value); }

    public event EventHandler Tapped;

    private void OnTapped(object sender, EventArgs e)
    {
        Tapped?.Invoke(sender, e);
    }
}