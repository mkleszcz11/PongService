# PongService
Service that supports communication between Unity pong game and control modules

Each module has its own Id which is used to recognize the sender of the http message:
VoiceModule = 1,
TangibleModule = 2,
SensorsModule = 3,
DlKinectmodule = 4,

An example that sends an http request (app will be working in local network so url will have "5001" or "8080" port):

    Console.WriteLine("Sending messages...");
    var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5001/");

    while (true)
    {
        Console.WriteLine("Enter message: ");
        var message = Console.ReadLine();

        // Create a JSON payload
        var payload = new
        {
           id = 1,
           message = message
        };
        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("", content);

        Console.WriteLine($"Response: {response.StatusCode}");

    }

Expected json body for each module:

TangibleModule:
    {
        "object": string,
        "coordinateX": int,
        "coordinateY": int,
        "rotationAngle": int
    }

SensorsModule:
    {
        "ballVelocity": int,
        "wallColor": string
    }
   
VoiceModule:
    {
        "racketDirection": int,
        "velocity": int
    }
    
DlKinectModule:
    {
        "racketDirection": int,
        "velocity": int
    }
    
To test if communication works fine you need to download .zip, then open ConsoleService/Program.cs in any text editor and change the line. Now you need to build it. Program is ready to use after opening .exe file in bin/Debug/net6.0.
Console.WriteLine(PongHTTPService.GetInstance().[YourOwnModule].[ModuleProperty]);
    ![image](https://user-images.githubusercontent.com/93325616/231897822-74648839-c161-4f96-8f83-b9923c8fde16.png)
