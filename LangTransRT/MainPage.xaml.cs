using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LangTransRT
{
	public sealed partial class MainPage : LangTransRT.Common.LayoutAwarePage
	{
		static MainPage itsMe;
		public static MainPage Current { get { return itsMe; } }

		internal static string [] langs = { "갈라시아어", "구자라트어", "그루지야어", "그리스어", "네덜란드어",
							  "노르웨이어", "덴마크어", "독일어", "라트비아어", "라틴어", "러시아어",
							  "루마니아어", "리투아니아어", "마케도니아어", "말레이어", "몰타어",
							  "바스크어", "베트남어", "벨로루시어", "벵골어", "불가리아어", 
							  "세르비아어", "스와힐리어", "스웨덴어", "스페인어", "슬로바키아어",
							  "슬로베니아어", "아랍어", "아르메니아어", "아이슬란드어",
							  "아이티프랑스어", "아일랜드어", "아제르바이잔어", "아프리칸스어",
							  "알바니아어", "에스토니아어", "에스페란토어", "영어", "우르드어",
							  "우크라이나어", "웨일즈어", "이디시어", "이탈리아어", "인도네시아어",
							  "일본어", "중국어", "체코어", "카탈로니아어", "칸다나어", "크로아티아어",
							  "타갈로그어", "타밀어", "태국어", "터키어", "텔루구어", "페르시아어",
							  "포르투갈어", "폴란드어", "프랑스어", "핀란드어", "한국어", "헝가리어",
							  "히브리어", "힌디어" };

		ObservableCollection<RecentData> recentList = new ObservableCollection<RecentData> ();

		public MainPage ()
		{
			itsMe = this;
			this.InitializeComponent ();
			SetComboBoxItems ();

			lstRecent.ItemsSource = recentList;
		}

		private void SetComboBoxItems ()
		{
			cmbSource.ItemsSource = langs;
			cmbDest.ItemsSource = langs;
			cmbSource.SelectedItem = "한국어";
			cmbDest.SelectedItem = "영어";
		}

		private void btnExchange_Click ( object sender, RoutedEventArgs e )
		{
			int temp = cmbSource.SelectedIndex;
			cmbSource.SelectedIndex = cmbDest.SelectedIndex;
			cmbDest.SelectedIndex = temp;
		}

		internal async void Retrans ()
		{
			await Dispatcher.RunAsync ( Windows.UI.Core.CoreDispatcherPriority.Normal,
				() =>
				{
					txtDest.Text = Translator.Translated;
					prgProcess.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
					btnTranslate.IsEnabled = true;
				} );
		}

		private void btnTranslate_Click ( object sender, RoutedEventArgs e )
		{
			btnTranslate.IsEnabled = false;
			prgProcess.Visibility = Windows.UI.Xaml.Visibility.Visible;
			Translator.TranslateEnded += () =>
			{
				RecentData recentData = new RecentData ();
				recentData.Source = txtSource.Text;
				recentData.SourceIndex = cmbSource.SelectedIndex;
				recentData.TargetIndex = cmbDest.SelectedIndex;

				var ee = from b in recentList where b.Equals ( recentData ) select b;
				bool bb = false;
				foreach ( var a in ee )
					bb = true;

				if ( !bb )
				{
					recentList.Add ( recentData );
					DataManager.SaveTo ( recentList );
				}

				Retrans ();
			};
			try
			{
				Translator.Translate ( ( LangTransRT.Language ) ( cmbSource.SelectedIndex + 1 ),
					( LangTransRT.Language ) ( cmbDest.SelectedIndex + 1 ), txtSource.Text );
			}
			catch
			{
				Task<MessageBoxResult> mr = MessageBox.ShowAsync ( "번역에 실패했습니다!",
					"랑트랜스", MessageBoxButton.OK );
				Retrans ();
			}
		}

		private async void abb_Remove_Click ( object sender, RoutedEventArgs e )
		{
			if ( lstRecent.SelectedItems.Count <= 0 )
			{
				await MessageBox.ShowAsync ( "삭제할 항목을 먼저 선택해주세요.", "랑트랜스", MessageBoxButton.OK );
				return;
			}
			foreach ( var item in lstRecent.SelectedItems )
			{
				recentList.Remove ( item as RecentData );
			}
			DataManager.SaveTo ( recentList );
		}

		private async void pageRoot_Loaded ( object sender, RoutedEventArgs e )
		{
			bool loadedRecent = true;
			if ( await DataManager.LoadFrom ( recentList ) == false )
			{
				loadedRecent = false;
			}

			if ( !loadedRecent )
			{
				string pdm = await FileIO.ReadTextAsync ( await Package.Current.InstalledLocation.GetFileAsync ( "Data\\PersonalDataManagement.txt" ) );
				await MessageBox.ShowAsync ( pdm, "랑트랜스 개인정보 취급방침(Privacy policy)", MessageBoxButton.OK );
				DataManager.SaveTo ( recentList );
			}
		}
		
		private void lstRecent_Tapped ( object sender, TappedRoutedEventArgs e )
		{
			if ( lstRecent.SelectedIndex < 0 ) return;
			RecentData data = recentList [ lstRecent.SelectedIndex ];
			txtSource.Text = data.Source;
			cmbSource.SelectedIndex = data.SourceIndex;
			cmbDest.SelectedIndex = data.TargetIndex;
			txtDest.Text = "";
			lstRecent.SelectedIndex = -1;
		}

		private void lstRecent_ItemClick ( object sender, ItemClickEventArgs e )
		{
			/*
			if ( lstRecent.SelectedIndex < 0 ) return;
			RecentData data = recentList [ lstRecent.SelectedIndex ];
			txtSource.Text = data.Source;
			cmbSource.SelectedIndex = data.SourceIndex;
			cmbDest.SelectedIndex = data.TargetIndex;
			txtDest.Text = "";
			lstRecent.SelectedIndex = -1;
			 * */
		}

		private void lstRecent_SelectionChanged ( object sender, SelectionChangedEventArgs e )
		{
			BottomAppBar.IsSticky = true;
			BottomAppBar.IsOpen = ( lstRecent.SelectedItems.Count > 0 );
		}
	}
}
