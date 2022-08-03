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
        var environmentConfig = new EnvironmentConfig();
        
        var serviceAccountProvider = new ServiceAccountProvider(
                keyId: environmentConfig.YaKeyId,
                serviceAccountId: environmentConfig.YaServiceAccountId,
                privateKey: environmentConfig.YaPrivateKey
            );
        
        serviceAccountProvider.Initialize().Wait();
        
        
        
        var config = new DriverConfig(
            endpoint: environmentConfig.YdbEndpoint,
            database: environmentConfig.YdbDbAddress,
            credentials:  serviceAccountProvider
        );
        
        _driver = new Driver(
            config: config,
            loggerFactory: null
        );
        _driver.Initialize().Wait();
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
        ExecuteQuery(query);
    }
}

public delegate T Converter<T>(ResultSet.Row row);