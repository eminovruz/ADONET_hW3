using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data;
using System.Linq;

namespace ADONET_hW3
{
    public partial class MainWindow : Window
    {
        DbConnection? connection = null;
        DbDataAdapter? adapter = null;
        DbProviderFactory? providerFactory = null;
        IConfigurationRoot? configuration = null;
        DataSet? dataSet = null;
        public MainWindow()
        {
            InitializeComponent();

            DbProviderFactories.RegisterFactory("library", typeof(SqlClientFactory));
            var providerName = "library";
            providerFactory = DbProviderFactories.GetFactory(providerName);
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = configuration.GetConnectionString(providerName);
            adapter = providerFactory.CreateDataAdapter();

            DataTableMapping[] mappings = {new DataTableMapping("Table","SET1"),
            new DataTableMapping("Table1","SET2"),
        new DataTableMapping("Table2","SET3")};


            adapter.TableMappings.AddRange(mappings);
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var command = providerFactory?.CreateCommand();
            dataSet = new DataSet();


            command.CommandText = txtCommand.Text;
            command.Connection = connection;
            adapter.SelectCommand = command;
            dataSet.Tables.Clear();

            adapter.Fill(dataSet);


            foreach (DataTable table in dataSet.Tables)
            {
                var tab = new TabItem();
                tab.Header = table.TableName;

                var dataGrid = new DataGrid();

                dataGrid.ItemsSource = table.AsDataView();

                tab.Content = dataGrid;

                Tabcontrol1.Items.Add(tab);
            }

        }
    }
}