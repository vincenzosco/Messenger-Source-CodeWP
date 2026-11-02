using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Facebook.Models
{
	[DataContract]
	public class Notifiable : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public bool Set<T>(ref T storage, T value, [CallerMemberName] string name = null) where T : IComparable
		{
			if (storage != null && storage.CompareTo(value) == 0)
			{
				return false;
			}
			storage = value;
			OnPropertyChanged(name);
			return true;
		}

		public void SetWithoutComparing<T>(ref T storage, T value, [CallerMemberName] string name = null)
		{
			storage = value;
			OnPropertyChanged(name);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
