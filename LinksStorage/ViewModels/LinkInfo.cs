using CommunityToolkit.Mvvm.ComponentModel;
using LinksStorage.Data;
using LinksStorage.Services;

namespace LinksStorage.ViewModels;

public partial class LinkInfo : ObservableObject
{
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _url;
    [ObservableProperty] private bool _isFavorite;

    public LinkInfo() { }
    public LinkInfo(LinkInfoData data)
    {
        _id = data.Id;
        _name = data.Name;
        _url = data.Url;
    }
    
    public void Update(EditLink data) => Update(data.Name, data.Url);

    private void Update(string name, string url)
    {
        Name = name;
        Url = url;
    }
}