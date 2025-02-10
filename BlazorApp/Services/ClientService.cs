using BlazorTest.Entities;
using System.Xml.Linq;

namespace BlazorTest.Services
{
    public class ClientService
    {
        private List<Client> _clients = new List<Client>();

        public ClientService()
        {
            // Mock data
            _clients.Add(new Client { Id = 1, Name = "John Doe", Email = "john@example.com", Phone = "123-456-7890" });
            _clients.Add(new Client { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Phone = "987-654-3210" });
        }

        public List<Client> GetClients() => _clients;

        public Client? GetClientById(int id) => _clients.FirstOrDefault(c => c.Id == id);

        public void AddClient(Client client)
        {
            client.Id = _clients.Count > 0 ? _clients.Max(c => c.Id) + 1 : 1;
            _clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            var existingClient = GetClientById(client.Id);
            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.Email = client.Email;
                existingClient.Phone = client.Phone;
            }
        }

        public void DeleteClient(int id)
        {
            var client = GetClientById(id);
            if (client != null)
            {
                _clients.Remove(client);
            }
        }
    }
}
