using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class LinkEditVM : ObservableObject, IQueryAttributable
{
    private readonly IMessenger _messenger;
    private static readonly object SenderMark = new();
    private Guid _group;
    private bool _isFromRoot;
    private Guid _id;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _url;

    public LinkEditVM(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var payload = query["payload"];
        if (payload is LinkCreateInfo createInfo)
        {
            _id = Guid.Empty;
            IsNew = true;
            _group = createInfo.Group;
            return;
        }

        if (payload is not LinkEditInfo editInfo) throw new InvalidOperationException("Link Edit VM need for edit or create links");

        _id = editInfo.Id;
        _group = editInfo.Group;
        Name = editInfo.Name;
        Url = editInfo.Url;
        _isFromRoot = editInfo.IsFromRoot;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (IsNew)
        {
            _messenger.Send(new CreateLink(Name, Url, _group));
        }
        else
        {
            _messenger.Send(new EditLink(_id, Name, Url, _group));
        }

        await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
        {
            ["group"] = _isFromRoot ? null : _group
        });
    }
}