using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

[QueryProperty(nameof(GroupId), "groupId")]
[QueryProperty(nameof(GroupName), "groupName")]
[QueryProperty(nameof(IsNotFromRoot), "notFromRoot")]
public sealed partial class GroupVM : RootGroupVM
{
    public GroupVM(
        IMessenger messenger,
        IServiceScopeFactory scopeFactory,
        BrowserLauncherService browserLauncherService)
        : base(messenger, scopeFactory, browserLauncherService)
    {
        messenger.Register<CreatedLink>(this, AddLink);

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
        _messenger.Send(new MarkLinkAsFavorite(payload.Id));
    }

    private void AddLink(object _, CreatedLink args)
    {
        if (args.GroupId != GroupId) return;
        Links.Add(new() { Id = args.Id, Name = args.Name, Url = args.Url });
    }

    protected override void MarkLinkAsFavorite(object _, MarkedLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        entry.IsFavorite = true;
    }

    protected override void RemoveMarkLinkAsFavorite(object _, RemovedMarkLinkAsFavorite args)
    {
        if (Links.FirstOrDefault(x => x.Id == args.Id) is not { } entry) return;
        entry.IsFavorite = false;
    }

    protected override Task<GroupData> GetGroupData(int groupId, Storage storage) => storage.GetGroup(groupId);

    //public override void Dispose()
    //{
    //    _messenger.UnregisterAll(this);
    //    base.Dispose();
    //}
}