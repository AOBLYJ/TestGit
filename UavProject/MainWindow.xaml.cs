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
using System.Windows.Threading;

namespace UavProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer m_timer;
        private int m_up_id_sel = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            for(int i=0; i<UavViewModel.UP_SIZE; i++)
            {
                Label lbl = new Label();
                up_grid.Children.Add(lbl);
            }

            for(int i = 0; i < UavViewModel.DN_SIZE; i++)
            {
                Label lbl = new Label();
                down_grid.Children.Add(lbl);
            }


            m_timer = new DispatcherTimer();
            m_timer.Interval = TimeSpan.FromMilliseconds(200);
            m_timer.Tick += OnTimerTick;
            m_timer.Start();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LinkManager.Mgr.Exit();
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            byte[] ar = UavViewModel.UavModel.GetUp(m_up_id_sel); //msg_id로 데이터를 가져오는거네..수정필요없음.
            
            for (int i = 0; i < up_grid.Children.Count; i++)
            {
                Label lbl = up_grid.Children[i] as Label;
                if (ar != null && i < ar.Length) lbl.Content = string.Format("{0:X}", ar[i]);
                else
                {
                    if (string.IsNullOrEmpty( lbl.Content as string)) break;
                    lbl.Content = string.Empty;
                }
            }
          
            byte[] dn = UavViewModel.UavModel.GetDown();
            for (int i = 0; i < down_grid.Children.Count; i++)
            {
                Label lbl = down_grid.Children[i] as Label;
                if (dn != null && i < dn.Length) lbl.Content = string.Format("{0:X}", dn[i]);
                else
                {
                    if (string.IsNullOrEmpty(lbl.Content as string)) break;
                    lbl.Content = string.Empty;
                }
            }

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LinkManager.Mgr.Initialize();
        }
        private void UpComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ComboBoxItem item = e.AddedItems[e.AddedItems.Count - 1] as ComboBoxItem;
                if (item == null || item.Tag == null) return;
                string text = item.Tag as string;
                if (!int.TryParse(text, out m_up_id_sel)) //TryParse가 true가 되면 Tag에 따라서 m_up_id_sel(즉 msg_id)이 바껴
                    m_up_id_sel = -1; //False
            }
        }

        public void OnCheck(object sender, RoutedEventArgs e)
        {
            UavViewModel.UavModel.AutoCheck = true;
        }
        public void OffCheck(object sender, RoutedEventArgs e)
        {
            UavViewModel.UavModel.AutoCheck = false;
        }    
    }
     
}
