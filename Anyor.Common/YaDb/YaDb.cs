﻿using System.ComponentModel;
using System.Diagnostics;
using Ydb.Sdk;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;
using Ydb.Sdk.Yc;

namespace Anyor.Common;

public class YaDb
{
    private Driver _driver;
    public YaDb()
    {
        
    }

    public async Task Init()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var environmentConfig = new EnvironmentConfig();
        var serviceAccountProvider = new ServiceAccountProvider(
            keyId: environmentConfig.YaKeyId,
            serviceAccountId: environmentConfig.YaServiceAccountId,
            privateKey: environmentConfig.YaPrivateKey
        );
        Console.WriteLine("START WAITING YDB");
        await serviceAccountProvider.Initialize();
        Console.WriteLine("END WAITING YDB");
        
        
        var config = new DriverConfig(
            endpoint: environmentConfig.YdbEndpoint,
            database: environmentConfig.YdbDbAddress,
            credentials:  serviceAccountProvider,
            defaultTransportTimeout: TimeSpan.FromSeconds(30),
            defaultStreamingTransportTimeout: TimeSpan.FromSeconds(30)
        );
        
        _driver = new Driver(
            config: config,
            loggerFactory: null
        );
        _driver.Initialize().Wait();
        sw.Stop();
        Console.WriteLine($"DB CONNECT INIT TIME: {sw.ElapsedMilliseconds}");
    }

    public async Task<List<T>> GetData<T>(
        string query, 
        Converter<T> converter )
    {
        Console.WriteLine($"YADB: {query}");
        using var tableClient = new TableClient(_driver, new TableClientConfig());
        
        var response = await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            query: query,
            txControl: TxControl.BeginSerializableRW().Commit()
        ));
        
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];

        return resultSet.Rows.Select(row => converter.Invoke(row)).ToList();
    }
    
    public async Task ExecuteQuery(string query)
    {
        Console.WriteLine($"YADB: {query}");
        using var tableClient = new TableClient(_driver, new TableClientConfig());
        
        var response = await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            query: query,
            txControl: TxControl.BeginSerializableRW().Commit()
        ));
        
        response.Status.EnsureSuccess();
    }
    
    public async Task RemoveRecord(string table, Guid id)
    {
        string query = $@"
            DELETE
            FROM `{table}`
            WHERE
                id = '{id.ToString()}'
           ";
        await ExecuteQuery(query);
    }
}

public delegate T Converter<T>(ResultSet.Row row);