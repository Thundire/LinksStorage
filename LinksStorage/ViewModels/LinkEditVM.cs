using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class LinkEditVM : ObservableObject, IQueryAttributable
{
    private readonly IMessagingCenter _messenger;
    private static readonly object SenderMark = new();
    private int _group;
    private bool _isFromRoot;
    private int _id;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _url;

    public LinkEditVM(IMessagingCenter messenger)
    {
        _messenger = messenger;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var payload = query["payload"];
        if (payload is LinkCreateInfo createInfo)
        {
            _id = 0;
            _isNew = true;
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
        if (_isNew)
        {
            _messenger.Send(SenderMark, nameof(CreateLink), new CreateLink(_name, _url, _group));
        }
        else
        {
            _messenger.Send(SenderMark, nameof(EditLink), new EditLink(_id, _name, _url, _group));
        }

        await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
        {
            ["group"] = _isFromRoot ? null : _group
        });
    }
}