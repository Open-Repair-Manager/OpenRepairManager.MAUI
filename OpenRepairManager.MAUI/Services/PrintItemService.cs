using System.Diagnostics;
using OpenRepairManager.Common.Models;
using Grpc.Net.Client;
using CLRSBRepairItem;

namespace OpenRepairManager.MAUI.Services;

public static class PrintItemService
{

    public static void PrintLabel(RepairItem item)
    {
        string IPAddress = Preferences.Default.Get("PrintIP", "");
        Debug.WriteLine(IPAddress);
        // The port number must match the port of the gRPC server.
        using var channel = GrpcChannel.ForAddress($"http://{IPAddress}:5000");
        var client = new PrintItem.PrintItemClient(channel);
        var reply = client.PrintItem(
            new RepairItemRequest { Id = item.ItemGuid.ToString(), Item = item.ItemName, Owner = item.CustomerName, Whatwrong = "Something" });
    }
}