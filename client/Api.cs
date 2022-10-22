using System;
using System.Text;
using System.Text.Json;
using System.Net.Sockets;
using System.Diagnostics;

public static class Api
{
    private static Process process { get; set; }
    private static int     port    { get; set; }

    static Api()
    {
        string path = Environment.GetEnvironmentVariable("SERVER_PATH");

        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path, UseShellExecute = false,
                RedirectStandardOutput = true, CreateNoWindow = true
            }
        };
        process.Start();

        string stringPort = process.StandardOutput.ReadLine();
        port = int.Parse(stringPort);
    }

    public static string Hello(string message)
    {
        RpcRequest req = new RpcRequest {
            Method = "API.Hello",
            Params = new [] {"Hello World!"},
            Id = 0,
        };

        string serialized = JsonSerializer.Serialize(req);

        using var client = new TcpClient();
        client.Connect("localhost", port);

        using NetworkStream networkStream = client.GetStream();
        // networkStream.ReadTimeout = 2000;

        using var reader = new StreamReader(networkStream, Encoding.UTF8);

        byte[] data = Encoding.UTF8.GetBytes(serialized);
        networkStream.Write(data, 0, data.Length);

        data = new byte[256];
        int bytes = networkStream.Read(data, 0, data.Length);
        string stringResponse = Encoding.ASCII.GetString(data, 0, bytes);

        RpcResponse res = JsonSerializer.Deserialize<RpcResponse>(stringResponse);
        return res.Result;
    }
}
