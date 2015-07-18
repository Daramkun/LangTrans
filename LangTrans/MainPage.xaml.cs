using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Marketplace;

namespace LangTrans
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage ()
		{
			Translator.TranslateEnded += TranslateEnded;

			InitializeComponent ();

			if ( new LicenseInformation ().IsTrial () )
			{
				( ApplicationBar.Buttons [ 1 ] as ApplicationBarIconButton ).IsEnabled = false;
				//btnSaveTranslated.IsEnabled = false;
			}

			#region Initialize ListPickers
			AddToListPicker ( lstSource );
			AddToListPicker ( lstDest );
			#endregion

			#region Configure Load
			int [] temp = Configure.ConfigureText;
			if ( temp != null )
			{
				lstSource.SelectedIndex = temp [ 0 ];
				lstDest.SelectedIndex = temp [ 1 ];
			}
			else
			{
				lstSource.SelectedIndex = 0x3C;
				lstDest.SelectedIndex = 0x25;
			}
			#endregion

			#region Recent translated
			RecentRecord [] recentRecords = RecentManager.Records;
			foreach ( RecentRecord rr in recentRecords )
			{
				AddRecentItem ( rr );
			}
			#endregion
		}

		private void TranslateEnded ()
		{
			Dispatcher.BeginInvoke ( () =>
			{
				txtTranslated.Text = Translator.Translated;
				SystemTray.ProgressIndicator.IsVisible = false;
			} );
		}

		private void AddToListPicker ( ListPicker listPicker )
		{
			listPicker.ItemsSource = items;
			listPicker.FullModeHeader = "언어";
			listPicker.ExpansionMode = ExpansionMode.FullScreenOnly;
		}

		#region ItemResources
		private string [] items = {
									"갈라시아어", "구자라트어", "그루지야어",
									"그리스어", "네덜란드어", "노르웨이어", "덴마크어",
									"독일어", "라트비아어", "라틴어", "러시아어",
									"루마니아어", "리투아니아어", "마케도니아어",
									"말레이어", "몰타어", "바스크어", "베트남어",
									"벨로루시어", "벵골어", "불가리아어", "세르비아어",
									"스와힐리어", "스웨덴어", "스페인어", "슬로바키아어",
									"슬로베니아어", "아랍어", "아르메니아어",
									"아이슬란드어", "아이티프랑스어", "아일랜드어",
									"아제르바이잔어", "아프리칸스어", "알바니아어",
									"에스토니아어", "에스페란토어", "영어", "우르두어",
									"우크라이나어", "웨일즈어", "이디시어", "이탈리아어",
									"인도네시아어", "일본어", "중국어", "체코어",
									"카탈로니아어", "칸다나어", "크로아티아어",
									"타갈로그어", "타밀어", "태국어", "터키어",
									"텔루구어", "페르시아어", "포르투갈어",
									"폴란드어", "프랑스어", "핀란드어", "한국어",
									"헝가리어", "히브리어", "힌디어",
								};
		private string [] itemsCode = {
									"gl", "gu", "ka", "el", "nl", "no", "da",
									"de", "lv", "la", "ru", "ro", "lt", "mk",
									"ms", "mt", "eu", "vi", "be", "bn", "bg",
									"sr", "sw", "sv", "es", "sk", "sl", "ar",
									"hy", "is", "ht", "ga", "az", "af", "sq",
									"et", "eo", "en", "ur", "uk", "cy", "yi",
									"it", "id", "ja", "zh-CN", "cs", "ca",
									"kn", "hr", "tl", "ta", "th", "tr", "te",
									"fa", "pt", "pl", "fr", "fi", "ko", "hu",
									"iw", "hl",
								};
		#endregion

		private void btnTranslate_Click ( object sender, EventArgs e )
		{
			if ( !NetworkInterface.GetIsNetworkAvailable () )
			{
				MessageBox.Show ( "인터넷이 연결되지 않았습니다.\n연결되지 않으면 사용할 수 없습니다." );
				return;
			}
			if ( txtText.Text.Trim ().Length == 0 )
			{
				MessageBox.Show ( "번역할 문장을 작성해주세요." );
				return;
			}

			SystemTray.ProgressIndicator.IsVisible = true;

			Translator.Translate ( Translator.GetLanguageFromCode ( itemsCode [ lstSource.SelectedIndex ] ),
				Translator.GetLanguageFromCode ( itemsCode [ lstDest.SelectedIndex ] ), txtText.Text );

			this.Focus ();
		}

		private void btnSwap_Click ( object sender, RoutedEventArgs e )
		{
			int temp = lstSource.SelectedIndex;
			lstSource.SelectedIndex = lstDest.SelectedIndex;
			lstDest.SelectedIndex = temp;
		}

		protected override void OnBackKeyPress ( System.ComponentModel.CancelEventArgs e )
		{
			int [] temp = { lstSource.SelectedIndex, lstDest.SelectedIndex };
			Configure.ConfigureText = temp;
			base.OnBackKeyPress ( e );
		}

		private void AddRecentItem ( RecentRecord rr )
		{
			RecentItem ri = new RecentItem ( rr );
			ri.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			ri.SetValue ( TiltEffect.IsTiltEnabledProperty, true );
			ri.Tap += ( object ss, System.Windows.Input.GestureEventArgs ee ) =>
				{
					txtText.Text = rr.Text;
					lstSource.SelectedIndex = rr.SourceIndex;
					lstDest.SelectedIndex = rr.DestinationIndex;
					pivotPage.SelectedIndex = 0;
					btnTranslate_Click ( btnTranslate, null );
				};

			ContextMenu cm = new ContextMenu ();
			cm.Tag = ri;
			cm.Items.Add ( new MenuItem () { Header = "삭제", Tag = ri } );
			cm.Tap += ( object ss, System.Windows.Input.GestureEventArgs ee ) =>
				{
					lstRecent.Items.Remove ( ( ss as ContextMenu ).Tag );
					RecentManager.RemoveRecord ( ( ( ss as ContextMenu ).Tag as RecentItem ).Content as RecentRecord );
					RecentManager.SaveToFile ();
				};
			ContextMenuService.SetContextMenu ( ri, cm );
			lstRecent.Items.Add ( ri );
		}

		private void pivotPage_SelectionChanged ( object sender, SelectionChangedEventArgs e )
		{
			switch ( pivotPage.SelectedIndex )
			{
				case 0:
					ApplicationBar.IsVisible = true;
					break;
				case 1:
					ApplicationBar.IsVisible = false;
					break;
			}
			this.Focus ();
		}

		private void btnSaveTranslated_Click ( object sender, EventArgs e )
		{
			RecentRecord rr = new RecentRecord ();
			rr.Text = txtText.Text;
			rr.SourceIndex = lstSource.SelectedIndex;
			rr.DestinationIndex = lstDest.SelectedIndex;

			RecentManager.AddRecord ( rr );
			AddRecentItem ( rr );

			RecentManager.SaveToFile ();
		}
	}
}