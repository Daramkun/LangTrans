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

namespace LangTrans
{
	public partial class RecentItem : UserControl
	{
		private static string [] items = {
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

		RecentRecord rr;

		public new object Content
		{
			get { return rr; }
			set
			{
				rr = value as RecentRecord;
				txtText.Text = rr.Text;
				txtLang.Text = String.Format ( "{0} -> {1}", 
					items [ rr.SourceIndex ], items [ rr.DestinationIndex ] );
			}
		}

		public RecentItem (RecentRecord rr)
		{
			InitializeComponent ();
			Content = rr;
		}
	}
}
