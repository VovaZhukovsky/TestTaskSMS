﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using TestTaskSMS.CommonLibrary.Model;
using Sms.Test;

namespace TestTaskSMS.gRPSClient;

public class gRPSClient
{
    private readonly SmsTestService.SmsTestServiceClient _client;

    public gRPSClient(string _url)
    {
        var channel = GrpcChannel.ForAddress(_url);
        _client = new SmsTestService.SmsTestServiceClient(channel);
    }

    public async Task<CommonLibrary.Model.GetMenuResponse> GetMenuAsync()
    {

        var grpsMenuResponse = await _client.GetMenuAsync(new BoolValue { Value = true });

        CommonLibrary.Model.GetMenuResponse result = new()
        {
            Success = grpsMenuResponse.Success,
            ErrorMessage = grpsMenuResponse.ErrorMessage
        };

        if (grpsMenuResponse.Success)
        {
            result.Data = new();

            foreach (var item in grpsMenuResponse.MenuItems)
            {
                result.Data.MenuItems.Add(new()
                {
                    Id = item.Id,
                    Article = item.Article,
                    Name = item.Name,
                    Price = item.Price,
                    IsWeighted = item.IsWeighted,
                    FullPath = item.FullPath,
                    Barcodes = item.Barcodes.ToList()
                });
            }
        }
        return result;
    }

    public async Task<SendOrderRespose> SendOrderAsync(OrderParams orderParams)
    {

        Sms.Test.Order grpsOrderParams = new() { Id = orderParams.OrderId };

        grpsOrderParams.OrderItems.AddRange(orderParams.MenuItems.Select(o => new Sms.Test.OrderItem
        {
            Id = o.Id,
            Quantity = o.Quantity
        }));

        var grpsGetOrderRespose = await _client.SendOrderAsync(grpsOrderParams);

        return new SendOrderRespose()
        {
            Success = grpsGetOrderRespose.Success,
            ErrorMessage = grpsGetOrderRespose.ErrorMessage
        };
    }
}
