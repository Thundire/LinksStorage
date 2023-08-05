using Bogus;
using LinksStorage.Data;

// initialize data base
var rootPath = args.Length > 0 ? args[0] + "\\" : string.Empty;
string path =  rootPath + "LinksStorage.db3";
File.Delete(path);
var storage = new Storage(path);
await storage.Initialize();

Faker faker = new();

var rootGroupId = Guid.Parse("49c1c87d-1f05-450e-b830-75ea36468d71");
// add groups to root
var secondGroupId = await storage.AddGroup(faker.Internet.DomainWord(), rootGroupId);
var thirdGroupId = await storage.AddGroup(faker.Internet.DomainWord(), rootGroupId);

// add group and 2 links to group 2
var fourthGroupId = await storage.AddGroup(faker.Internet.DomainWord(), secondGroupId);
var firstLinkId = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), secondGroupId);
var secondLinkId = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), secondGroupId);
// add links to group 3
var thirdLinkId = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), thirdGroupId);
// add links to group 4
var fourthLinkId = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), fourthGroupId);

Console.WriteLine("-----Data initialized---------");
Console.WriteLine(await storage.GetRootPage() + "\n");
Console.WriteLine(await storage.GetGroup(secondGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(thirdGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(fourthGroupId) + "\n");

// register link 1 and 2 as favorites so its will show on root page
await storage.RegisterFavoriteLink(firstLinkId);
await storage.RegisterFavoriteLink(secondLinkId);

// change name of fourth group
await storage.ChangeGroupName(fourthGroupId, "Some name");

Console.WriteLine("--------Data changed----------");
Console.WriteLine(await storage.GetRootPage() + "\n");
Console.WriteLine(await storage.GetGroup(secondGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(thirdGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(fourthGroupId) + "\n");

await storage.RemoveLinkFromFavorites(secondLinkId);

Console.WriteLine("---------Hot link removed--------");
Console.WriteLine(await storage.GetRootPage() + "\n");

await storage.RemoveLink(fourthLinkId);
await storage.RemoveGroup(secondGroupId);

Console.WriteLine("---------Data removed--------");
Console.WriteLine(await storage.GetRootPage() + "\n");
Console.WriteLine(await storage.GetGroup(secondGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(thirdGroupId) + "\n");
Console.WriteLine(await storage.GetGroup(fourthGroupId) + "\n");

await storage.RemoveGroup(secondGroupId);
await storage.RemoveGroup(thirdGroupId);
await storage.RemoveGroup(fourthGroupId);


var group4 = await storage.AddGroup(faker.Internet.DomainWord(), rootGroupId);
var group3 = await storage.AddGroup(faker.Internet.DomainWord(), rootGroupId);
var group2 = await storage.AddGroup(faker.Internet.DomainWord(), group3);
var group1 = await storage.AddGroup(faker.Internet.DomainWord(), group3);
var favorite1 = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), secondGroupId);
var favorite2 = await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), secondGroupId);
// add links to group 3
for (var i = 0; i < 4; i++)
{
    await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), thirdGroupId);
}
// add links to group 4

for (var i = 0; i < 4; i++)
{
    await storage.AddLink(faker.Internet.DomainName(), faker.Internet.Url(), group4);
}

await storage.RegisterFavoriteLink(favorite1);
await storage.RegisterFavoriteLink(favorite2);

Console.WriteLine("---------Data generated--------");
Console.WriteLine(await storage.GetRootPage() + "\n");
Console.WriteLine(await storage.GetGroup(group1) + "\n");
Console.WriteLine(await storage.GetGroup(group2) + "\n");
Console.WriteLine(await storage.GetGroup(group3) + "\n");
Console.WriteLine(await storage.GetGroup(group4) + "\n");

var data = await storage.Export();

Console.WriteLine("---------Export data--------");
Console.WriteLine(data);