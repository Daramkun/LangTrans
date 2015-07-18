using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace LangTransRT
{
	public static class DataManager
	{
		public static async void SaveTo ( ObservableCollection<RecentData> datas )
		{
			StorageFile sf = await ApplicationData.Current.LocalFolder.CreateFileAsync ( "data.dat",
				Windows.Storage.CreationCollisionOption.ReplaceExisting );
			FileRandomAccessStream stream = await sf.OpenAsync ( FileAccessMode.ReadWrite ) as FileRandomAccessStream;
			DataWriter dw = new DataWriter ( stream );

			dw.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
			dw.ByteOrder = ByteOrder.LittleEndian;

			dw.WriteInt32 ( datas.Count );
			foreach ( RecentData data in datas )
			{
				dw.WriteUInt32 ( ( uint ) dw.MeasureString ( data.Source ) );
				dw.WriteString ( data.Source );
				dw.WriteInt32 ( data.SourceIndex );
				dw.WriteInt32 ( data.TargetIndex );
			}

			await dw.StoreAsync ();
			await dw.FlushAsync ();
			stream.Dispose ();
		}

		public static async Task<bool> LoadFrom ( ObservableCollection<RecentData> datas )
		{
			try
			{
				StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync ( "data.dat" );
				FileRandomAccessStream stream = await sf.OpenAsync ( FileAccessMode.Read ) as FileRandomAccessStream;
				DataReader dr = new DataReader ( stream );
				dr.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
				dr.ByteOrder = ByteOrder.LittleEndian;

				await dr.LoadAsync ( ( uint ) stream.Size );

				int len = dr.ReadInt32 ();
				for ( int i = 0; i < len; i++ )
				{
					RecentData data = new RecentData ();
					uint srclen = dr.ReadUInt32 ();
					data.Source = dr.ReadString ( srclen );
					data.SourceIndex = dr.ReadInt32 ();
					data.TargetIndex = dr.ReadInt32 ();
					datas.Add ( data );
				}

				stream.Dispose ();
			}
			catch { return false; }
			return true;
		}
	}
}