using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.IO;

namespace LangTrans
{
	public class RecentRecord
	{
		string text;
		int sourceIndex, destIndex;

		public string Text { get { return text; } set { text = value; } }
		public int SourceIndex { get { return sourceIndex; } set { sourceIndex = value; } }
		public int DestinationIndex { get { return destIndex; } set { destIndex = value; } }
	}

	public static class RecentManager
	{
		static List<RecentRecord> records;

		public static RecentRecord [] Records
		{
			get
			{
				if ( records == null )
					LoadFromFile ();
				return records.ToArray ();
			}
		}

		public static void LoadFromFile ()
		{
			records = new List<RecentRecord> ();
			try
			{
				using ( IsolatedStorageFileStream fs = new IsolatedStorageFileStream (
					"recentTranslated.recent", System.IO.FileMode.Open,
					IsolatedStorageFile.GetUserStoreForApplication () ) )
				{
					BinaryReader br = new BinaryReader ( fs );
					int len = br.ReadInt32 ();
					for ( int i = 0; i < len; i++ )
					{
						RecentRecord rr = new RecentRecord ();
						rr.Text = br.ReadString ();
						rr.SourceIndex = br.ReadInt32 ();
						rr.DestinationIndex = br.ReadInt32 ();
						records.Add ( rr );
					}
				}
			}
			catch { }
		}

		public static void SaveToFile ()
		{
			using ( IsolatedStorageFileStream fs = new IsolatedStorageFileStream (
				"recentTranslated.recent", System.IO.FileMode.Create,
				IsolatedStorageFile.GetUserStoreForApplication () ) )
			{
				BinaryWriter bw = new BinaryWriter ( fs );
				bw.Write ( records.Count );
				foreach ( RecentRecord rr in records )
				{
					bw.Write ( rr.Text );
					bw.Write ( rr.SourceIndex );
					bw.Write ( rr.DestinationIndex );
				}
			}
		}

		public static void AddRecord ( RecentRecord rr )
		{
			if ( records == null ) LoadFromFile ();
			records.Add ( rr );
		}

		public static void AddRecord ( string text, int source, int dest )
		{
			RecentRecord rr = new RecentRecord ();
			rr.Text = text;
			rr.SourceIndex = source;
			rr.DestinationIndex = dest;
			AddRecord ( rr );
		}

		public static void RemoveRecord ( int index )
		{
			records.RemoveAt ( index );
		}

		public static void RemoveRecord ( RecentRecord rr )
		{
			records.Remove ( rr );
		}
	}
}
