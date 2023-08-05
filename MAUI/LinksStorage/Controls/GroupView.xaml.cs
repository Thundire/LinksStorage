using System.Windows.Input;
using LinksStorage.ViewModels;

namespace LinksStorage.Controls;

public partial class GroupView : ContentView
{
	public GroupView() => InitializeComponent();

    public static readonly BindableProperty OpenCommandProperty = BindableProperty.Create(
        nameof(OpenCommand),
        typeof(ICommand),
        typeof(GroupView),
        default(ICommand));
    public ICommand OpenCommand { get => (ICommand)GetValue(OpenCommandProperty); set => SetValue(OpenCommandProperty, value); }

    public static readonly BindableProperty GroupProperty = BindableProperty.Create(
        nameof(Group),
        typeof(GroupInfo),
        typeof(GroupView),
        default(GroupInfo));
    public GroupInfo Group { get => (GroupInfo)GetValue(GroupProperty); set => SetValue(GroupProperty, value); }

    public static readonly BindableProperty ChangeNameCommandProperty = BindableProperty.Create(
        nameof(ChangeNameCommand),
        typeof(ICommand),
        typeof(GroupView),
        default(ICommand));
    public ICommand ChangeNameCommand { get => (ICommand)GetValue(ChangeNameCommandProperty); set => SetValue(ChangeNameCommandProperty, value); }

    public static readonly BindableProperty RemoveCommandProperty = BindableProperty.Create(
        nameof(RemoveCommand),
        typeof(ICommand),
        typeof(GroupView),
        default(ICommand));
    public ICommand RemoveCommand { get => (ICommand)GetValue(RemoveCommandProperty); set => SetValue(RemoveCommandProperty, value); }
}