using Newtonsoft.Json;
using PongService.Modules;
using System.Net;


namespace PongService
{
    public class PongService
    {
        private HttpListener _service;

        public TangibleModule TangibleModule { get; set; }
        public DlKilnectModule DlKilnectModule { get; set; }
        public VoiceModule VoiceModule { get; set; }
        public SensorsModule SensorsModule { get; set; }


        public PongService(string url)
        {
            url = "http://localhost:8080/";
            _service = new HttpListener();
            _service.Prefixes.Add(url);
            _service.Start();

        }

        public void ServerListener()
        {
            while (true)
            {
            var context = _service.GetContext();
            ThreadPool.QueueUserWorkItem(ProcessRequest, context);
            }
        }

        private void ProcessRequest(object state)
        {
            var context = (HttpListenerContext)state;
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "GET")
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

        private void ProcessMessage(HttpListenerContext context)
        {
            var headers = context.Request.Headers;

            using (var streamReader = new StreamReader(context.Request.InputStream))
            {
                var json = streamReader.ReadToEnd();
                //var message = JsonConvert.DeserializeObject<MyMessage>(json);
            }
            //var sds = context.Request
        }
    }
}