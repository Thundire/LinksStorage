using CommunityToolkit.Mvvm.Input;
using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

[QueryProperty(nameof(GroupId), "group")]
public sealed partial class GroupVM : RootGroupVM
{
    public GroupVM(IMessagingCenter messenger, IServiceScopeFactory scopeFactory) : base(messenger, scopeFactory)
    {
        messenger.Subscribe<DataPersistenceOutbox, CreatedLink>(this, nameof(CreatedLink), AddLink);

    }

    [RelayCommand]
    private async Task AddLink()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.LinkEditForm, new Dictionary<string, object>()
        {
            ["payload"] = new LinkCreateInfo(GroupId)
        });
    }

    [RelayCommand]
    private void MarkLinkAsFavorite(LinkInfo payload)
    {
        _messenger.Send(SenderMark, nameof(Services.MarkLinkAsFavorite), new MarkLinkAsFavorite(payload.Id));
    }

    private void AddLink(DataPersistenceOutbox _, CreatedLink args)
    {
        if(args.GroupId != GroupId) return;
        Links.Add(new(){Id = args.Id, Name = args.Name, Url = args.Url});
    }

    protected override void MarkLinkAsFavorite(DataPersistenceOutbox _, MarkedLinkAsFavorite args)
    {
        if(Links.FirstOrDefault(x=>x.Id == args.Id) is not {} entry) return;
        entry.IsFavorite = true;
    }

    protected override void RemoveMarkLinkAsFavorite(DataPersistenceOutbox _, RemovedMarkLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        entry.IsFavorite = false;
    }

    protected override Task<GroupData> GetGroupData(int groupId, Storage storage) => storage.GetGroup(groupId);

    public override void Dispose()
    {
        _messenger.Unsubscribe<DataPersistenceOutbox, CreatedLink>(this, nameof(CreatedLink));
        base.Dispose();
    }
}