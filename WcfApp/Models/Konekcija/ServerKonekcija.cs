using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Models.Konekcije
{
    [ExcludeFromCodeCoverage]

    public class ServerKonekcija<T> : IKonekcija<T>
    {
        List<Uri> Uris = new List<Uri>();
        ServiceHost Host = null;
        NetTcpBinding Binding = null;
        
        public ServerKonekcija() : base() { }
        public override T Service { get; set; }

        public ServerKonekcija(string[] uris, T service) : base()
        {
            try
            {
                foreach (var uri in uris)
                {
                    Uris.Add(new Uri(uri));
                }

                Service = service;
                Host = new ServiceHost(Service, Uris.ToArray());
                Binding = new NetTcpBinding(SecurityMode.None);
                Host.AddServiceEndpoint(typeof(T), Binding, "");
                Host.Opened += Host_Opened;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while creating server:{e.Message}");
            }
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Server started.");
        }

        public bool IsOpen()
        {
            return Host.State == CommunicationState.Opened;
        }

        public void Open()
        {
            try
            {
                Host.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while opening server:{e.Message}");
            }
        }
    }
}

