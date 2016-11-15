﻿using System;
using System.Collections.Generic;
using System.Windows;
using Tetris.Controls;
using Tetris;
using Tetris.Algorithms;

namespace Tetris.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TileBrowser : Window
    {
        public List<TileControl> TileControls = new List<TileControl>();
        public TileBrowser()
        {
            InitializeComponent();
        }

        public TileBrowser(List<byte[,]> Tiles, /*ref*/ List<Shape> Shapes)
        {
            InitializeComponent();
            Random r = new Random();
            foreach (byte[,] b in Tiles)
            {
                TileControl t = new TileControl(b, ref Shapes, System.Drawing.Color.FromArgb(0, (byte)r.Next(20,235), (byte)r.Next(20,235), (byte)r.Next(20,235)));
                this.TilesPanel.Children.Add(t);
                TileControls.Add(t);
            }
        }


        public TileBrowser(/*ref*/List<Shape> Shapes)
        {
            InitializeComponent();
            Random r = new Random();
            foreach (Shape s in Shapes)
            {
                TileControl t = new TileControl(s);
                this.TilesPanel.Children.Add(t);
                TileControls.Add(t);
            }
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.TilesPanel.Columns = (int)(this.ActualWidth / 230);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.TilesPanel.Columns = (int)(this.ActualWidth / 230);

        }
    }
}
