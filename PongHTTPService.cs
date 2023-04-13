using PongService.Modules;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PongService
{
    public class PongHTTPService
    {
        
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private HttpListener _service;
        private static PongHTTPService _instance = null;
        private static readonly object syncLock = new object();

        public TangibleModule TangibleModule { get; set; } = new();
        public DlKilnectModule DlKilnectModule { get; set; } = new();
        public VoiceModule VoiceModule { get; set; } = new();
        public SensorsModule SensorsModule { get; set; } = new();

        private PongHTTPService()
        {
            var url = "http://localhost:5001/";
            _service = new HttpListener();
            _service.Prefixes.Add(url);
            _service.Start();
        }

        public static PongHTTPService GetInstance()
        {
            lock (syncLock)
            {
                if (_instance == null)
                {
                    _instance = new PongHTTPService();
                }
                return _instance;
            }
        }

        /// <summary>method <c>ServerListenerAsync</c> Start listening http requests.</summary>
        public async Task ServerListenerAsync()
        {
            while (true)
            {
            var context = _service.GetContext();
            ProcessRequest(context);
            }
        }

        /// <summary>method <c>ProcessRequest</c>Logic of proccessing http request.</summary>
        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "POST")
            {
                ProcessMessage(context);
                context.Response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
            response.Close();
        }

        /// <summary>method <c>ProcessMessage</c> Takes HttpListenerContext, then deserialize json body to appropriate property of type ModuleBase.</summary>
        private void ProcessMessage(HttpListenerContext context)
        {
            using (var streamReader = new StreamReader(context.Request.InputStream))
            {
                var json = streamReader.ReadToEnd();
                var url = context.Request.Url;

                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                JsonElement idElement = root.GetProperty("id");
                
                int id = idElement.GetInt32();

                if (Enum.TryParse(id.ToString(), out ModuleEnum moduleEnum))
                {
                    PropertyInfo propertyInfo = typeof(PongHTTPService).GetProperty(moduleEnum.ToString());
                    Type moduleType = propertyInfo.PropertyType;
                    ModuleBase jsonModule = (ModuleBase)JsonSerializer.Deserialize(json, moduleType, options);
                    propertyInfo.SetValue(this, jsonModule);
                }


            }
        }
    }
}