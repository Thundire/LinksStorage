using CommunityToolkit.Maui.Views;

using LinksStorage.ViewModels;

namespace LinksStorage.Forms;

public partial class GroupEditForm : Popup
{
    public GroupEditForm() => InitializeComponent();

    private void Ok(object sender, EventArgs e)
    {
        MessagingCenter.Send<GroupEditForm, GroupName>(this, "changeGroupName", new(){Name = GroupNameEntry.Text});
        Close();
    }

    private void Cancel(object sender, EventArgs e) => Close();
}