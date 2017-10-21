﻿using System;
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
using MahApps.Metro.Controls;
using System.Management;

namespace WALLnutClient
{
    /// <summary>
    /// Interaction logic for BlackWindow.xaml
    /// </summary>
    public partial class SelectDiskWindow : MetroWindow
    {
        public SelectDiskWindow()
        {
            InitializeComponent();
            UpdateDriveList();
            img_refresh.Source = new BitmapImage(new Uri(Properties.Resources.RESOURCES_PATH + "refresh.png", UriKind.RelativeOrAbsolute));
        }

        #region [Function] 버튼 버튼 핸들러
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            DiskInfo info = null;
            if (cb_disk.SelectedIndex.Equals(-1))
            {
                MessageBox.Show("진행할 디스크를 선택해주세요!", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            info = (DiskInfo)cb_disk.Items[cb_disk.SelectedIndex];
            if (info.isWALLNutDevice)
            {
                MainWindow window = new MainWindow(info);
                window.Show();
                this.Close();
            }
            else
            {
                if (MessageBoxResult.OK.Equals(MessageBox.Show(
                "정말로 포맷하시겠습니까? 디스크 내 모든 데이터가 초기화됩니다!",
                "주의",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning)))
                {
                    if (DiskManager.FormatDisk((DiskInfo)cb_disk.SelectedItem))
                    {
                        MessageBox.Show("포맷 성공", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateDriveList();
                        MainWindow window = new MainWindow(info);
                        window.Show();
                        this.Close();
                    }
                }
            }
        }
        #endregion

        #region [Function] Drive 목록 업데이트
        public void UpdateDriveList()
        {
            List<DiskInfo> list = DiskInfo.GetDriveList();
            cb_disk.Items.Clear();
            foreach (DiskInfo info in list)
            {
                cb_disk.Items.Add(info);
            }
            cb_disk.SelectedIndex = 0;
        }
        #endregion

        private void cb_disk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_disk.SelectedIndex.Equals(-1))
            {
                DiskInfo info = (DiskInfo)cb_disk.Items[cb_disk.SelectedIndex];
                if (!info.isWALLNutDevice)
                {
                    btn_ok.Content = "Format";
                }
                else
                {
                    btn_ok.Content = "Go";
                }
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateDriveList();
        }
    }
}
