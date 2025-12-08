using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using OnlyRetroRobloxHere.Launcher.UI.Commands;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class PaginationItemsControl : ItemsControl
{
	private bool _inDesignMode;

	private int _pageIndex;

	private int _pageCount;

	private bool _isEnd;

	private RelayCommand? _previousPageCommand;

	private RelayCommand? _nextPageCommand;

	public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(PaginationItemsControl), new PropertyMetadata(4));

	public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(PaginationItemsControl), new PropertyMetadata(4));

	public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PaginationItemsControl), new PropertyMetadata(Orientation.Horizontal, OnDependencyPropertyChanged));

	public static readonly DependencyProperty MainItemsSourceProperty = DependencyProperty.Register("MainItemsSource", typeof(IEnumerable), typeof(PaginationItemsControl), new PropertyMetadata(null, OnMainItemsSourceChanged));

	private static readonly DependencyPropertyKey AnyPagesPropertyKey = DependencyProperty.RegisterReadOnly("AnyPages", typeof(bool), typeof(PaginationItemsControl), new PropertyMetadata(false));

	public static readonly DependencyProperty AnyPagesProperty = AnyPagesPropertyKey.DependencyProperty;

	private static readonly DependencyPropertyKey PaginationTextPropertyKey = DependencyProperty.RegisterReadOnly("PaginationText", typeof(string), typeof(PaginationItemsControl), new PropertyMetadata(""));

	public static readonly DependencyProperty PaginationTextProperty = PaginationTextPropertyKey.DependencyProperty;

	public static readonly DependencyProperty PaginationTextFormatProperty = DependencyProperty.Register("PaginationTextFormat", typeof(string), typeof(PaginationItemsControl), new PropertyMetadata("", OnPaginationTextFormatChanged));

	public int Columns
	{
		get
		{
			return (int)GetValue(ColumnsProperty);
		}
		set
		{
			SetValue(ColumnsProperty, value);
		}
	}

	public int Rows
	{
		get
		{
			return (int)GetValue(RowsProperty);
		}
		set
		{
			SetValue(RowsProperty, value);
		}
	}

	public Orientation Orientation
	{
		get
		{
			return (Orientation)GetValue(OrientationProperty);
		}
		set
		{
			SetValue(OrientationProperty, value);
		}
	}

	public IEnumerable MainItemsSource
	{
		get
		{
			return (IEnumerable)GetValue(MainItemsSourceProperty);
		}
		set
		{
			SetValue(MainItemsSourceProperty, value);
		}
	}

	public bool AnyPages
	{
		get
		{
			return (bool)GetValue(AnyPagesProperty);
		}
		private set
		{
			SetValue(AnyPagesPropertyKey, value);
		}
	}

	public string PaginationText
	{
		get
		{
			return (string)GetValue(PaginationTextProperty);
		}
		private set
		{
			SetValue(PaginationTextPropertyKey, value);
		}
	}

	public string PaginationTextFormat
	{
		get
		{
			return (string)GetValue(PaginationTextFormatProperty);
		}
		set
		{
			SetValue(PaginationTextFormatProperty, value);
		}
	}

	public ICommand PreviousPageCommand => _previousPageCommand ?? (_previousPageCommand = new RelayCommand(delegate
	{
		_pageIndex--;
		OnDataChanged();
	}, (object? _) => _pageIndex > 0));

	public ICommand NextPageCommand => _nextPageCommand ?? (_nextPageCommand = new RelayCommand(delegate
	{
		_pageIndex++;
		OnDataChanged();
	}, (object? _) => !_isEnd));

	public PaginationItemsControl()
	{
		_inDesignMode = DesignerProperties.GetIsInDesignMode(this);
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(UniformGrid), "UniformGrid");
		Binding binding = new Binding("Columns")
		{
			Mode = BindingMode.OneWay,
			RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, GetType(), 1)
		};
		Binding binding2 = new Binding("Rows")
		{
			Mode = BindingMode.OneWay,
			RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, GetType(), 1)
		};
		frameworkElementFactory.SetBinding(UniformGrid.ColumnsProperty, binding);
		frameworkElementFactory.SetBinding(UniformGrid.RowsProperty, binding2);
		base.ItemsPanel = new ItemsPanelTemplate(frameworkElementFactory);
	}

	public void ResetPageIndex(bool updateItems = true)
	{
		_pageIndex = 0;
		if (updateItems)
		{
			OnDataChanged();
		}
	}

	private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((PaginationItemsControl)d).OnSizeChanged();
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		OnSizeChanged();
		base.OnRenderSizeChanged(sizeInfo);
	}

	private void OnSizeChanged()
	{
	}

	private static void OnMainItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		PaginationItemsControl paginationItemsControl = (PaginationItemsControl)d;
		if (e.OldValue is INotifyCollectionChanged notifyCollectionChanged)
		{
			notifyCollectionChanged.CollectionChanged -= paginationItemsControl.UniformGridItemsControl_CollectionChanged;
		}
		if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged2)
		{
			notifyCollectionChanged2.CollectionChanged += paginationItemsControl.UniformGridItemsControl_CollectionChanged;
		}
		paginationItemsControl.OnDataChanged();
	}

	private void UniformGridItemsControl_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		OnDataChanged();
	}

	private static void OnPaginationTextFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((PaginationItemsControl)d).UpdatePaginationInfo();
	}

	private void UpdatePaginationInfo()
	{
		string paginationText = PaginationTextFormat.Replace("[[[0]]]", (_pageIndex + 1).ToString()).Replace("[[[1]]]", ((_pageCount == 0) ? 1 : _pageCount).ToString());
		PaginationText = paginationText;
		AnyPages = _inDesignMode || _pageCount != 0;
	}

	private void OnDataChanged()
	{
		if (MainItemsSource == null)
		{
			return;
		}
		int num = Columns * Rows;
		IEnumerable<object> enumerable = MainItemsSource.Cast<object>();
		_pageCount = (int)Math.Ceiling((double)enumerable.Count() / (double)num);
		if (_pageCount != 0 && _pageCount - 1 < _pageIndex)
		{
			_pageIndex = _pageCount - 1;
		}
		UpdatePaginationInfo();
		IEnumerable<object> enumerable2 = enumerable.Skip(_pageIndex * num).Take(num);
		_isEnd = enumerable != null && enumerable.Count() <= num * (_pageIndex + 1);
		_previousPageCommand?.RaiseCanExecuteChanged();
		_nextPageCommand?.RaiseCanExecuteChanged();
		if (Orientation == Orientation.Vertical)
		{
			bool flag = false;
			object[] array = enumerable2.ToArray();
			List<object> list = new List<object>();
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					int num2 = i + j * Columns;
					list.Add((num2 < enumerable2.Count()) ? array[j + i] : null);
					if (num2 + 1 == enumerable2.Count())
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			base.ItemsSource = list;
		}
		else
		{
			base.ItemsSource = enumerable2;
		}
	}
}
