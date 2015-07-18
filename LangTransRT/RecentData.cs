using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangTransRT
{
	public class RecentData : INotifyPropertyChanged
	{
		private string _source;
		public string Source
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
				if ( PropertyChanged != null )
					PropertyChanged.Invoke ( this, new PropertyChangedEventArgs ( "Source" ) );
			}
		}

		private int _targetIndex;
		public int TargetIndex
		{
			get
			{
				return _targetIndex;
			}
			set
			{
				_targetIndex = value;
				if ( PropertyChanged != null )
					PropertyChanged.Invoke ( this, new PropertyChangedEventArgs ( "TargetIndex" ) );
			}
		}


		private int _sourceIndex;
		public int SourceIndex
		{
			get
			{
				return _sourceIndex;
			}
			set
			{
				_sourceIndex = value;
				if ( PropertyChanged != null )
					PropertyChanged.Invoke ( this, new PropertyChangedEventArgs ( "SourceIndex" ) );
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public override bool Equals ( object obj )
		{
			if ( !( obj is RecentData ) ) return false;

			RecentData another = obj as RecentData;
			if ( another.SourceIndex == SourceIndex && another.TargetIndex == TargetIndex
				&& another.Source == Source ) return true;
			return false;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}
