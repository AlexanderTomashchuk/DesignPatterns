using System;
using static System.Console;

namespace FluentAPI
{
    //"fake"
    interface IFakeDBConnection
    {
        void Open();
    } 
    
    class SqlConnection : IFakeDBConnection
    {
        private readonly string _connectionString;

        public SqlConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open()
        {
            WriteLine($"ConnectionString: {_connectionString}");
            WriteLine("Connecting to DB...");
        }
    }
    
    class FluentSqlConnection :
        IServerSelectionStage,
        IDatabaseSelectionStage,
        IPortSelectionStage, 
        IUserSelectionStage, 
        IPasswordSelectionStage, 
        IConnectionInitializerStage
    {
        private string _server;
        private string _database;
        private int _port;
        private string _username;
        private string _password;
        
        private FluentSqlConnection()
        {
        }

        public static IServerSelectionStage CreateConnection(Action<ConnectionConfiguration> configure = null)
        {
            var connectionConfiguration = new ConnectionConfiguration();
            configure?.Invoke(connectionConfiguration);
            return new FluentSqlConnection();
        }

        public IDatabaseSelectionStage ForServer(string server)
        {
            _server = server;
            return this;
        }

        public IPortSelectionStage AndDatabase(string database)
        {
            _database = database;
            return this;
        }

        public IUserSelectionStage OnPort(int port)
        {
            _port = port;
            return this;
        }

        public IPasswordSelectionStage AsUser(string username)
        {
            _username = username;
            return this;
        }

        public IConnectionInitializerStage WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public IFakeDBConnection Connect()
        {
            var connection = new SqlConnection($"Server={_server};Database={_database};Port={_port};User Id={_username};Password={_password};");
            connection.Open();
            return connection;
        }
    }

    interface IServerSelectionStage
    {
        IDatabaseSelectionStage ForServer(string server);
    }

    interface IDatabaseSelectionStage
    {
        IPortSelectionStage AndDatabase(string database);
    }

    interface IPortSelectionStage
    {
        IUserSelectionStage OnPort(int port);
    }

    interface IUserSelectionStage
    {
        IPasswordSelectionStage AsUser(string username);
    }

    interface IPasswordSelectionStage
    {
        IConnectionInitializerStage WithPassword(string password);
    }

    interface IConnectionInitializerStage
    {
        IFakeDBConnection Connect();
    }

    class ConnectionConfiguration
    {
        public string ConnectionName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var connection = FluentSqlConnection
                .CreateConnection(config =>
                {
                    config.ConnectionName = "Alex's connection";
                })
                .ForServer("localhost")
                .AndDatabase("mydb")
                .OnPort(5432)
                .AsUser("alex")
                .WithPassword("Password")
                .Connect();
            
            ReadKey();
        }
    }
}