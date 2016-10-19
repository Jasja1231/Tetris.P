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

namespace AlgorithmsGUI.Controls
{
    /// <summary>
    /// Interaction logic for NumberChooser.xaml
    /// </summary>
    public partial class NumberSetter : UserControl
    {
        private int selectedvalue;
        public int SelectedValue { get { return selectedvalue; } set { selectedvalue = value; this.ValueBlock.Text = selectedvalue.ToString(); } }

        public NumberSetter()
        {
            InitializeComponent();
        }

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            SelectedValue++;
            this.ValueBlock.Text = SelectedValue.ToString();

        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {
            SelectedValue--;
            if (SelectedValue < 0)
                SelectedValue = 0;
            this.ValueBlock.Text = SelectedValue.ToString();
        }

    }
}
