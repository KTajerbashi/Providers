using Microsoft.AspNetCore.Mvc;
using SoapParser;
using System.Text;
using System.Xml.Linq;

namespace SoapParserWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoapParserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string endPoint = "http://localhost:2711/SoapParser.svc";

            try
            {
                // First, let's check what methods are available by getting the WSDL
                string wsdlUrl = $"{endPoint}?wsdl";
                using (var client = new HttpClient())
                {
                    var wsdlResponse = await client.GetStringAsync(wsdlUrl);
                    Console.WriteLine("WSDL retrieved successfully");
                }

                // Correct SOAP action for ReadSoapStructure method
                string soapAction = "http://tempuri.org/ISoapParser/ReadSoapStructure";

                string soapEnvelope = @"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ReadSoapStructure>
         <tem:endpointUrl>http://localhost:2711/SoapParser.svc?wsdl</tem:endpointUrl>
         <tem:username></tem:username>
         <tem:password></tem:password>
         <tem:isSSLIgnore>true</tem:isSSLIgnore>
      </tem:ReadSoapStructure>
   </soapenv:Body>
</soapenv:Envelope>";

                // Call the SOAP service
                string xmlResponse = await CallSoapService(endPoint, soapAction, soapEnvelope);

                // Parse the XML response
                var parsedResponse = ParseSoapResponse(xmlResponse);

                return Ok(new
                {
                    success = true,
                    soapAction = soapAction,
                    xmlResponse = xmlResponse,
                    parsedResponse = parsedResponse,
                    message = "SOAP call completed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    hint = "Make sure the SOAP service is running at http://localhost:2711/SoapParser.svc"
                });
            }
        }

        private async Task<string> CallSoapService(string url, string soapAction, string soapEnvelope)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url)) // Changed from GET to POST
                {
                    request.Headers.Add("SOAPAction", soapAction);
                    request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");

                    using (var response = await client.SendAsync(request))
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"SOAP call failed with status {response.StatusCode}. Response: {responseContent}");
                        }

                        return responseContent;
                    }
                }
            }
        }
        private async Task<UserModel> CallInterSoapParser()
        {
            using (var client = new SoapParserClient())
            {
                var response = await client.GetUserInfoAsync("15123456123");

                return response;
            }
        }

        private object ParseSoapResponse(string xmlResponse)
        {
            try
            {
                var doc = XDocument.Parse(xmlResponse);
                XNamespace soapNs = "http://schemas.xmlsoap.org/soap/envelope/";

                // Get the SOAP body
                var body = doc.Descendants(soapNs + "Body").FirstOrDefault();
                if (body != null)
                {
                    var responseContent = body.Elements().FirstOrDefault();
                    if (responseContent != null)
                    {
                        // Try to extract the actual data
                        var allElements = responseContent.Descendants().ToList();
                        return new
                        {
                            responseNode = responseContent.Name.LocalName,
                            hasContent = !string.IsNullOrEmpty(responseContent.Value),
                            content = responseContent.Value?.Trim(),
                            childElements = allElements.Select(e => new
                            {
                                name = e.Name.LocalName,
                                value = e.Value?.Trim(),
                                attributes = e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value)
                            }).ToList(),
                            fullXml = responseContent.ToString()
                        };
                    }
                }

                return new { rawResponse = xmlResponse };
            }
            catch (Exception ex)
            {
                return new { error = $"Failed to parse XML response: {ex.Message}", rawXml = xmlResponse };
            }
        }

        [HttpGet("discover")]
        public async Task<IActionResult> DiscoverService()
        {
            string endPoint = "http://localhost:2711/SoapParser.svc";
            string wsdlUrl = $"{endPoint}?wsdl";

            try
            {
                using (var client = new HttpClient())
                {
                    string wsdlContent = await client.GetStringAsync(wsdlUrl);

                    // Parse the WSDL to find operations
                    var doc = XDocument.Parse(wsdlContent);
                    XNamespace wsdlNs = "http://schemas.xmlsoap.org/wsdl/";
                    XNamespace soapNs = "http://schemas.xmlsoap.org/wsdl/soap/";

                    var operations = doc.Descendants(wsdlNs + "operation")
                        .Select(op => new
                        {
                            Name = op.Attribute("name")?.Value,
                            SoapAction = op.Descendants(soapNs + "operation")
                                         .FirstOrDefault()?.Attribute("soapAction")?.Value
                        })
                        .ToList();

                    // Also extract service information
                    var services = doc.Descendants(wsdlNs + "service")
                        .Select(s => new
                        {
                            Name = s.Attribute("name")?.Value,
                            Port = s.Descendants(wsdlNs + "port").FirstOrDefault()?.Attribute("name")?.Value,
                            Address = s.Descendants(soapNs + "address").FirstOrDefault()?.Attribute("location")?.Value
                        })
                        .ToList();

                    return Ok(new
                    {
                        success = true,
                        endpoint = endPoint,
                        wsdlUrl = wsdlUrl,
                        operations = operations,
                        services = services,
                        message = "Review the operations list to find your method names"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("read-internal-structure")]
        public async Task<IActionResult> ReadSoapStructure()
        {
            var response = await CallInterSoapParser();

            return Ok(response);
        }
        [HttpPost("read-structure")]
        public async Task<IActionResult> ReadSoapStructure([FromBody] ReadSoapStructureRequest request)
        {
            string endPoint = "http://localhost:2711/SoapParser.svc";

            try
            {
                string soapAction = "http://tempuri.org/ISoapParser/ReadSoapStructure";

                string soapEnvelope = $@"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ReadSoapStructure>
         <tem:endpointUrl>{request.EndpointUrl}</tem:endpointUrl>
         <tem:username>{request.Username ?? ""}</tem:username>
         <tem:password>{request.Password ?? ""}</tem:password>
         <tem:isSSLIgnore>{request.IsSSLIgnore.ToString().ToLower()}</tem:isSSLIgnore>
      </tem:ReadSoapStructure>
   </soapenv:Body>
</soapenv:Envelope>";

                string xmlResponse = await CallSoapService(endPoint, soapAction, soapEnvelope);
                var parsedResponse = ParseSoapResponse(xmlResponse);

                return Ok(new
                {
                    success = true,
                    request = request,
                    response = parsedResponse,
                    rawXml = xmlResponse
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            string endPoint = "http://localhost:2711/SoapParser.svc";
            var results = new List<string>();
            bool isReachable = false;

            try
            {
                // Test if endpoint is reachable
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync(endPoint);
                    isReachable = response.IsSuccessStatusCode;
                    results.Add($"GET {endPoint}: {(int)response.StatusCode} - {response.StatusCode}");
                }

                // Test WSDL availability
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var wsdlResponse = await client.GetAsync($"{endPoint}?wsdl");
                    results.Add($"WSDL {endPoint}?wsdl: {(int)wsdlResponse.StatusCode} - {wsdlResponse.StatusCode}");
                }

                return Ok(new
                {
                    success = isReachable,
                    endpoint = endPoint,
                    tests = results,
                    message = isReachable ? "Service is reachable" : "Service is not responding correctly"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    endpoint = endPoint,
                    error = ex.Message,
                    tests = results
                });
            }
        }
    }

    public class ReadSoapStructureRequest
    {
        public string EndpointUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSSLIgnore { get; set; } = true;
    }
}