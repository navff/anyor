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
            saFilePath: environmentConfig.YaSaKeyFilePath!);
        
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
}

public delegate T Converter<T>(ResultSet.Row row);