using System.Windows.Input;
using LinksStorage.ViewModels;

namespace LinksStorage.Controls;

public partial class LinkView : ContentView
{
    public LinkView() => InitializeComponent();

    public static readonly BindableProperty LinkProperty = BindableProperty.Create(
        nameof(Link),
        typeof(LinkInfo),
        typeof(LinkView),
        default(LinkInfo));
    public LinkInfo Link { get => (LinkInfo)GetValue(LinkProperty); set => SetValue(LinkProperty, value); }

    public static readonly BindableProperty EditCommandProperty = BindableProperty.Create(
        nameof(EditCommand),
        typeof(ICommand),
        typeof(LinkView),
        default(ICommand));
    public ICommand EditCommand { get => (ICommand)GetValue(EditCommandProperty); set => SetValue(EditCommandProperty, value); }

    public static readonly BindableProperty RemoveCommandProperty = BindableProperty.Create(
        nameof(RemoveCommand),
        typeof(ICommand),
        typeof(LinkView),
        default(ICommand));
    public ICommand RemoveCommand { get => (ICommand)GetValue(RemoveCommandProperty); set => SetValue(RemoveCommandProperty, value); }

    public static readonly BindableProperty MarkAsFavoriteCommandProperty = BindableProperty.Create(
        nameof(MarkAsFavoriteCommand),
        typeof(ICommand),
        typeof(LinkView),
        default(ICommand));
    public ICommand MarkAsFavoriteCommand { get => (ICommand)GetValue(MarkAsFavoriteCommandProperty); set => SetValue(MarkAsFavoriteCommandProperty, value); }
    
    public static readonly BindableProperty RemoveMarkAsFavoriteCommandProperty = BindableProperty.Create(
        nameof(RemoveMarkAsFavoriteCommand),
        typeof(ICommand),
        typeof(LinkView),
        default(ICommand));
    public ICommand RemoveMarkAsFavoriteCommand { get => (ICommand)GetValue(RemoveMarkAsFavoriteCommandProperty); set => SetValue(RemoveMarkAsFavoriteCommandProperty, value); }

    public static readonly BindableProperty OpenInBrowserCommandProperty = BindableProperty.Create(
        nameof(OpenInBrowserCommand),
        typeof(ICommand),
        typeof(LinkView),
        default(ICommand));
    public ICommand OpenInBrowserCommand { get => (ICommand)GetValue(OpenInBrowserCommandProperty); set => SetValue(OpenInBrowserCommandProperty, value); }
}