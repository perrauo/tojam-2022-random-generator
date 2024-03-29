﻿using System.Collections.Generic;
using System.ComponentModel;
namespace Cirrus.Collections
{
	internal class PropertySyncNotifier
	{
		public System.Collections.Generic.List<PropertyChangedEventArgs> LastArgs = new System.Collections.Generic.List<PropertyChangedEventArgs>();
		public INotifyPropertySyncChanged SourceSync { get; set; }
		public INotifyPropertySyncChanged DestSync { get; set; }
		public INotifyPropertyChanged SourceNotify { get; set; }
		public INotifyPropertyChanged DestNotify { get; set; }
		public PropertySyncNotifier(INotifyPropertyChanged sourceItem, INotifyPropertyChanged destItem, bool sourceToDest = true, bool destToSource = true)
		{
			SourceNotify = sourceItem;
			DestNotify = destItem;
			SourceSync = sourceItem as INotifyPropertySyncChanged;
			DestSync = destItem as INotifyPropertySyncChanged;
			if(sourceItem == null && destItem == null) throw new System.ArgumentException("One of the objects must implement INotifyPropertySyncChanged."); ;
			if(DestSync != null && sourceToDest) SourceNotify.PropertyChanged += DestTrigger;
			if(SourceSync != null && destToSource) DestNotify.PropertyChanged += SourceTrigger;
		}
		public void DestTrigger(object sender, PropertyChangedEventArgs args)
		{
			if(LastArgs.Contains(args)) { LastArgs.Remove(args); return; }
			LastArgs.Add(args);
			DestSync.OnPropertyChanged(sender, args);
		}
		public void SourceTrigger(object sender, PropertyChangedEventArgs args)
		{
			if(LastArgs.Contains(args)) { LastArgs.Remove(args); return; }
			LastArgs.Add(args);
			SourceSync.OnPropertyChanged(sender, args);
		}
	}
}
