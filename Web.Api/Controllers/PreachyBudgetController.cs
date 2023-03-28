using System.Reflection.Metadata.Ecma335;
using System.Web;
using AmoToSheetFunc;
using AmoToSheetFunc.Dtos;
using AmoToSheetFunction;
using Api.Common;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PreachyBudgetController: ControllerBase
{
    private readonly AmoService _amoService;
    private readonly EnvironmentConfig _envConfig;

    public PreachyBudgetController()
    {
        _envConfig = new EnvironmentConfig();
        _amoService = new AmoService(_envConfig.AmoTokenConfig.AccessToken);
    }
    
    [HttpPost]
    public async Task<IActionResult> AmoHookHandle(AmoLeadStatusHook? hook = null)
    {
        foreach (var key in Request.Query.Keys)
        {
            Console.WriteLine("KEY: " + key);
            Console.WriteLine("VALUE: " + Request.Query[key]);
        }
        
        StreamReader reader = new StreamReader(Request.Body);
        string body = await reader.ReadToEndAsync();
        Console.WriteLine("BODY: " + body);
       
        /* if (request == null) return BadRequest();
            
        
            
        var amoRequest = JsonConvert.DeserializeObject<AmoHookRequest>(request);
        if (amoRequest == null) return BadRequest("Cannot deserialize webhook");
*/
        //var hook = ParseHook(base64request);

        Console.WriteLine("LEAD_ID: " + hook.LeadId);

        Task.Run(() =>
        {
            var lead = _amoService.GetAmoLead(hook.LeadId);
            var contact = _amoService.GetAmoContact(lead.Embedded.Contacts.First().Id);
            Console.WriteLine("CONTACT_NAME: " + contact.Name);

            var amount = lead.CustomFieldsValues
                .FirstOrDefault(f => f.FieldName == "tinkoff_amount")?.Values
                .FirstOrDefault()?.Value;

            var sheetDonationService = new SheetDonationsAddService(_envConfig.GoogleCredsJson);
            sheetDonationService.AddNewRow(new Donation
            {
                AmoLeadId = lead.Id,
                Amount = amount,
                Date = DateTime.Now.AddHours(3),
                ContactName = contact.Name
            });  
        });

        return Ok();
    }

    [HttpPut("AmoHookHandle")]
    public async Task<IActionResult> AmoHookHandlePut(AmoLeadStatusHook? hook = null)
    {
        foreach (var key in Request.Query.Keys)
        {
            Console.WriteLine("KEY: " + key);
            Console.WriteLine("VALUE: " + Request.Query[key]);
        }

        StreamReader reader = new StreamReader(Request.Body);
        string body = await reader.ReadToEndAsync();
        Console.WriteLine("BODY: " + body);

        return Ok();
    }
    
    [HttpGet("AmoHookHandle")]
    public async Task<IActionResult> AmoHookHandleGet(AmoLeadStatusHook? hook = null)
    {
        foreach (var key in Request.Query.Keys)
        {
            Console.WriteLine("KEY: " + key);
            Console.WriteLine("VALUE: " + Request.Query[key]);
        }

        StreamReader reader = new StreamReader(Request.Body);
        string body = await reader.ReadToEndAsync();
        Console.WriteLine("BODY: " + body);

        return Ok();
    }

    private AmoLeadStatusHook ParseHook(string valueToConvert)
    {
            
        var base64EncodedBytes = Convert.FromBase64String(valueToConvert);
        var decodedBody = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        var unescaped = Uri.UnescapeDataString(decodedBody);
        var valueCollection = HttpUtility.ParseQueryString(unescaped);

        return new AmoLeadStatusHook
        {
            LeadId = long.Parse(valueCollection["leads[status][0][id]"]),
            NewPipelineId = long.Parse(valueCollection["leads[status][0][pipeline_id]"]),
            NewStatusId = long.Parse(valueCollection["leads[status][0][status_id]"]),
            OldPipelineId = long.Parse(valueCollection["leads[status][0][old_pipeline_id]"]),
            OldStatusId = long.Parse(valueCollection["leads[status][0][old_status_id]"])
        };
    }
}