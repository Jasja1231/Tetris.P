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

namespace Tetris.Controls
{
    /// <summary>
    /// Interaction logic for NumberChooser.xaml
    /// </summary>
    public partial class NumberSetter : UserControl 
    {
        private int selectedvalue;

        public int MinValue { get; set; }
        public int SelectedValue { get { return selectedvalue; } set { selectedvalue = value; this.ValueBlock.Text = selectedvalue.ToString(); } }

        public NumberSetter()
        {
            InitializeComponent();
            MinValue = 1;
        }

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            SelectedValue++;
            this.ValueBlock.Text = SelectedValue.ToString();

        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {
            SelectedValue--;
            if (SelectedValue <MinValue)
                SelectedValue = MinValue;
            this.ValueBlock.Text = SelectedValue.ToString();
        }

    }
}
