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
using System.Windows.Shapes;
using Tetris.Controls;


namespace Tetris.Windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public int BoxDensityWeight { get; set; }
        public int YPositionWeight { get; set; }
        public int NeighborWeight { get; set; }


        public OptionsWindow(int YPositionWeight, int BoxDensityWeight, int NeighborWeight)
        {
            InitializeComponent();
            this.YSetter.MinValue = 0;
            this.DensSetter.MinValue = 0;
            this.NeighSetter.MinValue = 0;
            this.YSetter.SelectedValue = YPositionWeight;
            this.NeighSetter.SelectedValue = NeighborWeight;
            this.DensSetter.SelectedValue = BoxDensityWeight;
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            this.YPositionWeight = this.YSetter.SelectedValue;
            this.BoxDensityWeight = this.DensSetter.SelectedValue;
            this.NeighborWeight = this.NeighSetter.SelectedValue;
            this.DialogResult = true;
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
