using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Enums;
using OnlyRetroRobloxHere.Launcher.Models;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages.SettingsPages;

public partial class TTSSettingsPage : BasePage, IComponentConnector
{
	private const string FakeTTSInfoHelperResponse = "";

	private TTSSettingsViewModel _viewModel;

	private TTSVoices? _ttsVoices;

	private TTSEngine? _currentTTSEngine;

	public TTSSettingsPage()
	{
		InitializeComponent();
		_viewModel = new TTSSettingsViewModel();
		GetAvailableTTSVoices();
		base.DataContext = _viewModel;
	}

	private string GetTTSInfoHelperOutput()
	{
		if (!string.IsNullOrEmpty(""))
		{
			return "";
		}
		using Process process = new Process();
		process.StartInfo.FileName = "OnlyRetroRobloxHere.TTSInfoHelper.exe";
		process.StartInfo.WorkingDirectory = PathHelper.Base;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.Start();
		string result = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		return result;
	}

	private void GetAvailableTTSVoices()
	{
		try
		{
			TTSVoices tTSVoices = JsonSerializer.Deserialize<TTSVoices>(GetTTSInfoHelperOutput()) ?? throw new Exception("Missing voices");
			if (tTSVoices.Error != 0)
			{
				throw new Exception($"Voices error: {tTSVoices.Error}");
			}
			if (tTSVoices.SAPI4.Error == 0)
			{
				if (tTSVoices.SAPI4.Voices.Any())
				{
					_viewModel.TTSEngines.Add(TTSEngine.SAPI4);
				}
				else
				{
					Logger.Instance.Warn("No SAPI4 voices available");
				}
			}
			else
			{
				Logger.Instance.Error($"SAPI4 voices error: {tTSVoices.SAPI4.Error}");
			}
			if (tTSVoices.SAPI5.Error == 0)
			{
				if (tTSVoices.SAPI5.Voices.Any())
				{
					_viewModel.TTSEngines.Add(TTSEngine.SAPI5);
				}
				else
				{
					Logger.Instance.Warn("No SAPI5 voices available");
				}
			}
			else
			{
				Logger.Instance.Error($"SAPI5 voices error: {tTSVoices.SAPI4.Error}");
			}
			if (tTSVoices.SAPI4.Error != 0 && tTSVoices.SAPI5.Error != 0)
			{
				throw new Exception("Both voices failed!");
			}
			if (!_viewModel.TTSEngines.Any())
			{
				_viewModel.TTSError = "No voices available! Install some voices to enable TTS functionality.";
				throw new Exception("No voices installed");
			}
			if (!_viewModel.TTSEngines.Contains(_viewModel.TTSEngine))
			{
				_viewModel.TTSEngine = _viewModel.TTSEngines[0];
			}
			_ttsVoices = tTSVoices;
			UpdateTTSVoices(_viewModel.TTSEngine, changeSelectedVoice: false);
			if (!_viewModel.TTSVoices.Contains(_viewModel.TTSVoice))
			{
				_viewModel.TTSVoice = _viewModel.TTSVoices[0];
			}
			_viewModel.TTSErrorVisibility = Visibility.Collapsed;
			_viewModel.TTSCustomisationVisibility = Visibility.Visible;
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"Failed to get TTS voices: {value}");
			_viewModel.TTSErrorVisibility = Visibility.Visible;
			_viewModel.TTSCustomisationVisibility = Visibility.Collapsed;
			_viewModel.TTSEnabled = false;
		}
	}

	private TTSEngineVoices GetTTSEngineVoicesFromEnum(TTSEngine engine)
	{
		return engine switch
		{
			TTSEngine.SAPI4 => _ttsVoices.SAPI4, 
			TTSEngine.SAPI5 => _ttsVoices.SAPI5, 
			_ => throw new Exception("Unknown TTSEngine"), 
		};
	}

	private void UpdateTTSVoices(TTSEngine engine, bool changeSelectedVoice)
	{
		if (_currentTTSEngine == engine)
		{
			return;
		}
		_viewModel.TTSVoices.Clear();
		foreach (string voice in GetTTSEngineVoicesFromEnum(engine).Voices)
		{
			_viewModel.TTSVoices.Add(voice);
		}
		if (changeSelectedVoice)
		{
			_viewModel.TTSVoice = _viewModel.TTSVoices[0];
		}
		_currentTTSEngine = engine;
	}

	private void OnTTSEngineComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		UpdateTTSVoices(_viewModel.TTSEngine, changeSelectedVoice: true);
	}

}
