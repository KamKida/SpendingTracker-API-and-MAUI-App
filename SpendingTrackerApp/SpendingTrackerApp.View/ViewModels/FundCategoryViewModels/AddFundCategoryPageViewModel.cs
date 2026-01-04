using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundCategoryViewModels
{
	public class AddFundCategoryPageViewModel : INotifyPropertyChanged
	{
		private FundCategoryRequest _fundCategoryRequest;
		private IFundCategotuService _fundCategoryService;
		private ILogger<AddFundCategoryPageViewModel> _logger;

		private string _message = "";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _nameEntryColor = Colors.White;
		private Color _shouldBeEntryColor = Colors.White;
		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

		public FundCategoryRequest FundCategoryRequest
		{
			get => _fundCategoryRequest;
			set
			{
				if (_fundCategoryRequest != null)
				{
					_fundCategoryRequest = value;
					OnPropertyChanged(nameof(FundCategoryRequest));
				}
			}
		}

		public string Message
		{
			get => _message;
			set
			{
				if (_message != value)
				{
					_message = value;
					OnPropertyChanged(nameof(Message));
				}
			}
		}

		public Color MessageColor
		{
			get => _messageColor;
			set
			{
				if (_messageColor != value)
				{
					_messageColor = value;
					OnPropertyChanged(nameof(MessageColor));
				}
			}
		}

		public Color NameEntryColor
		{
			get => _nameEntryColor;
			set
			{
				if (_nameEntryColor != value)
				{
					_nameEntryColor = value;
					OnPropertyChanged(nameof(NameEntryColor));
				}
			}
		}

		public Color ShouldBeEntryColor
		{
			get => _shouldBeEntryColor;
			set
			{
				if (_shouldBeEntryColor != value)
				{
					_shouldBeEntryColor = value;
					OnPropertyChanged(nameof(ShouldBeEntryColor));
				}
			}
		}

		public bool ShowLoadingIcon
		{
			get => _showLoadingIcon;
			set
			{
				if (_showLoadingIcon != value)
				{
					_showLoadingIcon = value;
					OnPropertyChanged(nameof(ShowLoadingIcon));
				}
			}
		}

		public bool RunLoadingIcon
		{
			get => _runLoadingIcon;
			set
			{
				if (_runLoadingIcon != value)
				{
					_runLoadingIcon = value;
					OnPropertyChanged(nameof(RunLoadingIcon));
				}
			}
		}

		public bool BlockInteraction
		{
			get => _blockInteraction;
			set
			{
				if (_blockInteraction != value)
				{
					_blockInteraction = value;
					OnPropertyChanged(nameof(BlockInteraction));
				}
			}
		}

		public AddFundCategoryPageViewModel(
		IFundCategotuService fundCategoryService,
		ILogger<AddFundCategoryPageViewModel> logger)
		{
			_fundCategoryService = fundCategoryService;
			_logger = logger;
			_fundCategoryRequest = new FundCategoryRequest();

			AddFundCategoryCommand = new Command(async () => await AddFundCategory());
			CancelAddCommand = new Command(async () => await CancelAdd());
		}
		public ICommand AddFundCategoryCommand { get; }
		public ICommand CancelAddCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu funduszu i błędów UI.");

			FundCategoryRequest = new FundCategoryRequest();
			Message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa jest wymagana";
			MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];
			NameEntryColor = Colors.White;
			_logger.LogInformation("Zakończono resetowanie stanu funduszu i błędów UI.");
		}

		public async Task AddFundCategory()
		{
			_logger.LogInformation(
					"Rozpoczynam dodawanie kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}",
					FundCategoryRequest.Name,
					FundCategoryRequest.ShouldBe
				);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (String.IsNullOrEmpty(FundCategoryRequest.Name))
				{
					_logger.LogWarning(
						"Brak nazwy kategori. Name={Name}, ShouldBe={ShouldBe}",
						FundCategoryRequest.Name,
						FundCategoryRequest.ShouldBe
					);
					Message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.  Nazwa jest wymagana";
					MessageColor = NameEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}


				bool goodShouldBe = CheckShouldBe();
				if (!goodShouldBe)
				{
					_logger.LogWarning(
						"Niepoprawna kwota kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}",
						FundCategoryRequest.Name,
						FundCategoryRequest.ShouldBe
					);
					Message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.  Nazwa jest wymagana";

					MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				var response = await _fundCategoryService.AddFundCategory(FundCategoryRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Dodawanie kategorii funduszy nie powiodło się. Name={Name}, ShouldBe={ShouldBe}, StatusCode={StatusCode}, Content={Content}",
						FundCategoryRequest.Name,
						FundCategoryRequest.ShouldBe,
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak. Kategoria o tej nazwie już istnieje. Jeśli nie, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				MessageColor = (Color)Application.Current.Resources["Positive"];
				ShouldBeEntryColor = NameEntryColor = Colors.White;
				Message = "Kategoria funduszy dodana pomyślnie.";

				_logger.LogInformation(
					"Kategoria funduszy dodana pomyślnie. Name={Name}, ShouldBe={ShouldBe}",
					FundCategoryRequest.Name,
					FundCategoryRequest.ShouldBe
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}",
					FundCategoryRequest.Name,
					FundCategoryRequest.ShouldBe
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}",
					FundCategoryRequest.Name,
					FundCategoryRequest.ShouldBe
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces dodawania kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}",
					FundCategoryRequest.Name,
					FundCategoryRequest.ShouldBe
				);
			}
		}



		private bool CheckShouldBe()
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie kwoty kategorii funduszy. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			if (FundCategoryRequest.ShouldBe <= 0)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy jest mniejsza lub równa zero. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			string amountStr = FundCategoryRequest.ShouldBe.ToString();
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 15 cyfr przed przecinkiem. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 2 miejsca po przecinku. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota kategorii funduszy jest poprawna. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			return true;
		}


		public async Task CancelAdd()
		{
			_logger.LogInformation("Rozpoczynam anulowanie dodawania kategori funduszu (powrót do histori funduszy).");

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation("Nawigacja po anulowaniu dodawania kategori funduszu zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania dodawania kategori funduszu."
				);
				throw;
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}

