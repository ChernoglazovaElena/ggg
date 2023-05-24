using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Nodes;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Error = "Заметка с таким именем существует";
        private List<Zametka> AllZametka = new List<Zametka>();
        private string NameItemZametka;
        public MainWindow()
        {
            DSrealize.SearchJsonFile();
            AllZametka = DSrealize.Deserialize<List<Zametka>>();
            InitializeComponent();
            Calendar.Text = Calendar.DisplayDate.ToString();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in AllZametka)
            {
                if (element.Name == NameItemZametka && element.Date == Calendar.Text)
                {
                    AllZametka.Remove(element);
                    DSrealize.Serialize(AllZametka);
                    break;
                }
            }
            Create.IsEnabled = true;
            Delete.IsEnabled = false;
            Save.IsEnabled = false;
            Calendar_SelectedDateChanged(null, null);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in AllZametka)
            {
                if (element.Name == NameItemZametka && element.Date == Calendar.Text)
                {
                    element.Name = EnterName.Text;
                    element.Description = EnterDescription.Text;
                    break;
                }
            }
            DSrealize.Serialize(AllZametka);
            Calendar_SelectedDateChanged(null, null);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EnterName.Text))
            {
                Zametka zametka = new Zametka
                {
                    Name = EnterName.Text,
                    Description = EnterDescription.Text,
                    Date = Calendar.Text
                };
                foreach (var element in AllZametka)
                {
                    if (element.Name == zametka.Name && element.Date == zametka.Date)
                    {
                        TextError.Content = Error;
                        break;
                    }
                }
                AllZametka.Add(zametka);
                EnterName.Clear();
                EnterDescription.Clear();
                DSrealize.Serialize(AllZametka);
                Calendar_SelectedDateChanged(null, null);
            }
        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> NameZametka = new List<string>();
            foreach (var element in AllZametka)
            {
                if (element.Date == Calendar.Text)
                {
                    NameZametka.Add(element.Name);
                }
            }
            ListZametka.ItemsSource = NameZametka;
        }

        private void ListZametka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Create.IsEnabled = false;
            Delete.IsEnabled = true;
            Save.IsEnabled = true;
            try // 40000000 iq костыль
            {
                NameItemZametka = ListZametka.Items[ListZametka.SelectedIndex].ToString();
                EnterName.Text = NameItemZametka;
                foreach (var element in AllZametka)
                {
                    if (element.Name == NameItemZametka && element.Date == Calendar.Text)
                    {
                        EnterDescription.Text = element.Description;
                        break;
                    }
                }
            }
            catch
            {
                Create.IsEnabled = true;
                Delete.IsEnabled = false;
                Save.IsEnabled = false;
                EnterName.Text = null;
                EnterDescription.Text = null;
            }
        }
    }
}
