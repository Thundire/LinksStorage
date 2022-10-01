using LinksStorage.Data;

// initialize data base
var rootPath = args.Length > 0 ? args[0] + "\\" : string.Empty;
string path =  rootPath + "LinksStorage.db3";
File.Delete(path);
var storage = new Storage(path);
await storage.Initialize();

var rootGroupId = 1;

// add groups to root
var secondGroupId = await storage.AddGroup("Youtube", rootGroupId);
var thirdGroupId = await storage.AddGroup("Twice", rootGroupId);

// add group and 2 links to group 2
var fourthGroupId = await storage.AddGroup("Random", secondGroupId);
var firstLinkId = await storage.AddLink("Tow", "ewpjpewjhewh", secondGroupId);
var secondLinkId = await storage.AddLink("Strow", "ewpjpewjhewh", secondGroupId);
// add links to group 3
var thirdLinkId = await storage.AddLink("Caw", "ewpjpewjhewh", thirdGroupId);
// add links to group 4
var fourthLinkId = await storage.AddLink("Row", "ewpjpewjhewh", fourthGroupId);

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