using System;
using System.Text;
using NATS.Client;

namespace NATS.Handlers;
public class NATSSubscriber
{
    private static string natsUrl;
    private static IConnection c;
    private static IAsyncSubscription subscriber;

    public NATSSubscriber()
    {
        natsUrl = "nats://127.0.0.1:4222";
        
    }

    public NATSSubscriber(string ipv4)
    {
        natsUrl = "nats://" + ipv4 + ":4222";
        
    }
    public static void CreateSubscriber()
    {

        // Creating new connection factory
        Options opts = ConnectionFactory.GetDefaultOptions();
        opts.Url = natsUrl;

        // Creating the connection to the NATS Server
        ConnectionFactory cf = new();

        // Checking connection to the publisher
        try
        {
            c = cf.CreateConnection(opts);
            Console.WriteLine("Successfully Connected to: " + natsUrl);

        }
        catch (Exception)
        {
            Console.WriteLine("Connection Failed: Closing...");
            System.Environment.Exit(1);
        }
    }

    public void StartSubscribing(string vehicle_name, Action<string> messageCallback) 
    {
        // Creating the message handler to recieve messages
            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                Msg m = args.Message;
                string text = Encoding.UTF8.GetString(m.Data);
                messageCallback?.Invoke(text); // Invoke the callback with the received message
            };

            // Creating the Subscriber
            subscriber = c.SubscribeAsync(vehicle_name, handler);

            // Start listening
            while (true)
            {
                string prompt = Console.ReadLine();
                if (prompt == "exit")
                {
                    break;
                }
                
            }
    }

    public void StopConsuming() 
    {
        subscriber.Unsubscribe();
    }
}