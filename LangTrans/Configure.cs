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
using System.IO;

namespace LangTrans
{
	public static class Configure
	{
		public static int [] ConfigureText
		{
			get
			{
				int [] config = new int [ 2 ];

				try
				{
					using ( IsolatedStorageFileStream fs = new IsolatedStorageFileStream (
						"langtransConfig.config", System.IO.FileMode.Open,
						IsolatedStorageFile.GetUserStoreForApplication () ) )
					{
						BinaryReader br = new BinaryReader ( fs );
						config [ 0 ] = br.ReadInt32 ();
						config [ 1 ] = br.ReadInt32 ();
					}
				}
				catch { return null; }

				return config;
			}

			set
			{
				using ( IsolatedStorageFileStream fs = new IsolatedStorageFileStream (
					"langtransConfig.config", System.IO.FileMode.Create,
					IsolatedStorageFile.GetUserStoreForApplication () ) )
				{
					BinaryWriter bw = new BinaryWriter ( fs );
					bw.Write ( value [ 0 ] );
					bw.Write ( value [ 1 ] );
				}
			}
		}
	}
}